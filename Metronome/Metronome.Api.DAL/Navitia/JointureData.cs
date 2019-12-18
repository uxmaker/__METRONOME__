using System;
using System.Collections.Generic;
using System.Text;

namespace Metronome.Api.DAL.Navitia
{
    public class JointureData
    {
        public int Id { get; set; }
        public int LigneId { get; set; }
        public int StopAreaId1 { get; set; }
        public int StopAreaId2 { get; set; }
    }
}
