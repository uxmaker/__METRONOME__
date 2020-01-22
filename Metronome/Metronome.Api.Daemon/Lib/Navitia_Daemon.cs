using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Metronome.Api.DAL.Navitia;
using Metronome.Api.DAL;
using Dapper;
using System.Linq;

namespace Metronome.Api.Daemon.Lib
{
    internal class StationList {  public List<StopAreaData> Stop_areas { get; set; } }

    internal class LineList { public List<LineData> Lines { get; set; }  }

    internal class Ligne { public string id { get; set; } public List<string> Stations { get; set; } }
    internal class RootObject { public List<Ligne> Ligne { get; set; } }
    internal class DisruptionList { public List<DisruptionData> Disruptions { get; set; } }

    internal class ListHorraires { public List<HorrairesData> Departures { get; set; } }
    public class Navitia_Daemon : Daemon
    {
        readonly DaemonOptions _options;
        readonly LineGateway _lineGateway;
        readonly StopAreaGateway _stopAreaGateway;
        readonly JointureGateway _jointureGateway;
        readonly DisruptionGateway _disruptionGateway;
        readonly HorrairesGateway _horrairesGateway;

        // -- PUBLIC METHODS

        public Navitia_Daemon(DaemonOptions options, LineGateway lineGateway, StopAreaGateway stopAreaGateway, JointureGateway jointureGateway, DisruptionGateway disruptionGateway, HorrairesGateway horrairesGateway)
           : base(100000)
        {
            _options = options;
            _lineGateway = lineGateway;
            _stopAreaGateway = stopAreaGateway;
            _jointureGateway = jointureGateway;
            _disruptionGateway = disruptionGateway;
            _horrairesGateway = horrairesGateway;

        }

        // -- OVERRIDE METHODS

        public async override Task Init()
        {
            await GetAndInsertLines();
            
        }

        public async override Task Run()
        {
            await GetInfoTrafic();

        }

        // -- PRIVATE METHODS

        private async Task GetAndInsertLines()
        {
            WebRequest wR = WebRequest.Create(_options.API_CommercialUrl("Metro"));
            wR = ConfigureRequest(wR);

            using (var webRequestResponse = wR.GetResponse())
            {
                using (var streamResponse = new StreamReader(webRequestResponse.GetResponseStream()))
                {
                    var lineList = JsonConvert.DeserializeObject<LineList>(streamResponse.ReadToEnd().ToString());
                    foreach (var line in lineList.Lines)
                    {
                        var opening = DateTime.ParseExact($"{line.Opening_time[0]}{line.Opening_time[1]}:{line.Opening_time[2]}{line.Opening_time[3]}:{line.Opening_time[4]}{line.Opening_time[5]}", "HH:mm:ss", null);
                        var closing = DateTime.ParseExact($"{line.Closing_time[0]}{line.Closing_time[1]}:{line.Closing_time[2]}{line.Closing_time[3]}:{line.Closing_time[4]}{line.Closing_time[5]}", "HH:mm:ss", null);
                        await _lineGateway.Create(line.API_ID, line.Name, line.Color, line.Code, opening, closing);
                        Console.WriteLine($"{line.API_ID} : {line.Name} : {line.Color} : {line.Code} : {line.Opening_time} -- {line.Closing_time}"); 
                        await GetAndInsertStations(line);
                    }

                }
            }
            await InsertJointure();

        }

        private async Task GetAndInsertStations(LineData line)
        {
            WebRequest wR = WebRequest.Create(_options.API_StationUrl(line.API_ID));
            wR = ConfigureRequest(wR);

            using (var webRequestResponse = wR.GetResponse())
            {
                using (var streamResponse = new StreamReader(webRequestResponse.GetResponseStream()))
                {
                    var stationList = JsonConvert.DeserializeObject<StationList>(streamResponse.ReadToEnd().ToString());
                    foreach (var station in stationList.Stop_areas)
                    {
                        await _stopAreaGateway.Create(station.API_ID, station.Name, station.Coord.Lon, station.Coord.Lat);
                        Console.WriteLine($"{station.API_ID} : {station.Name} : {station.Coord.Lon} -- {station.Coord.Lat}");
                    }
                }
            }
            
        }
        private async Task InsertJointure()
        {
            var stream = File.OpenText("../../../ListeStations.json");
            string content = stream.ReadToEnd();
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(content);
            foreach (Ligne li in obj.Ligne)
            {
                for (int i = 0; i < li.Stations.Count - 1; i++)
                {
                    if (li.Stations[i + 1].IndexOf("%") != -1)
                    {
                        int pos = li.Stations[i + 1].IndexOf("%");
                        li.Stations[i + 1] = li.Stations[i + 1].Remove(pos);
                        await _jointureGateway.Create(li.id, li.Stations[i], li.Stations[i + 1]);
                        Console.WriteLine(li.id +"   " + li.Stations[i]);
                    }
                    else
                    {
                        Console.WriteLine(li.id + "   " + li.Stations[i]);
                        await _jointureGateway.Create(li.id, li.Stations[i], li.Stations[i + 1]);
                        await _jointureGateway.Create(li.id, li.Stations[i + 1], li.Stations[i]);
                    }
                }
            }
        }
        public async Task GetInfoTrafic()
        {
            WebRequest wR = WebRequest.Create(_options.API_DisruptionUrl());
            wR = ConfigureRequest(wR);

            using (var webRequestResponse = wR.GetResponse())
            {
                using (var streamResponse = new StreamReader(webRequestResponse.GetResponseStream()))
                {
                    var disruptionList = JsonConvert.DeserializeObject<DisruptionList>(streamResponse.ReadToEnd().ToString());
                    foreach (var dis in disruptionList.Disruptions)
                    {
                        if (dis.Status != "past")
                        {
                            await _disruptionGateway.Create(dis.Status, dis.messages[0].text, dis.API_ID);
                            Console.WriteLine("disruption");
                        }

                    }
                }
            }
            await GetHorrairesLignes();
        }

        public async Task GetHorrairesLignes()
        {
            IEnumerable<LineData> AllLines = await _lineGateway.GetAll();
            AllLines.ToList<LineData>();          // ou AsLIst() 
            foreach (LineData li in AllLines)
            {
                Console.WriteLine("ligne : " +li.Code + "  ----- " +li.API_ID);
                await _horrairesGateway.DeleteByLineId(li.Id);
                WebRequest wR = WebRequest.Create(_options.API_HorraireUrl(li.API_ID));

                wR = ConfigureRequest(wR);
                using (var webRequestResponse = wR.GetResponse())
                {
                    using (var streamResponse = new StreamReader(webRequestResponse.GetResponseStream()))
                    {
                        var horraireslist = JsonConvert.DeserializeObject<ListHorraires>(streamResponse.ReadToEnd().ToString());
                            foreach (var horraire in horraireslist.Departures)
                            {

                                string heurevalide = horraire.stop_date_time.arrival_date_time.Substring(9, 4);
                                int directionpos = horraire.route.name.IndexOf(" - ") + 3;
                                string directionvalide = horraire.route.name.Substring(directionpos);
                                 //string directionvalide = horraire.route.name;
                                await _horrairesGateway.Create(heurevalide, horraire.stop_point.stop_area.Name, li.Id, directionvalide);


                            }
                    }
                }
            }
        }

        

        private WebRequest ConfigureRequest(WebRequest request)
        {
            request.Headers.Add("Authorization", _options.API_TokenAuth);
            request.Method = "GET";
            request.ContentType = "application/json";
            return request;
        }

    }
}
