using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Metronome.Api.DAL.Navitia
{
    public class DisruptionData
    {
        public int Id { get; set; }

        [JsonProperty("id")]
        public string API_ID { get; set; }
        public List<ImpactedObject> impacted_objects { get; set; }   // <=  NAVITIA JSON RESPONSE

        public List<Messages> messages { get; set; }   // <=  NAVITIA JSON RESPONSE

        public string TextMessage { get; set; }  // <=> SQL

        public string Status { get; set; }

       
    }

    public class Messages
    {
        public string text { get; set; }
    }
    public class ImpactedObject
    {
        public PtObject pt_object { get; set; }
    }
    public class PtObject
    {
        public LineData line { get; set; }
    }
}
