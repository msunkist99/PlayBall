using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class ReplayDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        public int Inning { get;  set; }
        public int GameTeamCode { get; set; }
        public int Sequence { get;  set; }
        public int ComSequence { get; set; }
        // key player.ID
        public string PlayerID { get;  set; }
        // key team.ID
        public string TeamID { get;  set;  }
        // key umpire manager.ID
        public string UmpireID { get;  set; }
        // key Balpark.ID
        public string BallparkID { get;  set; }
        // reason reference data available
        public string Reason { get;  set; }
        public bool Reversed { get;  set; }
        // initiated reference data available
        public string Initiated { get;  set; }
        // key team.ID - populated only for manager initiated replay
        public string ReplayChallengeTeamID { get;  set; }
        // type  reference data available
        public string Type { get;  set; }
    }

}
