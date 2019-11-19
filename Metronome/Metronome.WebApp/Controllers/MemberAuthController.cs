// SYSTEM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

// MICROSOFT

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

// METRONOME

using Metronome.WebApp.Core.Auth;
using Metronome.WebApp.Models.MemberAuthModels;
using Metronome.WebApp.DAL;

namespace Metronome.WebApp.Controllers
{
    public class MemberAuthController : Controller
    {

        readonly MemberGateway _memberGateway;
        readonly MemberCookieService _cookieService;
        readonly MemberTokenJwtService _tokenJwtService;


        public MemberAuthController( MemberGateway memberGateway, MemberCookieService cookieService, MemberTokenJwtService tokenJwtService )
        {
            _memberGateway = memberGateway;
            _cookieService = cookieService;
            _tokenJwtService = tokenJwtService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginViewModel model )
        {
            if (ModelState.IsValid)
            {
                MemberData member = new MemberData { uID=1 }; // !!! A CHANGER !!!

                if (model.Email == model.Password)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                    return View(model);
                }

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email, ClaimValueTypes.String),
                    new Claim(ClaimTypes.NameIdentifier, member.uID.ToString(),ClaimValueTypes.String)
                };
                ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(MemberCookie.AuthScheme, ClaimTypes.Email, string.Empty));
                await HttpContext.SignInAsync(MemberCookie.AuthScheme, principal);
                return View(model);
            }
            return View(model);
        }
    
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register( RegisterViewModel model )
        {
            if( ModelState.IsValid )
            {
                // TRY CREATE USER
                
            }
        }
    }
}
