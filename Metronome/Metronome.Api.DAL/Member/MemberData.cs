using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL.Member
{
    public class MemberData
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public byte[] Password { get; set; }
    }
}
