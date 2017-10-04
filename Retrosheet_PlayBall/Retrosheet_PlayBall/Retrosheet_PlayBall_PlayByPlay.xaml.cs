﻿using System;
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
        private Collection<DataModels.PlayBeventInformation> PlayBeventInformation;
        private Collection<DataModels.ReplayInformation> ReplayInformation;
        private Collection<DataModels.GameCommentInformation> GameCommentInformation;
        private Collection<DataModels.BatterAdjustmentInformation> BatterAdjustmentInformation;
        private Collection<DataModels.PlayerEjectionInformation> PlayerEjectionInformation;
        private Collection<DataModels.GameDataInformation> GameDataInformation;
        private Collection<DataModels.PitcherAdjustmentInformation> PitcherAdjustmentInformation;
        private Collection<DataModels.SubstitutePlayerInformation> SubstitutePlayerInformation;
        private Collection<DataModels.SubstituteUmpireInformation> SubstituteUmpireInformation;
        private Collection<DataModels.StartingPlayerInformation> StartingPlayerInformation;

        private string HomeTeamName;
        private string VisitingTeamName;

        public Retrosheet_PlayBall_PlayByPlay(string gameID)
        {
            InitializeComponent();
            ShowsNavigationUI = true;

            string SeasonYear;
            string SeasonGameType;
            string HomeTeamID;
            string VisitingTeamID;

            RetrieveData retrieveData = new RetrieveData();

            retrieveData.RetrieveReferenceData();

            //Collection<DataModels.GameInformationItem> GameInformationItems = retrieveData.RetrieveGameInformation(gameID);

            PageTitle.Text = retrieveData.PageTitle;

            //dgGameInfoItems.ItemsSource = GameInformationItems;
            dgGameInfoItems.ItemsSource = retrieveData.RetrieveGameInformation(gameID);


            SeasonYear = retrieveData.SeasonYear;
            SeasonGameType = retrieveData.SeasonGameType;
            HomeTeamID = retrieveData.HomeTeamID;
            VisitingTeamID = retrieveData.VisitingTeamID;
            HomeTeamName = retrieveData.HomeTeamName;
            VisitingTeamName = retrieveData.VisitingTeamName;



            //Collection<DataModels.PlayerInformation> PlayerInformation = retrieveData.RetrievePlayers(SeasonYear,
            //                                                                                          SeasonGameType,
            //                                                                                          HomeTeamID,
            //                                                                                          VisitingTeamID);

            StartingPlayerInformation = retrieveData.RetrieveStartingPlayers(SeasonYear,
                                                                             SeasonGameType,
                                                                             gameID);

            SubstitutePlayerInformation = retrieveData.RetrieveSubstitutePlayer(SeasonYear,
                                          SeasonGameType,
                                          gameID);
            /*                                                                                                                     
            PlayInformation = retrieveData.RetrievePlay(gameInformation.SeasonYear,
                                                        gameInformation.SeasonGameType,
                                                        gameInformation.HomeTeamID,
                                                        gameInformation.VisitingTeamID,
                                                        gameInformation.GameID);
            */

            PlayBeventInformation = retrieveData.RetrievePlayBevent(SeasonYear,
                                                                    SeasonGameType,
                                                                    HomeTeamID,
                                                                    VisitingTeamID,
                                                                    gameID);

            ReplayInformation = retrieveData.RetrieveReplay(SeasonYear,
                                                            SeasonGameType,
                                                            gameID);

            BatterAdjustmentInformation = retrieveData.RetrieveBatterAdjustment(SeasonYear,
                                                                                SeasonGameType,
                                                                                HomeTeamID,
                                                                                VisitingTeamID,
                                                                                gameID);

            PlayerEjectionInformation = retrieveData.RetrievePlayerEjection(gameID);

            GameCommentInformation = retrieveData.RetrieveGameComment(gameID);

            GameDataInformation = retrieveData.RetrieveGameData(gameID);

            PitcherAdjustmentInformation = retrieveData.RetrievePitcherAdjustmentInformation(SeasonYear,
                                                                                             SeasonGameType,
                                                                                             gameID);

            SubstituteUmpireInformation = retrieveData.RetrieveSubstituteUmpireInformation(gameID);


            dgVisitingPlayers.ItemsSource = from startingPlayer in StartingPlayerInformation
                                            where startingPlayer.GameTeamCode == 0
                                            orderby startingPlayer.GameTeamCode, startingPlayer.BattingOrder
                                            select new
                                            {
                                                Batting = startingPlayer.BattingOrderDesc,
                                                Name = string.Format("{0}, {1}", startingPlayer.PlayerLastName, startingPlayer.PlayerFirstName),
                                                Position = startingPlayer.FieldPositionDesc,
                                                BatsThrows = string.Format("{0} / {1}", startingPlayer.BatsDesc, startingPlayer.ThrowsDesc),
                                            };

            dgHomePlayers.ItemsSource =     from startingPlayer in StartingPlayerInformation
                                            where startingPlayer.GameTeamCode == 1
                                            orderby startingPlayer.GameTeamCode, startingPlayer.BattingOrder
                                            select new
                                            {
                                                //Batting = startingPlayer.BattingOrder,
                                                Name = string.Format("{0}, {1}", startingPlayer.PlayerLastName, startingPlayer.PlayerFirstName),
                                                Position = startingPlayer.FieldPositionDesc,
                                                BatsThrows = string.Format("{0} / {1}", startingPlayer.BatsDesc, startingPlayer.ThrowsDesc),
                                            };

            // set the dgPlayBevents data source after retrieving all data cause it will trigger the dbPlayEvents_SelectedCellsChanged event
            dgPlayBevents.ItemsSource = PlayBeventInformation;
            //imgRunnersOnBaseDiagram.DataContext = PlayBeventInformation[0];
        }


        private void dbPlayEvents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var index = dg.SelectedIndex;
            tblkPlayerAdjustments.Text = "";
            tblkGameComments.Text = "";


            var countsVisiting = from countVisiting in PlayBeventInformation
                                 where countVisiting.GameID == PlayBeventInformation[index].GameID
                                    && countVisiting.EventNum == PlayBeventInformation[index].EventNum
                                 select new
                                 {
                                        CountTeam = VisitingTeamName,
                                        CountInning = countVisiting.Inning + " ⌃",
                                        CountRuns = countVisiting.VisitingTeamScore,
                                        CountHits = countVisiting.VistingTeamHitCount,
                                        CountErrors = countVisiting.VisitingTeamErrorCount
                                 };

            dgVisitingRunsHitsErrors.ItemsSource = countsVisiting;

            var countsHome = from countHome in PlayBeventInformation
                                 where countHome.GameID == PlayBeventInformation[index].GameID
                                    && countHome.EventNum == PlayBeventInformation[index].EventNum
                             select new
                             {
                                 CountTeam = HomeTeamName,
                                 CountInning ="   ⌄",
                                 CountRuns = countHome.HomeTeamScore,
                                 CountHits = countHome.HomeTeamHitCount,
                                 CountErrors = countHome.HomeTeamErrorCount
                             };


            dgHomeRunsHitsErrors.ItemsSource = countsHome;

            var batterAdjustments = from batterAdjustment in BatterAdjustmentInformation
                                    where batterAdjustment.GameID == PlayBeventInformation[index].GameID
                                       && batterAdjustment.Sequence == PlayBeventInformation[index].EventNum
                                    orderby batterAdjustment.Sequence
                                    select new
                                    {
                                        BatterAdjustment = batterAdjustment.PlayerID != PlayBeventInformation[index].BatterPlayerID?

                                                           "coming up to bat is " + batterAdjustment.PlayerLastName
                                                           + ", " + batterAdjustment.PlayerFirstName
                                                           + " - switches to batting "
                                                           + batterAdjustment.BatsDesc + " handed " :

                                                           batterAdjustment.PlayerLastName
                                                           + ", " + batterAdjustment.PlayerFirstName
                                                           + " switches to batting "
                                                           + batterAdjustment.BatsDesc + " handed "
                                    };

            foreach (var batterAdjustment in batterAdjustments)
            {
                tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + batterAdjustment.BatterAdjustment + Environment.NewLine;
            }


            var pitcherAdjustments = from pitcherAdjustment in PitcherAdjustmentInformation
                                     where pitcherAdjustment.GameID == PlayBeventInformation[index].GameID
                                        && pitcherAdjustment.Sequence == PlayBeventInformation[index].EventNum
                                     orderby pitcherAdjustment.Sequence
                                     select new
                                     {
                                         PitcherAdjustment = pitcherAdjustment.PlayerLastName
                                                             + ", " + pitcherAdjustment.PlayerFirstName
                                                             + " switches to pitching "
                                                             + pitcherAdjustment.PlayerHandDesc + " handed "
                                     };

            foreach (var pitcherAdjustment in pitcherAdjustments)
            {
                tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + pitcherAdjustment.PitcherAdjustment + Environment.NewLine;
            }


            var substitutePlayers = from substitutePlayer in SubstitutePlayerInformation
                                    where substitutePlayer.GameID == PlayBeventInformation[index].GameID
                                       && substitutePlayer.Sequence == PlayBeventInformation[index].EventNum
                                    orderby substitutePlayer.Sequence
                                    select new
                                    {
                                        SubstitutePlayer = substitutePlayer.FieldPosition != 11 && substitutePlayer.FieldPosition != 12 ?    // pinch hitter and pinch runner
                                                           substitutePlayer.FieldPositionDesc + " - "
                                                           + substitutePlayer.PlayerLastName + ", " + substitutePlayer.PlayerFirstName
                                                           + " comes into the game for " + substitutePlayer.TeamName
                                                           + " throws " + substitutePlayer.PlayerThrowsDesc :

                                                           ""
                                    };

            foreach (var substitutePlayer in substitutePlayers)
            {
                tblkPlayerAdjustments.Text = substitutePlayer.SubstitutePlayer + Environment.NewLine;
            }

            if (PlayBeventInformation[index].BatterRemovedForPinchPlayerID != null)
            {
                var battersRemoved = from batterRemoved in PlayBeventInformation
                                     where batterRemoved.GameID == PlayBeventInformation[index].GameID
                                        && batterRemoved.EventNum == PlayBeventInformation[index].EventNum
                                     select new
                                     {
                                         BatterRemoved = PlayBeventInformation[index].TeamName + " batter " + PlayBeventInformation[index].BatterRemovedForPinchLastName
                                              + ", " + PlayBeventInformation[index].BatterRemovedForPinchFirstName
                                              + " (" + PlayBeventInformation[index].BatterRemovedForPinchFieldPositionDesc
                                              + ") replaced by pinch hitter "
                                              + PlayBeventInformation[index].BatterLastName
                                              + ", " + PlayBeventInformation[index].BatterFirstName
                                     };

                foreach (var batterRemoved in battersRemoved)
                {
                    tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + batterRemoved.BatterRemoved + Environment.NewLine;
                }
            }

            if (PlayBeventInformation[index].RunnerFirstRemovedForPinchPlayerID!= null)
            {
                var runnersRemoved = from runnerRemoved in PlayBeventInformation
                                     where runnerRemoved.GameID == PlayBeventInformation[index].GameID
                                        && runnerRemoved.EventNum == PlayBeventInformation[index].EventNum
                                     select new
                                     {
                                         RunnerRemoved = PlayBeventInformation[index].TeamName + " runner on first base " + PlayBeventInformation[index].RunnerFirstRemovedForPinchLastName
                                              + ", " + PlayBeventInformation[index].RunnerFirstRemovedForPinchFirstName
                                              + " replaced by pinch runner"
                                     };

                foreach (var runnerRemoved in runnersRemoved)
                {
                    tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + runnerRemoved.RunnerRemoved + Environment.NewLine;
                }
            }

            if (PlayBeventInformation[index].RunnerSecondRemovedForPinchPlayerID != null)
            {
                var runnersRemoved = from runnerRemoved in PlayBeventInformation
                                     where runnerRemoved.GameID == PlayBeventInformation[index].GameID
                                        && runnerRemoved.EventNum == PlayBeventInformation[index].EventNum
                                     select new
                                     {
                                         RunnerRemoved = PlayBeventInformation[index].TeamName + " runner on second base " + PlayBeventInformation[index].RunnerSecondRemovedForPinchLastName
                                              + ", " + PlayBeventInformation[index].RunnerSecondRemovedForPinchFirstName
                                              + " replaced by pinch runner"
                                     };

                foreach (var runnerRemoved in runnersRemoved)
                {
                    tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + runnerRemoved.RunnerRemoved + Environment.NewLine;
                }
            }

            if (PlayBeventInformation[index].RunnerThirdRemovedForPinchPlayerID != null)
            {
                var runnersRemoved = from runnerRemoved in PlayBeventInformation
                                     where runnerRemoved.GameID == PlayBeventInformation[index].GameID
                                        && runnerRemoved.EventNum == PlayBeventInformation[index].EventNum
                                     select new
                                     {
                                         RunnerRemoved = PlayBeventInformation[index].TeamName + " runner on Third base " + PlayBeventInformation[index].RunnerThirdRemovedForPinchLastName
                                              + ", " + PlayBeventInformation[index].RunnerThirdRemovedForPinchFirstName
                                              + " replaced by pinch runner"
                                     };

                foreach (var runnerRemoved in runnersRemoved)
                {
                    tblkPlayerAdjustments.Text = tblkPlayerAdjustments.Text + runnerRemoved.RunnerRemoved + Environment.NewLine;
                }
            }

            var gameComments = from gameComment in GameCommentInformation
                               where gameComment.GameID == PlayBeventInformation[index].GameID
                                  && gameComment.Inning == PlayBeventInformation[index].Inning
                                  && gameComment.GameTeamCode == PlayBeventInformation[index].GameTeamCode
                                  && gameComment.Sequence == PlayBeventInformation[index].EventNum
                               orderby gameComment.CommentSequence
                               select new
                               {
                                   GameComment = "Comments - "
                                                 + gameComment.Comment
                               };

            foreach (var gameComment in gameComments)
            {
                tblkGameComments.Text = tblkGameComments.Text + gameComment.GameComment + Environment.NewLine;
            }

            var substituteUmpires = from substituteUmpire in SubstituteUmpireInformation
                                    where substituteUmpire.GameID == PlayBeventInformation[index].GameID
                                       && substituteUmpire.Sequence == PlayBeventInformation[index].EventNum
                                    orderby substituteUmpire.Sequence
                                    select new
                                    {
                                            SubstituteUmpire = substituteUmpire.UmpireLastName != null ?
                                                    "Umpire " + substituteUmpire.UmpireLastName + ", " + substituteUmpire.UmpireFirstName
                                                           + " comes into the game at " + substituteUmpire.FieldPositionDesc :
                                                    "Umpire (unknown) comes into the game at " + substituteUmpire.FieldPositionDesc,
                                        };

            foreach (var substituteUmpire in substituteUmpires)
            {
                tblkGameComments.Text = tblkGameComments.Text + substituteUmpire.SubstituteUmpire + Environment.NewLine;
            }

            var replayComments = from replayComment in ReplayInformation
                                 where replayComment.GameID == PlayBeventInformation[index].GameID
                                    && replayComment.Inning == PlayBeventInformation[index].Inning
                                    && replayComment.GameTeamCode == PlayBeventInformation[index].GameTeamCode
                                    && replayComment.Sequence == PlayBeventInformation[index].EventNum
                                 orderby replayComment.CommentSequence
                                 select new
                                     {
                                     ReplayComment = "Instant Replay Review - " + replayComment.InitiatorTeamName + " " + replayComment.InitiatorDesc + " initiates replay on " + replayComment.TypeDesc + " " + replayComment.ReasonDesc,
                                     ReplayRuling = "Replay Ruling - Umpire " + replayComment.UmpireName + " - " + replayComment.ReversedDesc
                                     };

            foreach (var replayComment in replayComments)
            {
                tblkGameComments.Text = tblkGameComments.Text + replayComment.ReplayComment + Environment.NewLine;
                tblkGameComments.Text = tblkGameComments.Text + replayComment.ReplayRuling + Environment.NewLine;
            }

            imgRunnersOnBaseDiagram.DataContext = PlayBeventInformation[index];
            lblRunnerDiagram.Content = "At Bat - " + PlayBeventInformation[index].TeamName;
        }
    }
}
