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

        public Retrosheet_PlayBall_PlayByPlay(string gameID)
        {
            InitializeComponent();
            ShowsNavigationUI = true;

            string SeasonYear;
            string SeasonGameType;
            string HomeTeamID;
            String VisitingTeamID;

            RetrieveData retrieveData = new RetrieveData();

            retrieveData.RetrieveReferenceData();

            Collection<DataModels.GameInformationItem> GameInformationItems = retrieveData.RetrieveGameInformation(gameID);

            PageTitle.Text = retrieveData.PageTitle;

            SeasonYear = retrieveData.SeasonYear;
            SeasonGameType = retrieveData.SeasonGameType;
            HomeTeamID = retrieveData.HomeTeamID;
            VisitingTeamID = retrieveData.VisitingTeamID;

            dgGameInfoItems.ItemsSource = GameInformationItems;


            Collection<DataModels.PlayerInformation> PlayerInformation = retrieveData.RetrievePlayers(SeasonYear,
                                                                                                      SeasonGameType,
                                                                                                      HomeTeamID,
                                                                                                      VisitingTeamID);

            Collection<DataModels.StartingPlayerInformation> StartingPlayerInformation = retrieveData.RetrieveStartingPlayers(SeasonYear,
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

            // set the dgPlayBevents data source after retrieving all data.
            dgPlayBevents.ItemsSource = PlayBeventInformation;
            imgEventHitLocationDiagram.DataContext = PlayBeventInformation[0];
        }


        private void dbPlayEvents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var index = dg.SelectedIndex;

            var gameComments = from gameComment in GameCommentInformation
                               where gameComment.GameID == PlayBeventInformation[index].GameID
                                  && gameComment.Inning == PlayBeventInformation[index].Inning
                                  && gameComment.GameTeamCode == PlayBeventInformation[index].GameTeamCode
                                  && gameComment.Sequence == PlayBeventInformation[index].EventNum
                               orderby gameComment.CommentSequence
                               select new
                               {
                                   //GameComment = "Comments - " + gameComment.Comment.ToString()
                                   GameComment = "Comments - " 
                                                 + gameComment.Comment 
                               };

            tblkComments.DataContext = gameComments;

            var batterAdjustments = from batterAdjustment in BatterAdjustmentInformation
                                    where batterAdjustment.GameID == PlayBeventInformation[index].GameID
                                       && batterAdjustment.Sequence == PlayBeventInformation[index].EventNum
                                    orderby batterAdjustment.Sequence
                                    select new
                                    {
                                        //BatterAdjustment = batterAdjustment.PlayerLastName
                                        //                   + ", " + batterAdjustment.PlayerFirstName
                                        //                   + " switches to batting "
                                        //                   + batterAdjustment.BatsDesc + " handed " 

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

            tblkBatterAdjustments.DataContext = batterAdjustments;

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

            tblkPitcherAdjustments.DataContext = pitcherAdjustments;

            var substitutePlayers = from substitutePlayer in SubstitutePlayerInformation
                                    where substitutePlayer.GameID == PlayBeventInformation[index].GameID
                                       && substitutePlayer.Sequence == PlayBeventInformation[index].EventNum
                                    orderby substitutePlayer.Sequence
                                    select new
                                    {
                                        SubstitutePlayer = substitutePlayer.FieldPositionDesc + " - "
                                                           + substitutePlayer.PlayerLastName + ", " + substitutePlayer.PlayerFirstName
                                                           + " comes into the game for " + substitutePlayer.TeamName
                                                           + " batting " + substitutePlayer.BattingOrder + " in the line up" + Environment.NewLine
                                                           + "Bats " + substitutePlayer.PlayerBatsDesc
                                                           + " / Throws " + substitutePlayer.PlayerThrowsDesc
                                    };

            tblkSubstitutePlayers.DataContext = substitutePlayers;

            var substituteUmpires = from substituteUmpire in SubstituteUmpireInformation
                                    where substituteUmpire.GameID == PlayBeventInformation[index].GameID
                                       && substituteUmpire.Sequence == PlayBeventInformation[index].EventNum
                                    orderby substituteUmpire.Sequence
                                    select new
                                    {
                                        SubstituteUmpire = "Umpire " + substituteUmpire.UmpireLastName + ", " + substituteUmpire.UmpireFirstName
                                                           + " comes into the game at " + substituteUmpire.FieldPositionDesc
                                    };

            tblkSubstituteUmpires.DataContext = substituteUmpires;

            var replayComments = from replayComment in ReplayInformation
                                 where replayComment.GameID == PlayBeventInformation[index].GameID
                                    && replayComment.Inning == PlayBeventInformation[index].Inning
                                    && replayComment.GameTeamCode == PlayBeventInformation[index].GameTeamCode
                                    && replayComment.Sequence == PlayBeventInformation[index].EventNum
                                 orderby replayComment.CommentSequence
                                 select new
                                 {
                                     ReplayComment = "Instant Replay Review - " + replayComment.InitiatorTeamName + " " + replayComment.InitiatorDesc + " initiates replay on " + replayComment.TypeDesc + " " + replayComment.ReasonDesc,
                                     ReplayRuling =  "Replay Ruling - Umpire " + replayComment.UmpireName + " - " + replayComment.ReversedDesc
                                  };

            tblkReplayComment.DataContext = replayComments;
            tblkReplayRuling.DataContext = replayComments;
            imgEventHitLocationDiagram.DataContext = PlayBeventInformation[index];
        }
    }
}
