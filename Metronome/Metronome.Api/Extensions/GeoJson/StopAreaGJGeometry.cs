using System.Collections.Generic;

namespace Metronome.Api.Extensions.GeoJson
{
    public class StopAreaGJGeometry
    {
        public string type { get; set; }
        public List<float> coordinates { get; set; }
    }
}