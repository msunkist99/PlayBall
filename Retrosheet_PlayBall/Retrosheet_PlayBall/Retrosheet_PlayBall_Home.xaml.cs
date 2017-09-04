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
            // view the Play Ball - Play-By-Play UI
            btnButton.IsEnabled = false;
            btnButton.Content = "Please wait...";
            MessageBox.Show("MLB Game Data will now load.  Click OK to continue....", Title);

            Retrosheet_PlayBall_Seasons selectGamePage = new Retrosheet_PlayBall_Seasons();
            this.NavigationService.Navigate(selectGamePage);
        }
    }
}
