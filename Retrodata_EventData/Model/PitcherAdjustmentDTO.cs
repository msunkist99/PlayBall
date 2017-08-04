using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class PitcherAdjustmentDTO
    {
        public System.Guid RecordID { get; set; }

        // this happened only once when Greg Harris of the Expos,
        // a right handed pitcher, pitched left-handed to two Cincinnati
        // batters on 9/28/1995.

        // see Seattle Mariners 9/30/2016 vendp001
        public string GameID { get;  set; }
        public int Inning { get;  set; }
        public int GameTeamCode { get; set; }
        public int Sequence { get;  set; }
        // key player.ID
        public string PlayerID { get;  set; }
        public string PitcherHand { get;  set; }
        public string TeamID { get; set; }
    }
}
