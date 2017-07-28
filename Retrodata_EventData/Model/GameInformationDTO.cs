using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_EventData.Model
{
    public class GameInformationDTO
    {
        public System.Guid RecordID { get; set; }

        public string GameID { get;  set; }

        // key to Team.ID
        public string VisitingTeam_ID { get;  set; }

        // key to Team.ID
        public string HomeTeam_ID { get;  set; }

        public DateTime GameDate { get;  set; }
        public int GameNumber { get;  set; }
        public string StartTime { get;  set; }
        public string DayNight { get;  set; }
        public bool UsedDH { get;  set; }
        public string Pitches { get;  set; }

        // key to UmpireManager.ID
        public string UmpireHome_ID { get;  set; }
        // key to UmpireManager.ID
        public string UmpireFirstBaseID { get;  set; }
        // key to UmpireManager.ID
        public string UmpireSecondBaseID { get;  set; }
        // key to UmpireManager.ID
        public string UmpireThirdBaseID { get;  set; }

        public string FieldCondition { get;  set; }
        public string Precipitation { get;  set; }
        public string Sky { get;  set; }
        public int Temperature { get;  set; }
        public string WindDirection { get;  set; }
        public int WindSpeed { get;  set; }

        public int GameTimeLengthMinutes { get;  set; }
        public int Attendance { get;  set; }
        //key BallPark.ID
        public string BallPark_ID { get;  set; }

        // key Player.ID
        public string WinningPitcherID { get;  set; }
        // key Player.ID
        public string LosingPitcherID { get;  set; }
        // key Player.ID
        public string SavePitcherID { get;  set; }

        // key Player.id
        public string WinningRBIPlayerID { get;  set; }

        public string OScorer { get; set; }
        public string SeasonYear { get; set; }
        public string SeasonGameType { get; set; }
        public string EditType { get; set; }
        public string HowScored { get; set; }
        public string InputProgVers { get; set; }
        public string Inputter { get; set; }
        public string InputTime { get; set; }
        public string Scorer { get; set; }
        public string Translator { get; set; }
    }
}
