using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_RetrieveData;

namespace Retrosheet_Console
{
    class BuildTreeView
    {
        static void xMain(string[] args)
        {
            RetrieveData retrieveData = new RetrieveData();


           Collection<TreeViewModels.Season> Seasons = retrieveData.RetrieveTreeViewData_Seasons();

        }

    }
}
