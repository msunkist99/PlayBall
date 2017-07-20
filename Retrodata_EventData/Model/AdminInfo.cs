using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    class AdminInfo
    {
        public string GameID { get; private set; }
        public DateTime EditTime { get; private set; }
        public string HowScored { get; private set; }
        public string InputProgVersion { get; private set; }
        public string Inputter { get; private set; }
        public DateTime InputTime { get; private set; }
        public string Scorer { get; private set; }
        public string Translator { get; private set; }
    }
}
