using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Retrosheet_RetrieveData
{
    public class TreeViewModels
    {
        public class Season : TreeViewItemBase
        {
            public string SeasonYear { get; set; }
            public string SeasonIcon { get; set; }
            public List<SeasonGameType> SeasonGameTypes { get; set; }
        }

        public class SeasonGameType : TreeViewItemBase
        {
            public string GameType { get; set; }
            public string GameTypeDesc { get; set; }
            public string GameTypeSortKey { get; set; }
            public List<League> Leagues { get; set; }
        }

        public class League : TreeViewItemBase
        {
            public string LeagueID { get; set; }
            public string LeagueName { get; set; }
            public string LeagueIcon { get; set; }
            public List<Team> Teams { get; set; }
        }

        public class Team : TreeViewItemBase
        {
            public string TeamID { get; set; }
            public string TeamName { get; set; }
            public string TeamCity { get; set; }
            public string TeamIcon { get; set; }
            public List<Game> Games { get; set; }
        }

        public class Game : TreeViewItemBase
        {
            public string GameID { get; set; }
            public DateTime GameDate { get; set; }
            public string GameHomeTeamName { get; set; }
            public string GameVisitTeamName { get; set; }
            public string GameLocation { get; set; }
            public string GameDesc { get; set; }
        }
    }

    public class TreeViewItemBase : INotifyPropertyChanged
    {
        private bool isSelected;

        public bool IsSelected
        {
            get { return this.isSelected;}
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}