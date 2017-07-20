using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class GameInfoDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get; set; }
        public string GameInfoType { get; set; }
        public string GameInfoValue { get; set; }
    }
}
