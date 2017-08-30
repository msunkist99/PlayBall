using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrosheet_RetrieveData
{
    public class TreeViewModels
    {
        public class Season
        {
            public string SeasonYear { get; set; }
            public string SeasonIcon { get; set; }
            public List<SeasonGameType> SeasonGameTypes { get; set; }
        }

        public class SeasonGameType
        {
            public string GameType { get; set; }
            public string GameTypeDesc { get; set; }
            public string GameTypeSortKey { get; set; }
        }

        public class League
        {
            public string LeagueID { get; set; }
            public string LeagueDesc { get; set; }
            public string LeagueIcon { get; set; }
            public List<Team> Teams { get; set; }
        }

        public class Team
        {
            public string TeamID { get; set; }
            public string TeamName { get; set; }
            public string TeamCity { get; set; }
            public string TeamIcon { get; set; }
            public List<Game> Games { get; set; }
        }

        public class Game
        {
            public string GameID { get; set; }
            public string GameDate { get; set; }
            public string GameHomeTeamDesc { get; set; }
            public string GameVisitTeamDesc { get; set; }
            //  home or away
            public string GameLocation { get; set; }
        }
    }
}
