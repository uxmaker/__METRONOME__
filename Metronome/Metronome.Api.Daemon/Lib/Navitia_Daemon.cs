using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.Daemon.Lib
{
    internal class StationList {  public List<StopAreaData> Stop_areas { get; set; } }

    internal class LineList { public List<LineData> Lines { get; set; }  }

    public class Navitia_Daemon : Daemon
    {
        readonly DaemonOptions _options;
        readonly LineGateway _lineGateway;
        readonly StopAreaGateway _stopAreaGateway;

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

        private WebRequest ConfigureRequest(WebRequest request)
        {
            request.Headers.Add("Authorization", _options.API_TokenAuth);
            request.Method = "GET";
            request.ContentType = "application/json";
            return request;
        }

    }
}
