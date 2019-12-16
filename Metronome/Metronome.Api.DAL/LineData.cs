using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class LineData
    {
        public int id { get; set; }

        public string api_id { get; set; }

        public string name { get; set; }

        public string color { get; set; }

        public DateTime openingTime { get; set; }

        public DateTime closingTime { get; set; }

        public string code { get; set; }
    }
}
