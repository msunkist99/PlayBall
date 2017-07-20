using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    class BatterAdjustmentDTO
    {
        public string GameID { get; private set; }
        public int Inning { get; private set; }
        public int Sequence { get; private set; }
        // key player.ID
        public string PlayerID { get; private set; }
        public string BatterHand { get; private set; }
    }
}
