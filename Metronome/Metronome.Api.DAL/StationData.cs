using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class StationData
    {
        public int id { get; set; }

        public string api_id { get; set; }

        public string name { get; set; }

        public int x { get; set; }

        public int y { get; set; }
    }
}
