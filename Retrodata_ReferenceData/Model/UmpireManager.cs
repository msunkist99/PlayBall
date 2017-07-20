using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_ReferenceData.Model
{
    class UmpireManager
    {
        public string ID { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime DebutDate { get; private set; }
    }
}
