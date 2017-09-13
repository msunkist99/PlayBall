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
        public Retrosheet_PlayBall_PlayByPlay(string gameID)
        {
            InitializeComponent();

            RetrieveData retrieveData = new RetrieveData();

            retrieveData.RetrieveReferenceData();

            Collection<DataModels.GameInformation> GameInformation = retrieveData.RetrieveGameInformation(gameID);

            DataModels.GameInformation gameInformation = GameInformation.First<DataModels.GameInformation>();

            Collection<DataModels.PlayerInformation> PlayerInformation = retrieveData.RetrievePlayers( gameInformation.SeasonYear, 
                                                                                                       gameInformation.SeasonGameType, 
                                                                                                       gameInformation.HomeTeamID,
                                                                                                       gameInformation.VisitingTeamID);

            Collection<DataModels.StartingPlayerInformation> StartingPlayerInformation = retrieveData.RetrieveStartingPlayers(gameInformation.SeasonYear,
                                                                                                                              gameInformation.SeasonGameType,
                                                                                                                              gameInformation.GameID);

            Collection<DataModels.SubstitutePlayerInformation> SubstitutePlayerInformation = retrieveData.RetrieveSubstitutePlayer(gameInformation.SeasonYear,
                                                                                                                                  gameInformation.SeasonGameType,
                                                                                                                                  gameInformation.GameID);
            Collection<DataModels.PlayInformation> PlayInformation = retrieveData.RetrievePlay(gameInformation.SeasonYear,
                                                                                               gameInformation.SeasonGameType,
                                                                                               gameInformation.HomeTeamID,
                                                                                               gameInformation.VisitingTeamID,
                                                                                               gameInformation.GameID);

            Collection<DataModels.BatterAdjustmentInformation> BatterAdjustmentInformation = retrieveData.RetrieveBatterAdjustment(gameInformation.SeasonYear,
                                                                                                                                   gameInformation.SeasonGameType,
                                                                                                                                   gameInformation.HomeTeamID,
                                                                                                                                   gameInformation.VisitingTeamID,
                                                                                                                                   gameInformation.GameID);

            Collection<DataModels.PlayerEjectionInformation> PlayerEjectionInformation = retrieveData.RetrievePlayerEjection(gameInformation.GameID);

            Collection<DataModels.GameCommentInformation> GameCommentInformation = retrieveData.RetrieveGameComment(gameInformation.GameID);

            Collection<DataModels.GameDataInformation> GameDataInformation = retrieveData.RetrieveGameData(gameInformation.GameID);

            Collection<DataModels.PitcherAdjustmentInformation> PitcherAdjustmentInformation = retrieveData.RetrievePitcherAdjustmentInformation(gameInformation.SeasonYear,
                                                                                                                                                 gameInformation.SeasonGameType,
                                                                                                                                                 gameInformation.GameID);
        }

    }
}
