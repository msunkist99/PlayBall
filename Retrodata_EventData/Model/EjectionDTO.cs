using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class EjectionDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        public int Inning { get;  set; }
        // home or visiting
        public int GameTeamCode { get; set; }

        public int Sequence { get;  set; }        
        public int ComSequence { get; set; }

        // key to player.ID or umpireManager.ID
        public string PlayerID { get;  set; }

        // ejection reference data available
        public string JobCode { get;  set; }
        // key to umpiremanager.ID
        public string UmpireID { get;  set; }
        public string Reason { get;   set; }
    }
}
