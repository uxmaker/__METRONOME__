using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Metronome.Api.DAL.Navitia
{
    public class LineData
    {
        public int Id { get; set; }

        [JsonProperty("id")]
        public string API_ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Color { get; set; }

        public string Opening_time { get; set; }

        public string Closing_time { get; set; }
    }
}
