using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////  need this for StreamReader
using System.IO;
using System.Text.RegularExpressions;

namespace Retrosheet_DataFileIO
{
    public class DataFileIO
    {
        private const char inputDelimiter = ',';
        private const char outputDelimiter = '|';

        string hitLocationsString = "78XD|7LDF|7LSF|89XD|9LDF|9LSF|23F|25F|34D|34S|3DF|4MD|4MS|56D|56S|5DF|6MD|6MS|78D|78S|7LD|7LF|7LS|89D|89S|8XD|9LD|9LF|9LS|13|15|1S|23|25|2F|34|3D|3F|3S|4D|4M|4S|56|5D|5F|5S|6D|6M|6S|78|7D|7L|7S|89|8D|8S|9D|9L|9S|1|2|3|4|5|6|7|8|9";
        string[] hitLocations;

        // the following four variables make up the full key
        private string gameID = null;
        private int inning = 0;
        private int gameTeamCode = 0;
        private int eventNum = 0;
        private int commentSequence = 0;
        private int eventNumHold = 0;
        private string homeTeamID = null;
        private string visitingTeamID = null;
        //private int eventNum = 0;

        // constructor
        public DataFileIO()
        {
        }

        private void DeleteDirectory(string outputPath)
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath,true);
            }
        }

        public void ProcessEventFiles(string inputPath,
                                      string outputPath,
                                      string seasonYear,
                                      string seasonGameType)
        {
            string outputFile;


            hitLocations = hitLocationsString.Split(outputDelimiter);

            DeleteDirectory(outputPath);

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(inputPath);
            foreach (string pathFile in fileEntries)
            {
                Console.WriteLine(pathFile);
                string fileName = Path.GetFileName(pathFile);
                string outputFullPath = outputPath + @"\" + fileName.Substring(0, fileName.IndexOf(".")) + @"\";
                // event file
                if (fileName.IndexOf(".EV") > -1)
                {
                    outputFile = fileName.Substring(0, fileName.IndexOf("."));

                    //  the @ tells the compiler to ignore special characters (\) in the string
                    ReadEventFiles(inputPath + @"\" + fileName,
                                           outputFullPath, 
                                           outputFile,
                                           "event",
                                           seasonYear,
                                           seasonGameType);
                }
                // player file
                else if (fileName.IndexOf(".ROS") > -1)
                {
                    outputFile = fileName.Substring(3, 4) + fileName.Substring(0, 3);

                    outputPath = inputPath + @"\Output\" + outputFile + @"\";
                    //  the @ tells the compiler to ignore special characters (\) in the string
                    ReadEventFiles(inputPath + @"\" + fileName,
                                           outputPath,
                                           outputFile,
                                           "player",
                                           seasonYear,
                                           seasonGameType);
                }
                // team file
                else if (fileName.IndexOf("TEAM") > -1)
                {
                    outputFile = fileName.Substring(0, fileName.IndexOf("."));
                    outputPath = inputPath + @"\Output\";
                    ReadEventFiles(inputPath + @"\" + fileName,
                                           outputPath,
                                           outputFile,
                                           "team",
                                           seasonYear,
                                           seasonGameType);
                }
            }
        }


        private void ReadEventFiles(string inputPathFile,
                                    string outputPath,
                                    string outputFile,
                                    string fileType,
                                    string seasonYear,
                                    string seasonGameType)
        {
            string[] columnValue;
            string textLine = null ;

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            using (StreamReader reader = new StreamReader(inputPathFile))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + inputPathFile + " file could not be read:");
                        Console.WriteLine(e.Message);
                    }

                    columnValue = textLine.Split(inputDelimiter);

                    if (fileType == "event")
                    {
                        CreateEventOutput(outputPath,
                                          outputFile,
                                          textLine,
                                          columnValue,
                                          seasonYear,
                                          seasonGameType);
                    }
                    else if (fileType == "player")
                    {
                        CreatePlayerOutput(outputPath,
                                           outputFile,
                                           textLine,
                                           seasonYear,
                                           seasonGameType);
                    }
                    else if (fileType == "team")
                    {
                        CreateTeamOutput(outputPath,
                                         outputFile,
                                         textLine,
                                         seasonYear,
                                         seasonGameType);
                    }
                }
            }
        }

        public void ProcessBallparkFile(string inputPath,
                                        string inputFile,
                                        string outputPath,
                                        string outputFile)
        {
            DeleteDirectory(outputPath);

            ReadWriteBallparkFile(inputPath,
                                  inputFile,
                                  outputPath,
                                  outputFile);
        }

        private void ReadWriteBallparkFile(string inputPath,
                                          string inputFile,
                                          string outputPath,
                                          string outputFile)
        {
            //string[] columnValue;
            string textLine = null;
            string textLine1 = null;
            string textLine2 = null;
            int doubleQuoteIndex = 0;

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            Console.WriteLine(inputPath + @"\" + inputFile);
            using (StreamReader reader = new StreamReader(inputPath + @"\" + inputFile))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        textLine = reader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The " + inputPath + @"\" + inputFile + " file could not be read:");
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    // skip the header record in the input file
                    if (textLine.IndexOf("PARKID") == -1)
                    {
                        doubleQuoteIndex = textLine.IndexOf("\"");

                        // there are notes available on the baseballcodes record which may contain 
                        // commas within the double quotes - leave the comma in the notes.
                        if (doubleQuoteIndex > -1)
                        {
                            textLine1 = textLine.Substring(0, doubleQuoteIndex - 1);
                            textLine2 = textLine.Substring(doubleQuoteIndex + 1);
                            textLine1 = textLine1.Replace(",", "|");
                            textLine2 = textLine2.Replace("\"", "");
                            textLine = textLine1 + "|" + textLine2;
                        }
                        else
                        {
                            textLine = textLine.Replace(",", "|");
                        }
                        //columnValue = textLine.Split('|');
                        WriteEventFile(outputPath, outputFile, textLine, true);
                    }
                }
            }
        }

        private void CreateTeamOutput(string outputPath,
                                      string outputFile,
                                      string textLine,
                                       string seasonYear,
                                       string seasonGameType)
        {
            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

            WriteEventFile(outputPath,
                      outputFile + "_team",
                      seasonYear + outputDelimiter + seasonGameType + outputDelimiter + textLine,
                      true);
        }

        private void CreateEventOutput(string outputPath,
                                       string outputFile,
                                       string textLine,
                                       string[] columnValue,
                                       string season_year,
                                       string season_game_type)
        {
            string commentText = null;

            switch (columnValue[0])
            {
                case "id":
                    // capture the gameID which is the first part of the record key
                    gameID = columnValue[1];
                    inning = 0;
                    gameTeamCode = 0;
                    eventNum = 0;
                    commentSequence = 0;
                    homeTeamID = null;
                    visitingTeamID = null;

                    WriteEventFile(outputPath,
                                   outputFile + "_gameinfo" ,
                                   gameID + outputDelimiter + "info" + outputDelimiter + "season_game_type"
                                   + outputDelimiter + season_game_type,
                                   true);

                    WriteEventFile(outputPath,
                                   outputFile + "_gameinfo" ,
                                   gameID + outputDelimiter + "info" + outputDelimiter + "season_year"
                                   + outputDelimiter + season_year,
                                   true);
                    break;

                case "version":
                    break;

                case "info":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);
                    

                    /*if ((columnValue[1] == "edittime") ||
                        (columnValue[1] == "howscored") ||
                        (columnValue[1] == "inputprogvers") ||
                        (columnValue[1] == "inputter") ||
                        (columnValue[1] == "inputtime") ||
                        (columnValue[1] == "scorer") ||
                        (columnValue[1] == "translator") )
                    {
                        WriteEventFile(outputPath,
                                  outputFile + "_admin" + columnValue[0],
                                  gameID + outputDelimiter + textLine,
                                  true);
                    }
                    else
                    {
                    */

                    if (columnValue[1] == "hometeam")
                    {
                        homeTeamID = columnValue[2];
                    }

                    if (columnValue[1] == "visteam")
                    {
                        visitingTeamID = columnValue[2];
                    }

                    WriteEventFile(outputPath,
                                   outputFile + "_game" + columnValue[0],
                                   gameID + outputDelimiter + textLine,
                                   true);
                    //}
                    break;

                case "start":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    if (columnValue[3] == "0")
                    {
                        WriteEventFile(outputPath,
                                       outputFile + "_" + columnValue[0],
                                       gameID + outputDelimiter + textLine + outputDelimiter + visitingTeamID,
                                       true);

                    }
                    else
                    {
                    WriteEventFile(outputPath,
                                   outputFile + "_" + columnValue[0],
                                   gameID + outputDelimiter + textLine + outputDelimiter + homeTeamID,
                                   true);
                    }

                    break;

                case "play":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);
                    string[] stringArray = textLine.Split(outputDelimiter);

                    //skip the No Play/No Event play records
                    if (stringArray[6].Contains("NP"))
                    {
                        break;
                    }

                    eventNum++;
                    if (inning != Int32.Parse(columnValue[1]))
                    {
                        inning = Int32.Parse(columnValue[1]);
                        commentSequence = 0;
                    }

                    //  0 - top of the inning - visiting team
                    //  1 - bottom of the inning - home team
                    if (gameTeamCode != Int32.Parse(columnValue[2]))
                    {
                        gameTeamCode = Int32.Parse(columnValue[2]);
                        commentSequence = 0;
                    }
                    break;

                case "sub":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    // many time there is a substitute DEFENSIVE player record in the input Event file that is before the 
                    // first play record when the inning changes.
                    // in this case - add 1 to the eventNum value so that the substitute player record is assigned
                    // to the correct team in the correct half of the inning
                    // 11 pinch hitter, 12 pinch runner - both are OFFENSIVE players
                    if ((columnValue[5] != "11") && (columnValue[5] != "12"))
                    {
                        // visiting team defensive player / home team batting
                        if ((columnValue[3] == "0") && (gameTeamCode == 1))
                        {
                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + textLine + outputDelimiter + visitingTeamID,
                                      true);
                        }
                        // visiting team defensive player / visiting team batting - bump up the eventNum
                        else if ((columnValue[3] == "0") && (gameTeamCode == 0))
                        {
                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + (eventNum + 1) + outputDelimiter
                                      + textLine + outputDelimiter + visitingTeamID,
                                      true);
                        }
                        // home team defensive player / visiting team batting 
                        else if ((columnValue[3] == "1") && (gameTeamCode == 0))
                        {
                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + textLine + outputDelimiter + homeTeamID,
                                      true);
                        }
                        // home teame defensive play / home team batting - bump up the eventNum
                        else // if ((columnValue[3] == "1") && (gameTeamCode == 1))
                        {
                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + (eventNum + 1) + outputDelimiter
                                      + textLine + outputDelimiter + homeTeamID,
                                      true);
                        }
                    }
                    // 11 pinch hitter, 12 pinch runner - both are OFFENSIVE players
                    else
                    {
                        WriteEventFile(outputPath,
                            outputFile + "_" + columnValue[0],
                            gameID + outputDelimiter
                            + inning + outputDelimiter
                            + gameTeamCode + outputDelimiter
                            + eventNum + outputDelimiter
                            + textLine + outputDelimiter + homeTeamID,
                            true);
                    }
                    break;

                case "data":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    WriteEventFile(outputPath,
                              outputFile + "_game" + columnValue[0],
                              gameID + outputDelimiter + textLine,
                              true);
                    break;

                case "badj":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    if (gameTeamCode == 0)
                    //  top of the inning - visiting team is batting
                    {
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + eventNum + outputDelimiter
                                  + textLine + outputDelimiter + visitingTeamID,
                                  true);
                    }
                    else if (gameTeamCode == 1)
                    //  bottom of the inning - home team is batting
                    {
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + eventNum + outputDelimiter
                                  + textLine + outputDelimiter + homeTeamID,
                                  true);
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    break;

                case "padj":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    if (gameTeamCode == 0)
                    {
                    // top of the inning - home team is pitching
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + eventNum + outputDelimiter
                                  + textLine + outputDelimiter + homeTeamID,
                                  true);
                    }
                    else if (gameTeamCode == 1)
                    {
                     // bottom of the inning - visiting team is pitching
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + eventNum + outputDelimiter
                                  + textLine + outputDelimiter + visitingTeamID,
                                  true);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    break;

                case "ladj":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    WriteEventFile(outputPath,
                              outputFile + "_" + columnValue[0],
                              gameID + outputDelimiter
                              + inning + outputDelimiter
                              + gameTeamCode + outputDelimiter
                              + eventNum + outputDelimiter
                              + textLine,
                              true);
                    break;

                case "com":

                    if (eventNumHold != eventNum)
                    {
                        commentSequence = 0;
                        eventNumHold = eventNum;
                    }
                    columnValue[1] = columnValue[1].Replace("\"","");

                    switch (columnValue[1])
                    {
                        case "replay":

                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + commentSequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "ej":

                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter 
                                      + commentSequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "umpchange":

                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + commentSequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "protest":

                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + commentSequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "suspend":

                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + commentSequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        default:
                            ++commentSequence;
                            commentText = textLine.Substring( textLine.IndexOf("com,") + 5);
                            commentText = commentText.Replace("\"", "");

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + eventNum + outputDelimiter
                                      + commentSequence + outputDelimiter
                                      + "com" + outputDelimiter
                                      + commentText,
                                      true);
                            break;
                    }
                    break;

                default:

                    WriteEventFile(outputPath,
                              outputFile + "_default",
                              gameID + outputDelimiter
                              + inning + outputDelimiter
                              + gameTeamCode + outputDelimiter
                              + eventNum + outputDelimiter
                              + commentSequence + outputDelimiter
                               + textLine,
                              true);
                    break;
            }
        }

        private void CreatePlayerOutput(string outputPath,
                                        string outputFile,
                                        string textLine,
                                        string seasonYear,
                                        string seasonGameType)
        {
            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

            // the last column of the player record tends to have a trailing space.
            // use trim to clean up that trailing space
            textLine = textLine.Trim();

            WriteEventFile(outputPath,
                      outputFile + "_players",
                      seasonYear + outputDelimiter + seasonGameType + outputDelimiter + textLine,
                      true);

        }

        private void WriteEventFile(string outputPath, 
                                    string outputFile, 
                                    string outputString, 
                                    bool appendToExistingFile )
        {
            using (StreamWriter recordWriter = new StreamWriter((outputPath + @"\" + outputFile), appendToExistingFile))
            {
                try
                {
                    recordWriter.WriteLine(outputString);
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The " + outputPath + "_" + outputFile +"file could not be written:");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

