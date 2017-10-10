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
        private RetrieveData RetrieveData;
        private Collection<DataModels.PlayBeventInformation> PlayBeventInformation;

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

            RetrieveData = new RetrieveData();

            dgGameInfoItems.ItemsSource = RetrieveData.RetrieveGameInformation(gameID);
            PageTitle.Text = RetrieveData.PageTitle;

            SeasonYear = RetrieveData.SeasonYear;
            SeasonGameType = RetrieveData.SeasonGameType;
            HomeTeamID = RetrieveData.HomeTeamID;
            VisitingTeamID = RetrieveData.VisitingTeamID;
            HomeTeamName = RetrieveData.HomeTeamName;
            VisitingTeamName = RetrieveData.VisitingTeamName;

            RetrieveData.RetrieveStartingPlayers(SeasonYear,
                                                 SeasonGameType,
                                                 gameID);

            RetrieveData.RetrieveSubstitutePlayers(SeasonYear,
                                                   SeasonGameType,
                                                   gameID);

            PlayBeventInformation = RetrieveData.RetrievePlayBevent(SeasonYear,
                                                                    SeasonGameType,
                                                                    HomeTeamID,
                                                                    VisitingTeamID,
                                                                    gameID);

            RetrieveData.RetrievePlayerLineup();

            RetrieveData.RetrieveReplay(SeasonYear,
                                        SeasonGameType,
                                        gameID);

            RetrieveData.RetrieveBatterAdjustment(SeasonYear,
                                                  SeasonGameType,
                                                  HomeTeamID,
                                                  VisitingTeamID,
                                                  gameID);

            RetrieveData.RetrievePlayerEjection(gameID);

            RetrieveData.RetrieveGameComment(gameID);

            RetrieveData.RetrieveGameData(gameID);

            RetrieveData.RetrievePitcherAdjustmentInformation(SeasonYear,
                                                              SeasonGameType,
                                                              gameID);

            RetrieveData.RetrieveSubstituteUmpireInformation(gameID);

            // set the dgPlayBevents data source after retrieving all data cause it will trigger the dbPlayEvents_SelectedCellsChanged event
            dgPlayBevents.ItemsSource = PlayBeventInformation;
        }


        private void dbPlayEvents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var index = dg.SelectedIndex;
            tblkPlayerAdjustments.Text = "";
            tblkGameComments.Text = "";

            
            dgVisitingPlayers.ItemsSource = RetrieveData.RetrieveBeventPlayerLineUp(PlayBeventInformation[index].EventNum, 0);
            dgHomePlayers.ItemsSource = RetrieveData.RetrieveBeventPlayerLineUp(PlayBeventInformation[index].EventNum, 1);

            dgVisitingRunsHitsErrors.ItemsSource = RetrieveData.RetrieveBeventCounts(PlayBeventInformation[index].EventNum, 0);

            dgHomeRunsHitsErrors.ItemsSource = RetrieveData.RetrieveBeventCounts(PlayBeventInformation[index].EventNum, 1);
            tblkPlayerAdjustments.Text = RetrieveData.RetrieveBeventPlayerAdjustments(index, PlayBeventInformation[index].EventNum);

            tblkGameComments.Text = RetrieveData.RetrieveBeventGameComments(PlayBeventInformation[index].EventNum);

            imgRunnersOnBaseDiagram.DataContext = PlayBeventInformation[index];
            lblRunnerDiagram.Content = "At Bat - " + PlayBeventInformation[index].TeamName;
        }
    }
}
