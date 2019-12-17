using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.Daemon
{
    public class DaemonOptions
    {
        public string API_BaseURL { get; set; }
        public string API_TokenAuth { get; set; }
        public string ConnectionString { get; set; }
        public string API_CommercialUrl(string commercialMode) => API_BaseURL + String.Format("commercial_modes/commercial_mode:{0}/lines?", commercialMode);
        public string API_StationUrl(string lineId) => API_BaseURL + String.Format("lines/{0}/stop_areas?", lineId);
    }
}
