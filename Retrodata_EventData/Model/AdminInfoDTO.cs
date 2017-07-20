using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class AdminInfoDTO
    {
        public System.Guid RecordID { get; set; }
        public string GameID { get; set; }
        public string AdminInfoType { get; set; }
        public string AdminInfoValue { get; set; }
    }
}
