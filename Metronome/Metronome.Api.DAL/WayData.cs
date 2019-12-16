using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class WayData
    {
        public int id { get; set; }

        public int departureID { get; set; }

        public int stopID { get; set; }

        public TimeSpan Tmoy { get; set; }

        public int lineID { get; set; }
    }
}
