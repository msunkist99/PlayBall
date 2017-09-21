using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Retrosheet_RetrieveData;

namespace Retrosheet_PlayBall
{
    /// <summary>
    /// Interaction logic for Retrosheet_PlayBall_PlayByPlay.xaml
    /// </summary>
    public partial class Retrosheet_PlayBall_PlayByPlay : Page
    {
        private Collection<DataModels.PlayInformation> PlayInformation;
        private Collection<DataModels.ReplayInformation> ReplayInformation;
        private Collection<DataModels.GameCommentInformation> GameCommentInformation;

        public Retrosheet_PlayBall_PlayByPlay(string gameID)
        {
            InitializeComponent();
            ShowsNavigationUI = true;

            RetrieveData retrieveData = new RetrieveData();

            retrieveData.RetrieveReferenceData();

            Collection<DataModels.GameInformation> GameInformation = retrieveData.RetrieveGameInformation(gameID);

            DataModels.GameInformation gameInformation = GameInformation.First<DataModels.GameInformation>();

            Collection<DataModels.PlayerInformation> PlayerInformation = retrieveData.RetrievePlayers(gameInformation.SeasonYear,
                                                                                                       gameInformation.SeasonGameType,
                                                                                                       gameInformation.HomeTeamID,
                                                                                                       gameInformation.VisitingTeamID);

            Collection<DataModels.StartingPlayerInformation> StartingPlayerInformation = retrieveData.RetrieveStartingPlayers(gameInformation.SeasonYear,
                                                                                                                              gameInformation.SeasonGameType,
                                                                                                                              gameInformation.GameID);

            Collection<DataModels.SubstitutePlayerInformation> SubstitutePlayerInformation = retrieveData.RetrieveSubstitutePlayer(gameInformation.SeasonYear,
                                                                                                                                  gameInformation.SeasonGameType,
                                                                                                                                  gameInformation.GameID);
            PlayInformation = retrieveData.RetrievePlay(gameInformation.SeasonYear,
                                                        gameInformation.SeasonGameType,
                                                        gameInformation.HomeTeamID,
                                                        gameInformation.VisitingTeamID,
                                                        gameInformation.GameID);

            ReplayInformation = retrieveData.RetrieveReplay(gameInformation.SeasonYear,
                                                                                                     gameInformation.SeasonGameType,
                                                                                                     gameInformation.GameID);

            Collection<DataModels.BatterAdjustmentInformation> BatterAdjustmentInformation = retrieveData.RetrieveBatterAdjustment(gameInformation.SeasonYear,
                                                                                                                                   gameInformation.SeasonGameType,
                                                                                                                                   gameInformation.HomeTeamID,
                                                                                                                                   gameInformation.VisitingTeamID,
                                                                                                                                   gameInformation.GameID);

            Collection<DataModels.PlayerEjectionInformation> PlayerEjectionInformation = retrieveData.RetrievePlayerEjection(gameInformation.GameID);

            GameCommentInformation = retrieveData.RetrieveGameComment(gameInformation.GameID);

            Collection<DataModels.GameDataInformation> GameDataInformation = retrieveData.RetrieveGameData(gameInformation.GameID);

            Collection<DataModels.PitcherAdjustmentInformation> PitcherAdjustmentInformation = retrieveData.RetrievePitcherAdjustmentInformation(gameInformation.SeasonYear,
                                                                                                                                                 gameInformation.SeasonGameType,
                                                                                                                                                 gameInformation.GameID);

            Collection<DataModels.SubstituteUmpireInformation> SubstituteUmpireInformation = retrieveData.RetrieveSubstituteUmpireInformation(gameInformation.GameID);

            if (gameInformation.GameNumber > 0)
            {
                PageTitle.Text = gameInformation.HomeTeamName + " vs " + gameInformation.VisitTeamName + " @ " + gameInformation.HomeTeamCity + " on "
                    + gameInformation.GameDate.ToShortDateString()
                    + " game " + gameInformation.GameNumber.ToString() + " of 2";
            }
            else
            {
                PageTitle.Text = gameInformation.HomeTeamName + " vs " + gameInformation.VisitTeamName + " @ " + gameInformation.HomeTeamCity + " on "
                    + gameInformation.GameDate.ToShortDateString();
            }


            var startingVisitingPlayers = from startingPlayer in StartingPlayerInformation
                                          where startingPlayer.GameTeamCode == 0
                                          orderby startingPlayer.GameTeamCode, startingPlayer.BattingOrder
                                          select new
                                          {
                                              Batting = startingPlayer.BattingOrderDesc,
                                              Name = string.Format("{0}, {1}", startingPlayer.PlayerLastName, startingPlayer.PlayerFirstName),
                                              Position = startingPlayer.FieldPositionDesc,
                                              BatsThrows = string.Format("{0} / {1}", startingPlayer.BatsDesc, startingPlayer.ThrowsDesc),
                                          };

            var startingHomePlayers = from startingPlayer in StartingPlayerInformation
                                      where startingPlayer.GameTeamCode == 1
                                      orderby startingPlayer.GameTeamCode, startingPlayer.BattingOrder
                                      select new
                                      {
                                          //Batting = startingPlayer.BattingOrder,
                                          Name = string.Format("{0}, {1}", startingPlayer.PlayerLastName, startingPlayer.PlayerFirstName),
                                          Position = startingPlayer.FieldPositionDesc,
                                              BatsThrows = string.Format("{0} / {1}", startingPlayer.BatsDesc, startingPlayer.ThrowsDesc),
                                      };

            dgVisitingPlayers.ItemsSource = startingVisitingPlayers;
            dgHomePlayers.ItemsSource = startingHomePlayers;

            /*var playEvents = from playEvent in PlayInformation
                             where playEvent.GameID == gameInformation.GameID
                             orderby playEvent.Inning,
                                     playEvent.GameTeamCode,
                                     playEvent.Sequence
                             select new
                             {
                                 Inning = playEvent.Inning,
                                 TeamName = playEvent.TeamName,
                                 VisitHome = playEvent.GameTeamCodeDesc,
                                 Name = string.Concat(playEvent.PlayerLastName, ", ", playEvent.PlayerFirstName),
                                 Count = string.Concat(playEvent.CountBalls, " / ", playEvent.CountStrikes),
                                 Pitches = playEvent.PitchDesc,
                                 EventSequenceDesc = playEvent.EventSequenceDesc,
                                 EventModifierDesc = playEvent.EventModifierDesc,
                                 RunnersAdvance = playEvent.EventRunnerAdvance,
                                 HitLocation = playEvent.EventHitLocationDesc,
                             };

            dgPlayEvents.ItemsSource = playEvents;
            */

            dgPlayEvents.ItemsSource = PlayInformation;

            /*
            List<Umpire> umpires = new List<Umpire>();
            umpires.Add(new Umpire() { UmpirePosition = "Umpire Home Plate", UmpireName = string.Concat(gameInformation.UmpireHomeLastName, ", ", gameInformation.UmpireHomeFirstName) });
            umpires.Add(new Umpire() { UmpirePosition = "Umpire First Base", UmpireName = string.Concat(gameInformation.UmpireFirstBaseLastName, ", ", gameInformation.UmpireFirstBaseFirstName) });
            umpires.Add(new Umpire() { UmpirePosition = "Umpire Second Base", UmpireName = string.Concat(gameInformation.UmpireSecondBaseLastName, ", ", gameInformation.UmpireSecondBaseFirstName) });
            umpires.Add(new Umpire() { UmpirePosition = "Umpire Third Base", UmpireName = string.Concat(gameInformation.UmpireThirdBaseLastName, ", ", gameInformation.UmpireThirdBaseFirstName) });

            dgUmpires.ItemsSource = umpires;
            */

            List<GameInfoItem> gameInfoItems = new List<GameInfoItem>();
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "", GameItemValue = gameInformation.SeasonYear + " " + gameInformation.SeasonGameTypeDesc + " game" });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Visiting Team", GameItemValue = gameInformation.VisitTeamLeagueName + " League " + gameInformation.VisitTeamCity + " " + gameInformation.VisitTeamName });
            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Visiting Team League", GameItemValue = gameInformation.VisitTeamLeagueName });
            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Visiting Team Name", GameItemValue = gameInformation.VisitTeamName });
            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Visiting Team City", GameItemValue = gameInformation.VisitTeamCity });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Home Team", GameItemValue = gameInformation.HomeTeamLeagueName + " League " + gameInformation.HomeTeamCity + " " + gameInformation.HomeTeamName });

            // gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Home Team League", GameItemValue = gameInformation.HomeTeamLeagueName });
            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Home Team Name", GameItemValue = gameInformation.HomeTeamName });
            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Home Team City", GameItemValue = gameInformation.HomeTeamCity });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Game Date/Time", GameItemValue = gameInformation.GameDate.ToShortDateString() + "  " + gameInformation.StartTime + " " + gameInformation.DayNight + " game" });

            /*
            if (gameInformation.GameNumber > 1)
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Double Header", GameItemValue = "Game 2" });
            }
            else if (gameInformation.GameNumber > 0)
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Double Header", GameItemValue = "Game 1" });
            }
            else
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Single Game", GameItemValue = "" });
            }
            */

            if (gameInformation.GameNumberDesc != null)
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "", GameItemValue = gameInformation.GameNumberDesc });

            }

            //gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Game Time", GameItemValue = gameInformation.StartTime + " " + gameInformation.DayNight });


            if (gameInformation.UsedDH == "Y")
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "", GameItemValue = "Designated hitter for pitcher" });
            }

            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Field Condition", GameItemValue = gameInformation.FieldCondition });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Precipitation", GameItemValue = gameInformation.Precipitation });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Sky", GameItemValue = gameInformation.Sky });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Tempurature", GameItemValue = gameInformation.Temperature.ToString() + " degrees fahrenheit" });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Wind Direction", GameItemValue = gameInformation.WindDirectionDesc });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Wind Speed", GameItemValue = gameInformation.WindSpeed.ToString() + " mph" });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Game Length", GameItemValue = gameInformation.GameTimeLengthMinutes.ToString("N0") + " minutes" });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Attendance", GameItemValue = gameInformation.Attendance.ToString("N0") });
            if (gameInformation.BallparkAKA != "")
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Ballpark", GameItemValue = gameInformation.BallparkName + " aka " + gameInformation.BallparkAKA });
            }
            else
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Ballpark", GameItemValue = gameInformation.BallparkName });

            }
            if (gameInformation.BallparkNotes != "")
            {
                gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "", GameItemValue = gameInformation.BallparkNotes });
            }

            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Umpire Home Plate", GameItemValue = string.Concat(gameInformation.UmpireHomeLastName, ", ", gameInformation.UmpireHomeFirstName) });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Umpire First Base", GameItemValue = string.Concat(gameInformation.UmpireFirstBaseLastName, ", ", gameInformation.UmpireFirstBaseFirstName) });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Umpire Second Base", GameItemValue = string.Concat(gameInformation.UmpireSecondBaseLastName, ", ", gameInformation.UmpireSecondBaseFirstName) });
            gameInfoItems.Add(new GameInfoItem() { GameItemDesc = "Umpire Third Base", GameItemValue = string.Concat(gameInformation.UmpireThirdBaseLastName, ", ", gameInformation.UmpireThirdBaseFirstName) });

            dgGameInfoItems.ItemsSource = gameInfoItems;
        }

        /*
        private class Umpire
        {
            public string UmpirePosition { get; set; }
            public string UmpireName { get; set; }
        }
        */

        private class GameInfoItem
        {
            public string GameItemDesc { get; set; }
            public string GameItemValue { get; set; }
        }

        private void dbPlayEvents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var index = dg.SelectedIndex;

            var gameComments = from gameComment in GameCommentInformation
                               where gameComment.GameID == PlayInformation[index].GameID
                                  && gameComment.Inning == PlayInformation[index].Inning
                                  && gameComment.GameTeamCode == PlayInformation[index].GameTeamCode
                                  && gameComment.Sequence == PlayInformation[index].Sequence
                               orderby gameComment.CommentSequence
                               select new
                               {
                                   //GameComment = "Comments - " + gameComment.Comment.ToString()
                                   GameComment = "Comments - " + gameComment.Comment
                               };

            tblkComments.DataContext = gameComments;

            var replayComments = from replayComment in ReplayInformation
                                 where replayComment.GameID == PlayInformation[index].GameID
                                    && replayComment.Inning == PlayInformation[index].Inning
                                    && replayComment.GameTeamCode == PlayInformation[index].GameTeamCode
                                    && replayComment.Sequence == PlayInformation[index].Sequence
                                 orderby replayComment.CommentSequence
                                 select new
                                 {
                                     ReplayComment = "Instant Replay Review - " + replayComment.InitiatorTeamName + " " + replayComment.InitiatorDesc + " initiates replay on " + replayComment.TypeDesc + " " + replayComment.ReasonDesc,
                                     ReplayRuling =  "Replay Ruling - Umpire " + replayComment.UmpireName + " - " + replayComment.ReversedDesc
                                  };

            tblkReplayComment.DataContext = replayComments;
            tblkReplayRuling.DataContext = replayComments;
        }
    }
}
