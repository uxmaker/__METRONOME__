using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Metronome.Api.Controllers
{
    public class TransportController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> GetTrains(int stopAreaId)
        {
            var result = new { line6 =  new Random().Next(21), line8 = new Random().Next(21) };
            //result = Json(result);
            return Ok(result);
        }

    }
}
