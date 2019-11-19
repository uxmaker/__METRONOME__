// SYSTEM

using System;

// MICROSOFT

using Microsoft.IdentityModel.Tokens;

namespace Metronome.WebApp.Core.Auth
{
    public class MemberJwtOptsProvider
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
