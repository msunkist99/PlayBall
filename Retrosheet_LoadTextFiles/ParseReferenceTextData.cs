using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Retrosheet_LoadTextFiles
{
    public class ParseReferenceTextData
    {
        // constructor
        public ParseReferenceTextData()
        {
        }

        private IO_processes io = new IO_processes();

        public void DeleteDirectory(string outputPath)
        {
            if (Directory.Exists(outputPath))
            {
                io.DeleteDirectory(outputPath);
            }
        }

        public void ReadWriteFiles(string inputPathFile,
                                   string outputPath,
                                   string outputFile,
                                   string fileType)
        {
            string[] columnValue;
            string textLine = null;
            string textLine1 = null;
            string textLine2 = null;
            int doubleQuoteIndex = 0;
            //DateTime dateTime;

            if (!Directory.Exists(outputPath))
            {
                io.CreateDirectory(outputPath);
                //Directory.CreateDirectory(outputPath);
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
                        columnValue = textLine.Split('|');

                        io.WriteFile(outputPath,
                                     outputFile,
                                     textLine,
                                     true);
                    }

                    /*
                    BallparkDTO ballparkDTO = new BallparkDTO();

                    //Retrosheet_Persist.Ballpark newBallpark = new Retrosheet_Persist.Ballpark();

                    ballparkDTO.RecordID = Guid.NewGuid();
                    ballparkDTO.ID = columnValue[0];
                    ballparkDTO.Name = columnValue[1];
                    ballparkDTO.AKA = columnValue[2];
                    ballparkDTO.City = columnValue[3];
                    ballparkDTO.State = columnValue[4];

                    if (DateTime.TryParse(columnValue[5], out dateTime))
                    {
                        ballparkDTO.StartDate = dateTime;
                    }
                    else
                    {
                        ballparkDTO.StartDate = DateTime.MinValue;
                    }

                    if (DateTime.TryParse(columnValue[6], out dateTime))
                    {
                        ballparkDTO.EndDate = dateTime;
                    }
                    else
                    {
                        ballparkDTO.EndDate = DateTime.MaxValue;
                    }

                    ballparkDTO.League = columnValue[7];
                    ballparkDTO.Notes = columnValue[8];

                    BallparkPersist.CreateBallpark(ballparkDTO);
                    */
                }
                    Console.WriteLine(textLine);
            }
        }
    }
}
