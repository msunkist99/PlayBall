using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_ReferenceData.Model
{
    public class BallparkDTO
    {
        public System.Guid RecordID { get; set; }
        public string ID { get;  set; }
        public string Name { get;  set; }
        public string AKA { get;  set; }
        public string City { get;  set; }
        public string State { get;  set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get;  set; }
        public string League { get;  set; }
        public string Notes { get;  set; }
    }
}
