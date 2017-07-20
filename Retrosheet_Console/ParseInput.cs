using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retrosheet_DataFileIO;
using System.IO;
using Retrosheet_Settings;

namespace Retrosheet_Console
{
    class ParseInput
    {
        static void Main(string[] args)
        {

            // settings are captured in Settings constructor
           
            Settings settings = new Settings();
           
            DataFileIO  dataFileIO = new DataFileIO();

            settings.BackupSettings();

            string[] settingsArray = new string[4];

            settingsArray[0] = @"reference_data|TRUE|C:\users\mmr\documents\retrosheet\ReferenceData\|reference_data.txt|||";
            settingsArray[1] = @"ballpark_data|TRUE|C:\users\mmr\documents\retrosheet\ReferenceData\|Ballpark.txt|C:\users\mmr\documents\retrosheet\ReferenceData\Output\|Ballpark.txt|";
            settingsArray[2] = @"personal_data|TRUE|C:\users\mmr\documents\retrosheet\ReferenceData\|personal.txt|||";
            settingsArray[3] = @"event_data|TRUE|C:\users\mmr\documents\retrosheet\2016 Regular Season\||C:\users\mmr\documents\retrosheet\2016 Regular Season\Output\||2016 Regular Season";

            settings.WriteSettings(settingsArray);

            
            //dataFileIO.ProcessBallparkFile(@"C:\users\mmr\documents\retrosheet\ReferenceData",
            //                            "Ballpark.txt",
            //                            @"C: \users\mmr\documents\retrosheet\ReferenceData\Output",
            //                            "BallPark.txt");
            

            
            dataFileIO.ProcessBallparkFile(settings.BallparkDataInputPath, settings.BallparkDataInputFile,
                            settings.BallparkDataOutputPath,
                            settings.BallparkDataOutputFile);


            
            //dataFileIO.ProcessEventFiles(@"C:\users\mmr\documents\retrosheet\2016 Regular Season",
            //                          @"C:\users\mmr\documents\retrosheet\2016 Regular Season\Output");
            //
            

            dataFileIO.ProcessEventFiles(settings.EventDataInputPath,
                                         settings.EventDataOutputPath);

           

            LoadDatabase loadDatabase = new LoadDatabase();

            loadDatabase.TruncateDatabase();
            loadDatabase.LoadDatabaseEventData(settings.EventDataOutputPath);
            loadDatabase.LoadDatabaseReferenceData(settings.ReferenceDataInputPath  + settings.ReferenceDataInputFile);
            loadDatabase.LoadDatabasePersonalData(settings.PersonalDataInputPath + settings.PersonalDataInputFile);
            loadDatabase.LoadDatabaseBallparkData(settings.BallparkDataInputPath  + settings.BallparkDataInputFile);
            loadDatabase.LoadDatabaseGameInformation();


            Console.ReadLine();
            
        }
    }
}
