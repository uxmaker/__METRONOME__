using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.Extensions.GeoJson
{
    public class GeoJson
    {
        public async static Task<StopAreasGJ> CreateStopAreasGeoJson(List<StopAreaData> stopAreas, StopAreaGateway stopAreaGateway, LineGateway lineGateway)
        {
            List<StopAreaGJ> features = new List<StopAreaGJ>();
            var lines = await lineGateway.GetAll();
            List<string> ligneCodes = new List<string>();

            foreach (var stopArea in stopAreas)
            {
                ligneCodes.Clear();
                
                var ligneIds = await stopAreaGateway.GetLines(stopArea.Id);

                foreach(var l in lines)
                {
                    foreach(var lineId in ligneIds)
                    {
                        if (l.Id == lineId)
                        {
                            ligneCodes.Add(l.Code);
                        }
                    }
                }

                var position = new List<float>()
                {
                    stopArea.Lon,
                    stopArea.Lat
                };

                var properties = new StopAreaPropertiesGJ()
                {
                    title = stopArea.Name,
                    ligne = new List<string>(ligneCodes)
                };

                var stopAreaGJ = new StopAreaGJ()
                {
                    type = "Feature",
                    geometry = new StopAreaGeometryGJ() { type = "Point", coordinates = position },
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
