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
        private int sequence = 0;
        private int commentSequence = 0;
        private int sequenceHold = 0;
        private string homeTeamID = null;
        private string visitingTeamID = null;
        private int eventNum = 0;

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
                    sequence = 0;
                    commentSequence = 0;
                    homeTeamID = null;
                    visitingTeamID = null;
                    eventNum = 0;

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

                    stringArray[4] = stringArray[4].Substring(0, 1) + outputDelimiter + stringArray[4].Substring(1, 1);
                    // next split up the contents of columnSix into the three parts that make up the Event
                    //  eventSequence
                    //  eventModifier
                    //  eventRunnerAdvance
                    // split by forward slash and period
                    string columnSix = stringArray[6];

                    // columnSix
                    // characters up to the leftmost / make up the eventSequence information
                    //  eventSequence - if the leftmost characters are non-numeric this is the eventType.  Default event type is '~generic out'.
                    //  values inclosed in () indicate the eventPlayOnRunner values, i.e 'play on runner on first', 'play on runner on second', 'play on runner on third', - default is 'play on batter'
                    //  there can be multiple eventPlayOnRunnerValues
                    //  remaining numeric characters make up the eventFieldedBy values - each of the numeric characters will be 1-9 and 
                    //  tell you the fielder(s) involved in the event

                    // characters between the leftmost / and leftmost . make up the eventModifier information
                    //  there can be multiple eventModifier values delimited with /
                    //  the first eventModifier can contain the eventHitLocation value
                    //  multiple eventModifier values are delimited by a | in the database column

                    // characters following the . make up the eventRunnerAdvance information
                    //  there can be multiple eventRunnerAdvances delimited with ;
                    //  multiple eventRunnerAdvance values are delimited by a | in the database column

                    int forwardSlashIndex = columnSix.IndexOf('/');
                    int periodIndex = columnSix.IndexOf('.');

                    string eventSequence = "";
                    string eventModifier = "";
                    string eventRunnerAdvance = "";
                    string eventHitLocation = "";
                    string eventFieldedBy = "";
                    string eventPlayOnRunner = "";
                    string eventType = "";

                    // first split up columnSix into
                    // eventSequence, eventModifier, eventRunnerAdvance

                    if ((forwardSlashIndex < 0) && (periodIndex < 0))
                        // no eventModifiers, no eventRunnerAdvance
                        // only eventSequence
                    {
                        eventSequence = columnSix;
                    }
                    else if ((periodIndex > -1) && (forwardSlashIndex > periodIndex))
                    {
                        // there are no modifiers and the runner advance data contains a forward slash
                        eventSequence = columnSix.Substring(0, periodIndex);
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);
                    }
                    else if ((forwardSlashIndex > -1) && (periodIndex < 0))
                        // there are eventModifier values but no eventRunnerAdvance values
                    {
                        eventSequence = columnSix.Substring(0, forwardSlashIndex);
                        eventModifier = columnSix.Substring(forwardSlashIndex + 1);
                    }
                    else if ((forwardSlashIndex < 0) && (periodIndex > -1))
                    {
                        // there are no eventModifiers but there are eventRunnerAdvances
                        eventSequence = columnSix.Substring(0, periodIndex);
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);
                    }
                    else
                    {
                        // there are both eventModifiers and eventRunnerAdvances
                        eventSequence = columnSix.Substring(0, forwardSlashIndex);
                        eventModifier = columnSix.Substring(forwardSlashIndex + 1, periodIndex - forwardSlashIndex -1);
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);
                    }

                    // eventSequence - look for '(1)', '(2)', '(3)' - and set eventPlayOnRunner value
                    // eventSequence - look for numeric value - and set eventFieldedBy value
                    // eventSequence - any remaining values should be non-numeric - set eventType value 

                    if ((eventSequence.Contains("(")) && (eventSequence.Contains(")")))
                    {
                        int openParmIndex = eventSequence.IndexOf("(");
                        int closeParmIndex = eventSequence.IndexOf(")");
                        eventPlayOnRunner = eventSequence.Substring(openParmIndex + 1, closeParmIndex - openParmIndex - 1);
                        eventSequence = eventSequence.Replace((eventSequence.Substring(openParmIndex, closeParmIndex - openParmIndex + 1)), "");
                    }

                    /*
                    // if there are multiple values in eventPlayOnRunner then deliminate with "|"
                    if (eventPlayOnRunner != "")
                    {
                        char[] eventPlayOnRunnerArray = eventPlayOnRunner.ToArray();
                        eventPlayOnRunner = "";
                        foreach (char value in eventPlayOnRunnerArray)
                        {
                            eventPlayOnRunner = eventPlayOnRunner + value.ToString() + outputDelimiter;
                        }
                        //  remove trailing "|"
                        if (eventPlayOnRunner.Substring(eventPlayOnRunner.Length - 1) == "|")
                        {
                            eventPlayOnRunner = eventPlayOnRunner.Substring(0, eventPlayOnRunner.Length - 1);
                        }
                    }
                    */

                    /*
                    if (eventSequence.Contains("(1)"))
                    {
                        eventPlayOnRunner = "1";
                        eventSequence = eventSequence.Replace("(1)", string.Empty);
                    }
                    else if (eventSequence.Contains("(2)"))
                    {
                        eventPlayOnRunner = "2";
                        eventSequence = eventSequence.Replace("(2)", string.Empty);
                    }
                    else if (eventSequence.Contains("(3)"))
                    {
                        eventPlayOnRunner = "3";
                        eventSequence = eventSequence.Replace("(3)", string.Empty);
                    }
                    else
                    {
                        eventPlayOnRunner = "B";
                    }
                    */

                    eventType = Regex.Replace(eventSequence, @"\d", "");
                    eventFieldedBy = Regex.Replace(eventSequence, @"\D", "");

                    /*
                    // eventType may contain multiple type values = replace + delimiter with |
                    eventType.Replace("+", "|");
                    */

                    if (eventType == "")
                    {
                        eventType = "~";  // default generic out
                    }

                    /*
                    // if there are multiple values in eventFieldedBy then deliminate with "|"
                    if (eventFieldedBy != "")
                    {
                        char[] eventFieldedByArray = eventFieldedBy.ToArray();
                        eventFieldedBy = "";
                        foreach (char value in eventFieldedByArray)
                        {
                            eventFieldedBy = eventFieldedBy + value.ToString() + outputDelimiter;
                        }
                        //  remove trailing "|"
                        if (eventFieldedBy.Substring(eventFieldedBy.Length - 1) == "|")
                        {
                            eventFieldedBy = eventFieldedBy.Substring(0, eventFieldedBy.Length - 1);
                        }
                    }
                    */


                    eventSequence = "";  //  everything that was originally in the eventSequence should now have been moved to eventType or eventFieldedBy, so empty eventSequence

                    //  eventModifier may contain the hit location so lets grab that
                    if (eventModifier != "")
                    {
                        foreach (string hitLocation in hitLocations)
                        {
                            if (eventModifier.Contains(hitLocation))
                            {
                                eventHitLocation = hitLocation;
                                eventModifier = eventModifier.Replace(hitLocation, string.Empty);
                                break;
                            }
                        }
                    }

                    if (eventModifier != "")
                    {
                        if (eventModifier.Substring(0, 1) == "/")
                        {
                            eventModifier = eventModifier.Substring(1);
                        }
                    }

                    /*
                    // eventModifier - there can be multiple modifier values - replace the / delimiter with |
                    if (eventModifier != "")
                    {
                        eventModifier = eventModifier.Replace("/","|");
                    }
                    */

                    /*
                    // eventRunnerAdvance - there can be multiple runner advance values - replace the ; delimiter with |
                    if (eventRunnerAdvance != "")
                    {
                        eventRunnerAdvance = eventRunnerAdvance.Replace(";", "|");
                    }
                    */

                    // put the textLine back together from the stringArray
                    // eventSequence should be empty
                    // capture columnSix original contents in the database for debugging and analysis purposes

                    eventNum++;
                    stringArray[6] = eventSequence + outputDelimiter + eventModifier
                                                   + outputDelimiter + eventRunnerAdvance
                                                   + outputDelimiter + eventHitLocation
                                                   + outputDelimiter + eventFieldedBy
                                                   + outputDelimiter + eventPlayOnRunner
                                                   + outputDelimiter + eventType
                                                   + outputDelimiter + columnSix
                                                   + outputDelimiter + eventNum;
                    textLine = null;

                    for (int i = 0; i < stringArray.Length ; i++)
                    {
                        textLine += stringArray[i] ;
                        if(i < stringArray.Length - 1)
                        {
                            textLine += outputDelimiter;
                        }
                    } 

                    //  capture the second part of the record key
                    if (inning != Int32.Parse(columnValue[1]))
                    {
                        inning = Int32.Parse(columnValue[1]);
                        sequence = 0;
                        commentSequence = 0;
                    }

                    //  capture the third part of the record key
                    //  0 - top of the inning - visiting team
                    //  1 - bottom of the inning - home team
                    if (gameTeamCode != Int32.Parse(columnValue[2]))
                    {
                        gameTeamCode = Int32.Parse(columnValue[2]);
                        sequence = 0;
                        commentSequence = 0;
                    }

                    //  capture the fourth part of the record key
                    ++sequence;

                    WriteEventFile(outputPath,
                              outputFile + "_" + columnValue[0],
                              gameID + outputDelimiter
                              + inning + outputDelimiter
                              + gameTeamCode + outputDelimiter
                              + sequence + outputDelimiter
                              + textLine,
                              true);
                    break;

                case "sub":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    if (columnValue[3] == "0")
                    {
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + sequence + outputDelimiter
                                  + textLine + outputDelimiter + visitingTeamID,
                                  true);
                    }
                    else
                    {
                        WriteEventFile(outputPath,
                                  outputFile + "_" + columnValue[0],
                                  gameID + outputDelimiter
                                  + inning + outputDelimiter
                                  + gameTeamCode + outputDelimiter
                                  + sequence + outputDelimiter
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
                                  + sequence + outputDelimiter
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
                                  + sequence + outputDelimiter
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
                                  + sequence + outputDelimiter
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
                                  + sequence + outputDelimiter
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
                              + sequence + outputDelimiter
                              + textLine,
                              true);
                    break;

                case "com":

                    if (sequenceHold != sequence)
                    {
                        commentSequence = 0;
                        sequenceHold = sequence;
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
                                      + sequence + outputDelimiter
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
                                      + sequence + outputDelimiter 
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
                                      + sequence + outputDelimiter
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
                                      + sequence + outputDelimiter
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
                                      + sequence + outputDelimiter
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
                                      + sequence + outputDelimiter
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
                              + sequence + outputDelimiter
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

