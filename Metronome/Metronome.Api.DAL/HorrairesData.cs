using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL
{
    public class HorrairesData
    {

        public StopPoint stop_point { get; set; }
        public StopDateTime stop_date_time { get; set; }


    }
    public class StopDateTime
    {
        public string arrival_date_time { get; set; }
    }
    public class StopPoint
    {
        public string name { get; set; }
    }
}