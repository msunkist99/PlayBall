using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////  need this for StreamReader
using System.IO;


namespace Retrosheet_LoadTextFiles
{
    public class IO_processes
    {
        // constructor
        public IO_processes()
        {
        }

        public void DeleteDirectory(string outputPath)
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
        }

        public void CreateDirectory(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
        }

        public void WriteFile(string outputPath,
                               string outputFile,
                               string outputString,
                               bool appendToExistingFile)
        {
            using (StreamWriter recordWriter = new StreamWriter((outputPath + outputFile), appendToExistingFile))
            {
                try
                {
                    recordWriter.WriteLine(outputString);
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The " + outputPath + "_" + outputFile + "file could not be written:");
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
