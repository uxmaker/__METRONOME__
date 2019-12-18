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
            throw new NotImplementedException();
        }

    }
}
