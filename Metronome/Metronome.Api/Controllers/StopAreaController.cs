using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Metronome.Api.Authentication.Cookie;
using Metronome.Api.DAL.Navitia;
using Metronome.Api.Extensions;
using Metronome.Api.Extensions.GeoJson;

namespace Metronome.Api.Controllers
{
    public class StopAreaController : Controller
    {
        StopAreaGateway _stopAreaGateway;

        public StopAreaController(StopAreaGateway stopAreaGateway)
        {
            _stopAreaGateway = stopAreaGateway;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = AuthCookieParameters.Scheme)]
        public async Task<IActionResult> GetStopAreas()
        {
            var stopAreas = await _stopAreaGateway.GetAll();
            StopAreasGJ result = GeoJsonHandler.CreateStopAreasGeoJson(stopAreas);
            return Ok(result);
        }

    }
}
