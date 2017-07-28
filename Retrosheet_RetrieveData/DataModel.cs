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
            public string GameNumberDesc { get; set; }
            public TeamDTO VisitingTeam {get; set;}
            public TeamDTO HomeTeam { get; set; }
            public PersonnelDTO UmpireHome { get; set; }
            public PersonnelDTO UmpireFirstBase { get; set; }
            public PersonnelDTO UmpireSecondBase { get; set; }
            public PersonnelDTO UmpireThirdBase { get; set; }
            public string WindDirectionDesc { get; set; }
            public BallparkDTO BallPark { get; set; }
            public PlayerDTO WinningPitcher { get; set; }
            public PlayerDTO LosingPitcher { get; set; }
            public PlayerDTO SavePitcher { get; set; }
            public PlayerDTO WinningRBIPlayer { get; set; }
            public string SeasonGameTypeDesc { get; set; }
        }
    }

   


}
