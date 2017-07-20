﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////  need this for StreamReader
using System.IO;

namespace Retrosheet_DataFileIO
{
    public class DataFileIO
    {
        //private string inputPathFile;
        //private string outputPath;
        //private string outputFile;
        //private string textLine;

        private const char inputDelimiter = ',';
        private const char outputDelimiter = '|';

        // the following four variables make up the full key
        private string gameID = null;
        private int inning = 0;
        private int gameTeamCode = 0;
        private int sequence = 0;
        private int comSequence = 0;

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
                                      string outputPath)
        {
            string outputFile;

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
                                           "event");
                    //outputTextbox.Text += fileName + Environment.NewLine;
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
                                           "player");
                    //outputTextbox.Text += fileName + Environment.NewLine;
                }
                // team file
                else if (fileName.IndexOf("TEAM") > -1)
                {
                    outputFile = fileName.Substring(0, fileName.IndexOf("."));
                    outputPath = inputPath + @"\Output\";
                    ReadEventFiles(inputPath + @"\" + fileName,
                                           outputPath,
                                           outputFile,
                                           "team");
                    //outputTextbox.Text += fileName + Environment.NewLine;
                }
            }
        }


        private void ReadEventFiles(string inputPathFile,
                                    string outputPath,
                                    string outputFile,
                                    string fileType )
        {
            string[] columnValue;
            string textLine = null ;
            //string inputPathFile = inputPathFile;
            //string outputPath = outputPath;
            //string outputFile = outputFile;

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
                                          columnValue);
                    }
                    else if (fileType == "player")
                    {
                        CreatePlayerOutput(outputPath,
                                           outputFile,
                                           textLine);
                    }
                    else if (fileType == "team")
                    {
                        CreateTeamOutput(outputPath,
                                         outputFile,
                                         textLine);
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
                                      string textLine)
        {
            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

            WriteEventFile(outputPath,
                      outputFile + "_team",
                      textLine,
                      true);
        }

        private void CreateEventOutput(string outputPath,
                                       string outputFile,
                                       string textLine,
                                       string[] columnValue)
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
                    comSequence = 0;

                    break;

                case "version":
                    break;

                case "info":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    if ((columnValue[1] == "edittime") ||
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
                        WriteEventFile(outputPath,
                                  outputFile + "_game" + columnValue[0],
                                  gameID + outputDelimiter + textLine,
                                  true);
                    }
                    break;

                case "start":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    WriteEventFile(outputPath,
                              outputFile + "_" + columnValue[0],
                              gameID + outputDelimiter + textLine,
                              true);
                    break;

                case "play":

                    textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                    // next split up the contents of columnSix into the three parts that make up the Event
                    // split by forward slash and period
                    string[] stringArray = textLine.Split(outputDelimiter );
                    string columnSix = stringArray[6];

                    // eventModifier may contain multiple modifiers delimited with forward slash /
                    // eventRunnerAdvance may contain multiple advances delimited by semicolon ;
                    // multiple modifiers and advances are NOT split into seperate fields
                    string eventDescription ;
                    string eventModifier;
                    string eventRunnerAdvance;
                    int forwardSlashIndex = columnSix.IndexOf('/');
                    int periodIndex = columnSix.IndexOf('.');

                    if ((forwardSlashIndex < 0) && (periodIndex < 0))
                    // there are no modifiers or advances, only the description
                    {
                        eventDescription = columnSix;
                        eventModifier = null;
                        eventRunnerAdvance = null;
                    }
                    else if ((periodIndex > -1) && (forwardSlashIndex > periodIndex))
                    // there are no modifiers and the runner advances contain forward slashes
                    {
                        eventDescription = columnSix.Substring(0, periodIndex);
                        eventModifier = null;
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);

                    }
                    else if (forwardSlashIndex < 0) 
                    // there are no modifiers, only description and runner advances
                    {
                        eventDescription = columnSix.Substring(0, periodIndex);
                        eventModifier = null;
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);
                    }
                    else if (periodIndex < 0)
                    // there are no runner advances, only description and modifiers
                    {
                        eventDescription = columnSix.Substring(0, forwardSlashIndex);
                        eventModifier = columnSix.Substring(forwardSlashIndex + 1);
                        eventRunnerAdvance = null;
                    }
                    else
                    // all three are present - description, modifier, runner advance
                    {
                        eventDescription = columnSix.Substring(0, forwardSlashIndex);
                        eventModifier = columnSix.Substring(forwardSlashIndex + 1, periodIndex - forwardSlashIndex - 1);
                        eventRunnerAdvance = columnSix.Substring(periodIndex + 1);
                    }

                    // put the textLine back together from the stringArray
                    stringArray[6] = eventDescription + outputDelimiter + eventModifier + outputDelimiter + eventRunnerAdvance;
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
                        comSequence = 0;
                    }

                    //  capture the third part of the record key
                    if (gameTeamCode != Int32.Parse(columnValue[2]))
                    {
                        gameTeamCode = Int32.Parse(columnValue[2]);
                        sequence = 0;
                        comSequence = 0;
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

                    WriteEventFile(outputPath,
                              outputFile + "_" + columnValue[0],
                              gameID + outputDelimiter
                              + inning + outputDelimiter
                              + gameTeamCode + outputDelimiter
                              + sequence + outputDelimiter
                              + textLine,
                              true);
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

                    WriteEventFile(outputPath,
                              outputFile + "_" + columnValue[0],
                              gameID + outputDelimiter
                              + inning + outputDelimiter
                              + gameTeamCode + outputDelimiter
                              + sequence + outputDelimiter
                              + textLine,
                              true);
                    break;

                case "padj":

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

                    columnValue[1] = columnValue[1].Replace("\"","");

                    switch (columnValue[1])
                    {
                        case "replay":

                            comSequence = 0;
                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "ej":

                            comSequence = 0;
                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "umpchange":

                            comSequence = 0;
                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "protest":

                            comSequence = 0;
                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        case "suspend":

                            comSequence = 0;
                            textLine = textLine.Replace("\"", "");
                            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0] + "_" + columnValue[1],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + textLine,
                                      true);
                            break;

                        default:

                            ++comSequence;
                            commentText = textLine.Substring( textLine.IndexOf("com,") + 5);
                            commentText = commentText.Replace("\"", "");

                            WriteEventFile(outputPath,
                                      outputFile + "_" + columnValue[0],
                                      gameID + outputDelimiter
                                      + inning + outputDelimiter
                                      + gameTeamCode + outputDelimiter
                                      + sequence + outputDelimiter
                                      + comSequence + outputDelimiter
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
                              + textLine,
                              true);
                    break;
            }
        }

        private void CreatePlayerOutput(string outputPath,
                                        string outputFile,
                                        string textLine)
        {
            textLine = textLine.Replace(inputDelimiter, outputDelimiter);

            WriteEventFile(outputPath,
                      outputFile + "_players",
                      textLine,
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

