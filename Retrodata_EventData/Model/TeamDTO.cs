using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class TeamDTO
    {
        public System.Guid RecordID { get; set; }
        public string ID { get;  set; }
        public string League { get;  set; }
        public string City { get;  set; }
        public string Name { get;  set; }
    }
}
