using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Metronome.Api.DAL.Navitia;
using Metronome.Api.Extensions.GeoJson;

namespace Metronome.Api.Extensions
{
    public class GeoJsonHandler
    {
        public static StopAreasGJ CreateStopAreasGeoJson(List<StopAreaData> stopAreas)
        {
            List<StopAreaGJ> features = new List<StopAreaGJ>();

            foreach(var stopArea in stopAreas)
            {
                var position = new List<float>()
                {
                    stopArea.Lat,
                    stopArea.Lon
                };

                var properties = new StopAreaGJProperties()
                {
                    title = stopArea.Name,
                    ligne = stopArea.Id.ToString()
                };

                var stopAreaGJ = new StopAreaGJ()
                {
                    type = "Feature",
                    geometry = new StopAreaGJGeometry() { type = "Point", coordinates = position },
                    properties = properties
                };
                features.Add(stopAreaGJ);
            }

            StopAreasGJ result = new StopAreasGJ()
            {
                type = "FeatureCollection",
                features = features
            };

            return result;
        }

    }
}
