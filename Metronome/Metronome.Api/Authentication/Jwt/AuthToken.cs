using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metronome.Api.Authentication.Jwt
{
    public class AuthToken
    {
        public string AccessToken { get; }
        public int ExpiresIn { get; }

        public AuthToken(string accessToken, int expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}
