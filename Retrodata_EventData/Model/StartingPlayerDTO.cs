using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class StartingPlayerDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        // key player.ID
        public string PlayerID { get;  set; }

        // home or visiting
        public int GameTeamCode { get;  set; }
        public int BattingOrder { get;  set; }

        // may have to cast from int to string to access reference value
        public int FieldPosition { get;  set; }
        public string TeamId { get; set; }

    }
}
