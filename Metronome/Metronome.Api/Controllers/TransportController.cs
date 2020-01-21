using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Metronome.Api.DAL;
using Metronome.Api.DAL.Navitia;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Metronome.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class TransportController : Controller
    {
        readonly HorrairesGateway _HorrairesGateway ;
        readonly StopAreaGateway _StopAreaGateway;
        readonly LineGateway _LineGateway;

        public TransportController( HorrairesGateway horrairesGateway, StopAreaGateway stopAreaGateway, LineGateway lineGateway)
        {
            _HorrairesGateway = horrairesGateway;
            _StopAreaGateway = stopAreaGateway;
            _LineGateway = lineGateway;
        }
        [HttpGet("{stopArea}")]
        public async Task<IActionResult> GetTrains(string stopArea)
        {
            string stopAreaDecode = HttpUtility.UrlDecode(stopArea);
            Console.WriteLine(stopAreaDecode);
            //StopAreaData station = await _StopAreaGateway.FindByName(stopAreaDecode);
            //int stopAreaId = station.Id;
            
            int stopAreaId = await _StopAreaGateway.GetId("nation");
            Console.WriteLine(stopAreaId);
            List<HorrairesResponse> result = new List<HorrairesResponse>();
            List<int> listLignes = await _StopAreaGateway.GetLines(stopAreaId);
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
