using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Metronome.Api.Authentication.Jwt
{
    public class AuthTokenService
    {
        readonly AuthTokenOptions _options;

        public AuthTokenService(IOptions<AuthTokenOptions> options)
        {
            _options = options.Value;
        }

        public AuthToken GenerateToken(string userId, string email)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim( JwtRegisteredClaimNames.Sub, userId ),
                new Claim( JwtRegisteredClaimNames.Email, email ),
                new Claim( JwtRegisteredClaimNames.Iat, ( ( int )( now - new DateTime( 1970, 1, 1 ) ).TotalSeconds).ToString(), ClaimValueTypes.Integer64 )
            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthToken(encodedJwt, (int)_options.Expiration.TotalSeconds);
        }
    }
}
