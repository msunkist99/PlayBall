using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class PlayerDTO
    {
        public System.Guid RecordID { get; set; }
        public string PlayerID { get;  set; }
        public string LastName { get;  set; }
        public string FirstName { get;  set; }
        public string Throws { get;  set; }
        public string Bats { get;  set; }

        // key to Team.ID
        public string TeamID { get;  set; }
        // field position reference data available
        public string Position { get;  set; }
        public string SeasonYear { get; set; }
        public string SeasonGameType { get; set; }
    }
}
