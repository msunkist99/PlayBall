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
using System.Diagnostics;

namespace Retrosheet_PlayBall
{
    /// <summary>
    /// Interaction logic for Retrosheet_PlayBallHome.xaml
    /// </summary>
    public partial class Retrosheet_PlayBall_Home : Page
    {
        public Retrosheet_PlayBall_Home()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
        }

        private void btnButton_Click(object sender, RoutedEventArgs e)
        {
            btnButton.IsEnabled = false;
            // view the Play Ball - game selection UI
            Retrosheet_PlayBall_Seasons selectGamePage = new Retrosheet_PlayBall_Seasons();
            this.NavigationService.Navigate(selectGamePage);
        }

        private void btnButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            btnButton.Content = "Please wait...";
        }

        private void Hyperlink_RequestNavigate(object sender,
                                               System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.retrosheet.org");
            System.Diagnostics.Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
