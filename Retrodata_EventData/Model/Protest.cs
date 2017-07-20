using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    class Protest
    {
        public string GameID { get; private set; }
        public int Inning { get; private set; }
        public int Sequence { get; private set; }
        // protest code reference code available
        // this usually at the start of the game
        public string ProtestCode { get; private set; }
    }
}
