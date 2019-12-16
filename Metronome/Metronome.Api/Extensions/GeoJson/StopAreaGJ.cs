using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metronome.Api.Extensions.GeoJson
{
    public class StopAreaGJ
    {
        public string type { get; set; }
        
        public StopAreaGJGeometry geometry { get; set; }

        public StopAreaGJProperties properties { get; set; } 
    }
}
