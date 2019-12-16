using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class LinkData
    {
        public int id { get; set; }

        public int stationAID { get; set; }

        public int stationBID { get; set; }

        public TimeSpan Tmoy {get; set; }

        public int state { get; set; }
    }
}
