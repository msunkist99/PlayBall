﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_RetrieveData;

namespace Retrosheet_Console
{
    class DisplayRetrievedData
    {

        static void Main(string[] args)
        {
            RetrieveData retrieveData = new RetrieveData();

            //retrieveData.RetrieveAvailableSeasons();
            //retrieveData.RetrieveAvailableSeasons2();
            retrieveData.RetrieveReferenceData();
            retrieveData.RetrieveGame("MIN201609222");
            retrieveData.RetrieveStartingPlayers("MIN201609222");
            retrieveData.RetrievePlayByPlay("MIN201609222");
            Console.WriteLine("All done!");
            Console.ReadLine();
        }
    }
}
