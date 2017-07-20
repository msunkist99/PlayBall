using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class GameDataDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get;  set; }
        // currently the only data type is ER - earned runs allowed by the pitcher
        public string DataType { get;  set; }
        // key player.ID
        public string PlayerID { get;  set; }
        // for DataType = ER the DataValue is the number of earned runs allowed by the pitcher
        public string DataValue { get;  set; }


    }
}
