using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_ReferenceData.Model
{
    public class ReferenceDataDTO
    {
        public System.Guid RecordID { get; set; }
        public string ReferenceDataType { get; set; }
        public string ReferenceDataCode { get; set; }
        public string ReferenceDataDescription { get; set; }
    }
}
