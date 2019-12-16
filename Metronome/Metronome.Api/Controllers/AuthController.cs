using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Metronome.Api.DAL.Member;
using Metronome.Api.Authentication.Cookie;
using Metronome.Api.Models.MemberViewModels;
using Metronome.Api.Authentication;
using Metronome.Api.Authentication.Jwt;

namespace Metronome.Api.Controllers
{
    public class AuthController : Controller
    {

        readonly MemberGateway _memberGateway;
        readonly PasswordHasher _passwordHasher;
        readonly AuthTokenService _tokenService;
        readonly Random _random;

        public AuthController( MemberGateway memberGateway, PasswordHasher passwordHasher, AuthTokenService tokenService )
        {
            _memberGateway = memberGateway;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _random = new Random();
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
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (ModelState.IsValid)
            {
                var member = await _memberGateway.FindByEmail(model.Email);
                
                if (member.Id != -1)
                {
                    if(_passwordHasher.VerifyHashedPassword(member.Password, model.Password) == PasswordVerificationResult.Success)
                    {
                        await SignInCookie(member.Email, member.Id);
                        return RedirectToAction(nameof(Authenticated));
                    }
                }

                ModelState.AddModelError(string.Empty, "Utilisateur ou mot de passe incorrect.");
                return View(model);
            }
            else { return View(); }
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
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var member = await _memberGateway.FindByEmail(model.Email);

                if (member.Id == -1)
                {
                    var member_id = await _memberGateway.Create(model.Email, _passwordHasher.HashPassword(model.Password));
                    
                    if(member_id != -1)
                    {
                        await SignInCookie(model.Email, member_id);
                        return RedirectToAction(nameof(Authenticated));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Impossible de créer votre compte :'(");
                        return View(model);
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Email déjà utilisé.");
                return View(model);
            }
            else { return View(); }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = AuthCookieParameters.Scheme)]
        public async Task<IActionResult> Authenticated()
        {
            string member_id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string member_email = User.FindFirst(ClaimTypes.Email).Value;

            await Task.Delay(10);

            AuthenticatedModel model = new AuthenticatedModel();

            model.Token = _tokenService.GenerateToken(member_id, member_email);
            model.BreackPadding = GetBreachPadding();
            model.MemberEmail = member_email;
            model.SpaHost = "http://localhost:8080/";
            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = AuthCookieParameters.Scheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthCookieParameters.Scheme);
            ViewData["SpaHost"] = "http://localhost:8080/";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Token()
        {
            var identity = User.Identities.SingleOrDefault(i => i.AuthenticationType == AuthCookieParameters.Type);
            if (identity == null) return Ok(new { Success = false });
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            AuthToken token = _tokenService.GenerateToken(userId, email);
            return Ok(new { Success = true, Bearer = token, Email = email });
        }

        // -- PRIVATE METHOD

        private async Task SignInCookie(string memberEmail, int memberId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, memberEmail, ClaimValueTypes.String),
                new Claim(ClaimTypes.NameIdentifier, memberId.ToString(), ClaimValueTypes.Integer32)
            };

            ClaimsPrincipal cookie_claim = new ClaimsPrincipal(new ClaimsIdentity(
                    claims,
                    AuthCookieParameters.Type,
                    ClaimTypes.Email,
                    string.Empty
                ));

            await HttpContext.SignInAsync(AuthCookieParameters.Scheme, cookie_claim);
        }

        private string GetBreachPadding()
        {
            byte[] data = new byte[_random.Next(64, 256)];
            _random.NextBytes(data);
            return Convert.ToBase64String(data);
        }

    }
}
