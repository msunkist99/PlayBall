using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Retrosheet_RetrieveData;

namespace Retrosheet_PlayBall
{
    /// <summary>
    /// Interaction logic for Retrosheet_PlayBall_Seasons.xaml
    /// </summary>
    public partial class Retrosheet_PlayBall_Seasons : Page
    {
        public Retrosheet_PlayBall_Seasons()
        {
            InitializeComponent();

            ShowsNavigationUI = false;

            RetrieveData retrieveData = new RetrieveData();

            //Collection<TreeViewModels.Season> Seasons = retrieveData.RetrieveTreeViewData_Seasons();

            Collection<TreeViewModels.Season> Seasons = retrieveData.RetrieveTreeViewData_GameSelectionList();

            trvSeasons.ItemsSource = Seasons;
        }



        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (trvSeasons.SelectedItem != null)
            {
                if (trvSeasons.SelectedItem.GetType().Name == "Game")
                {
                    /// using cast on trvSeasons.SelectedItem
                    Retrosheet_RetrieveData.TreeViewModels.Game item = (Retrosheet_RetrieveData.TreeViewModels.Game)trvSeasons.SelectedItem;

                    //MessageBox.Show("Item selected:  " + item.GameDesc + " " + item.GameID, Title);

                    Retrosheet_PlayBall_PlayByPlay PlayPage = new Retrosheet_PlayBall_PlayByPlay(item.GameID);

                    this.NavigationService.Navigate(PlayPage);
                    btnSelect.Content = "Select Game";
                }
                else
                {
                    MessageBox.Show("No game selected", Title);
                }
            }
            else
            {
                MessageBox.Show("No item selected", Title);
            }
        }

        private void trvSeasons_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (trvSeasons.SelectedItem != null)
            {
                if (trvSeasons.SelectedItem.GetType().Name == "Game")
                {
                    btnSelect.IsEnabled = true;
                }
                else
                {
                    btnSelect.IsEnabled = false;
                }
            }
            else
            {
                btnSelect.IsEnabled = false;
            }
        }

        private void trvSeasons_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (trvSeasons.SelectedItem != null)
            {
                if (trvSeasons.SelectedItem.GetType().Name == "Game")
                {
                    btnSelect.Content = "Please wait...";

                    Retrosheet_RetrieveData.TreeViewModels.Game item = (Retrosheet_RetrieveData.TreeViewModels.Game)trvSeasons.SelectedItem;
                    //lblGameSelection.Content = "Item selected:  " + item.GameDesc + " " + item.GameID;
                    MessageBox.Show("Game selected:  " + item.GameDesc + " " + "\r\nClick OK to load game", Title);
                    Retrosheet_PlayBall_PlayByPlay PlayPage = new Retrosheet_PlayBall_PlayByPlay(item.GameID);

                    btnSelect.Content = "Select Game";

                    this.NavigationService.Navigate(PlayPage);
                }
            }
        }

        private void btnSelect_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (trvSeasons.SelectedItem != null)
            {
                if (trvSeasons.SelectedItem.GetType().Name == "Game")
                {
                    btnSelect.Content = "Please wait...";
                    //Retrosheet_RetrieveData.TreeViewModels.Game item = (Retrosheet_RetrieveData.TreeViewModels.Game)trvSeasons.SelectedItem;
                    //lblGameSelection.Content = "Item selected:  " + item.GameDesc + " " + item.GameID;
                }
            }
        }

        private void trvSeasons_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (trvSeasons.SelectedItem != null)
            {
                if (trvSeasons.SelectedItem.GetType().Name == "Game")
                {
                    btnSelect.Content = "Please wait...";
                }
            }
        }
    }
}