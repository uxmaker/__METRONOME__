﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Metronome.Api.DAL.Navitia;
using Metronome.Api.DAL;

namespace Metronome.Api.Daemon.Lib
{
    internal class StationList {  public List<StopAreaData> Stop_areas { get; set; } }

    internal class LineList { public List<LineData> Lines { get; set; }  }

    internal class Ligne { public string id { get; set; } public List<string> Stations { get; set; } }
    internal class Jointure { public List<Ligne> Lignes { get; set; } }

    internal class ListHorraires { public List<HorrairesData> Horraires { get; set; } }
    public class Navitia_Daemon : Daemon
    {
        readonly DaemonOptions _options;
        readonly LineGateway _lineGateway;
        readonly StopAreaGateway _stopAreaGateway;
        readonly JointureGateway _jointureGateway;

        // -- PUBLIC METHODS

        public Navitia_Daemon(DaemonOptions options, LineGateway lineGateway, StopAreaGateway stopAreaGateway)
            :base(524160000)
        {
            _options = options;
            _lineGateway = lineGateway;
            _stopAreaGateway = stopAreaGateway;
            
        }

        // -- OVERRIDE METHODS

        public async override Task Init()
        {
            await GetAndInsertLines();
            await InsertJointure();
        }

        public async override Task Run() { await Task.Delay(100); }
        
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
            var stream = File.OpenText("C:/Users/bapti/OneDrive/Bureau/ListeStations.json");
            string content = stream.ReadToEnd();
            Jointure obj = JsonConvert.DeserializeObject<Jointure>(content);
            foreach (Ligne li in obj.Lignes)
            {
                for (int i = 0; i < li.Stations.Count - 1; i++)
                {
                    if (li.Stations[i + 1].IndexOf("%") != -1)
                    {
                        int pos = li.Stations[i + 1].IndexOf("%");
                        li.Stations[i + 1] = li.Stations[i + 1].Remove(pos);
                        await _jointureGateway.Create(li.id, li.Stations[i], li.Stations[i + 1]);
                    }
                    else
                    {
                        await _jointureGateway.Create(li.id, li.Stations[i], li.Stations[i + 1]);
                        await _jointureGateway.Create(li.id, li.Stations[i + 1], li.Stations[i]);
                    }
                }
            }
        }

        // Pas encore fonctionnelle
        /*public async Task GetHorrairesLignes()
        {
            Task<IEnumerable<LineData>> AllLines = _lineGateway.GetAll();
            List<LineData> aaa = new List<LineData>();
            foreach (LineData li in aaa)
            {
                WebRequest wR = WebRequest.Create(_options.API_StationUrl(li.API_ID));
                wR = ConfigureRequest(wR);
                using (var webRequestResponse = wR.GetResponse())
                {
                    using (var streamResponse = new StreamReader(webRequestResponse.GetResponseStream()))
                    {
                        var horraireslist = JsonConvert.DeserializeObject<ListHorraires>(streamResponse.ReadToEnd().ToString());
                        foreach (var horraire in horraireslist.Horraires)
                        {
                            Console.WriteLine(horraire.stop_point.name);
                            Console.WriteLine(horraire.stop_date_time.arrival_date_time);

                        }
                    }
                }
            }
        }*/

        private WebRequest ConfigureRequest(WebRequest request)
        {
            request.Headers.Add("Authorization", _options.API_TokenAuth);
            request.Method = "GET";
            request.ContentType = "application/json";
            return request;
        }

    }
}