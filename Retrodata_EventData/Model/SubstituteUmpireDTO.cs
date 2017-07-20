using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class SubstituteUmpireDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        public int Inning { get;  set; }
        public int Sequence { get;  set; }
        public int ComSequence { get; set; }
        // field position reference data available
        public string FieldPosition { get;  set; }
        // key to umpiremanager.ID
        public string UmpireID { get;  set; }

    }
}
