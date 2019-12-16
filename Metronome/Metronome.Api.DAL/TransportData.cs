using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class TransportData
    {
        public int id { get; set; }

        public string name { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        public int lineID { get; set; }
    }
}
