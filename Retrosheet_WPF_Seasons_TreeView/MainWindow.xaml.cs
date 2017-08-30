using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Retrosheet_RetrieveData;

namespace Retrosheet_WPF_Seasons_TreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RetrieveData retrieveData = new RetrieveData();

            Collection<TreeViewModels.Season> Seasons = retrieveData.RetrieveTreeViewData_Seasons();

            trvSeasons.ItemsSource = Seasons;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if(trvSeasons.SelectedItem != null)
            {
                var list = trvSeasons.ItemsSource as Collection<TreeViewModels.Season>;
                int curIndex = list.IndexOf(trvSeasons.SelectedItem as TreeViewModels.Season);

            }
        }
    }

    public class TreeViewItemBase : INotifyPropertyChanged
    {
        private bool isSelected;

        public bool IsSelected
        {
            get { return this.isSelected; }
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
