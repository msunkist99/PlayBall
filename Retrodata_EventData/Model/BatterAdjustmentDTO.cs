using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class BatterAdjustmentDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get; set; }
        public int Inning { get; set; }
        public int Sequence { get; set; }
        // home or visiting
        public int GameTeamCode { get;  set; }
        // key to player.ID or umpireManager.ID
        public string PlayerID { get; set; }
        public string Bats { get; set; }
        public string TeamID { get; set; }

    }
}
