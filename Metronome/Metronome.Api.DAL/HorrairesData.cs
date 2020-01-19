using System;
using System.Collections.Generic;
using System.Text;
using Metronome.Api.DAL.Navitia;
using System;

namespace Metronome.Api.DAL
{
    public class HorrairesData
    {
        public int Id { get; set; }
        public string Arrival_time { get; set; }
        public string Direction { get; set; }

        public StopPoint stop_point { get; set; }
        public Route route { get; set; }
        public StopDateTime stop_date_time { get; set; }


    }
    public class StopDateTime
    {
        public string arrival_date_time { get; set; }
    }
    public class StopPoint
    {
        public StopAreaData stop_area { get; set; }
    }
    public class Route
    {
        public string name { get; set; }
    }
}