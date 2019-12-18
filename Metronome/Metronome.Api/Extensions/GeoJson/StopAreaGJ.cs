using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.Extensions.GeoJson
{
    public class StopAreasGJ
    {
        public string type { get; set; }

        public List<StopAreaGJ> features { get; set; }
    }
}
