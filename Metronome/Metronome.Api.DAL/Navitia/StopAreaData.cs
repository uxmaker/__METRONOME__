using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Metronome.Api.DAL.Navitia
{
    public class StopAreaData
    {
        public int Id { get; set; }

        [JsonProperty("id")]
        public string API_ID { get; set; }

        public string Name { get; set; }

        public GeoPos Coord { get; set; }   // <=  NAVITIA JSON RESPONSE

        public float Lon { get; set; }      // <=> SQL

        public float Lat { get; set; }      // <=> SQL
    }

    public class GeoPos
    {
        public float Lon { get; set; }

        public float Lat { get; set; }
    }
}
