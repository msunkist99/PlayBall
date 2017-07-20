using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    class TeamBattingOrderAdjustment
    {
        public string GameID { get; private set; }
        public int Inning { get; private set; }
        public int Sequence { get; private set; }
        //  cannot find the layout for the LADJ record or any samples
        public string RestOfTheRecord { get; private set; }
    }
}
