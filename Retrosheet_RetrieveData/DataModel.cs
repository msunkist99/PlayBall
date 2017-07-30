using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_EventData.Model;
using Retrosheet_ReferenceData.Model;

namespace Retrosheet_RetrieveData
{
    public class DataModels
    {
        public class GameInformation : GameInformationDTO
        {
            public BallparkInformation Ballpark = new BallparkInformation();
            public TeamInformation VisitingTeam = new TeamInformation();
            public TeamInformation HomeTeam = new TeamInformation();
            public UmpireInformation UmpireHome = new UmpireInformation();
            public UmpireInformation UmpireFirst = new UmpireInformation();
            public UmpireInformation UmpireSecond = new UmpireInformation();
            public UmpireInformation UmpireThird = new UmpireInformation();
            public PlayerInformation WinningPitcher = new PlayerInformation();
            public PlayerInformation LosingPitcher = new PlayerInformation();
            public PlayerInformation SavePitcher = new PlayerInformation();
            public PlayerInformation WinningRBIPlayer = new PlayerInformation();

            public string GameNumberDesc { get; set; }
            public string WindDirectionDesc { get; set; }
            public string SeasonGameTypeDesc { get; set; }
        }

        public class UmpireInformation 
        {
            public PersonnelDTO UmpirePersonel = new PersonnelDTO();
            public string UmpireRole { get; set; }
        }

        public class ReferenceData
        {
            public ReferenceDataDTO ReferenceItem = new ReferenceDataDTO();
        }

        public class PlayerInformation
        {
            public PlayerDTO Player = new PlayerDTO();
            public string GameID { get; set; }
            public string PlayerTeam { get; set; }
            public string PlayerPositionDesc { get; set; }
        }

        public class StartingPlayerInformation
        {
            public StartingPlayerDTO StartingPlayer = new StartingPlayerDTO();
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Throws { get; set; }
            public string Bats { get; set; }
            public string FieldPostionDesc { get; set; }
        }

        public class TeamInformation
        {
            public TeamDTO Team = new TeamDTO();
            public string TeamLeagueDesc { get; set; }
        }

        public class BallparkInformation
        {
            public BallparkDTO Ballpark = new BallparkDTO();
            public string BallparkLeagueDesc { get; set; }
        }
    }

   


}
