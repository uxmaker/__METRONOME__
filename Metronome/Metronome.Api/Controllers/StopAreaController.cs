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
        readonly StopAreaGateway _stopAreaGateway;
        readonly LineGateway _lineGateway;

        public StopAreaController(StopAreaGateway stopAreaGateway, LineGateway lineGateway)
        {
            _stopAreaGateway = stopAreaGateway;
            _lineGateway = lineGateway;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = AuthCookieParameters.Scheme)]
        public async Task<IActionResult> GetStopAreas()
        {
            var stopAreas = await _stopAreaGateway.GetAll();
            StopAreasGJ result = await GeoJson.CreateStopAreasGeoJson(stopAreas, _stopAreaGateway, _lineGateway);
            return Ok(result);
        }
    }
}
