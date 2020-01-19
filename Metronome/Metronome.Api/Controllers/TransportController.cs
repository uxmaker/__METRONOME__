using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Metronome.Api.DAL;
using Metronome.Api.DAL.Navitia;
using Microsoft.AspNetCore.Mvc;

namespace Metronome.Api.Controllers
{
    public class TransportController : Controller
    {
        readonly HorrairesGateway _HorrairesGateway;
        readonly StopAreaGateway _StopAreaGateway;
        readonly LineGateway _LineGateway;

        [HttpGet]
        public async Task<IActionResult> GetTrains(string  stopArea)
        {
            StopAreaData station = await _StopAreaGateway.FindByName(stopArea);
            int stopAreaId = station.Id;
            List<int> listLignes = new List<int>();
            List<HorrairesResponse> result = new List<HorrairesResponse>();
            /*
             * line 8 { hour , direction }, line 7 {hour , direction }
            */
            //var result = new { line6 =  new Random().Next(21), line8 = new Random().Next(21) };
            //result = Json(result);
            listLignes = await _StopAreaGateway.GetLines(stopAreaId);
            foreach (int l in listLignes)
            {
                string codeligne = await _LineGateway.GetCode(l);
                IEnumerable<HorrairesData> AllHorraires = await _HorrairesGateway.GetHorraireByStopArea(stopAreaId, l);
                AllHorraires.ToList<HorrairesData>();
                foreach (HorrairesData h in AllHorraires)
                {
                    HorrairesResponse newHorraire = new HorrairesResponse(codeligne, h.Arrival_time, h.Direction);
                    result.Add(newHorraire);
                }


            }

            return Ok(result);
        }

        public class HorrairesResponse
        {
            public string lignecode { get; set; }

            public string Hour { get; set; }

            public string Direction { get; set; }

            public HorrairesResponse(string codeligne, string arrivaltime, string direction)
            {
                lignecode = codeligne;
                Hour = arrivaltime;
                Direction = direction;
            }
        }

    }
}
