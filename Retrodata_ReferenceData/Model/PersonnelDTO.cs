using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_ReferenceData.Model
{
    public class PersonnelDTO
    {
        public System.Guid RecordID { get; set; }
        public string PersonID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime CareerDate { get; set; }
        public string Role { get; set; }
    }
}
