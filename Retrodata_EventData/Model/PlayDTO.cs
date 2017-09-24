using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class PlayDTO
    {
        public System.Guid RecordID { get; set; }

        public string GameID { get;  set; }
        public int Inning { get;  set; }
        // home or visiting
        public int GameTeamCode { get;  set; }
        public int Sequence { get;  set; }
        // key player.ID
        public string PlayerID { get;  set; }
        public int CountBalls { get; set; }
        public int CountStrikes { get; set; }
        public string Pitches { get; set; }
        public string EventSequence { get; set; }
        public string EventModifier { get;  set; }
        public string EventRunnerAdvance { get;  set; }
        public string EventHitLocation { get; set; }
        public string EventFieldedBy { get; set; }
        public string EventPlayOnRunner { get; set; }
        public string EventType { get; set; }
        public string EventColumnSix { get; set; }
        public int EventNum { get; set; }
    }
}
