using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Retrosheet_EventIO;
using System.IO;

namespace Retrosheet_Wrapper
{
    public partial class Form1 : Form
    {
        public EventIO eventIO = new EventIO();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void startButton_Click(object sender, EventArgs e)
        {
            ProcessDirectory();
        }

        private void ProcessDirectory()
        {
            bool deleteFiles = false;

            string inputPath = null;
            string outputPath = null;
            string outputFile = null;

            deleteFiles = deleteFilesCheckbox.Checked;
            startButton.Enabled = false;
            deleteFilesCheckbox.Enabled = false;

            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                inputPath = Path.GetFullPath(folderBrowserDialog1.SelectedPath);
            }

            outputTextbox.Text = "PLAY BALL !!!!!" + Environment.NewLine;
            Application.DoEvents();

            if (deleteFilesCheckbox.Checked == true)
            {
                outputPath = inputPath + @"\Output";
                eventIO.DeleteDirectory(outputPath);
            }

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(inputPath);

            foreach (string pathFile in fileEntries)
            {
                string fileName = Path.GetFileName(pathFile);
                outputPath = inputPath + @"\Output\" + fileName.Substring(0, fileName.IndexOf(".")) + @"\" ;
                Application.DoEvents();

                // event file
                if (fileName.IndexOf(".EV") > -1 )
                {
                    outputFile = fileName.Substring(0, fileName.IndexOf("."));

                    //  the @ tells the compiler to ignore special characters (\) in the string
                    eventIO.ReadWriteFiles(inputPath + @"\" + fileName,
                                           outputPath,
                                           outputFile,
                                           "event");
                    outputTextbox.Text += fileName + Environment.NewLine;
                }
                // player file
                else if (fileName.IndexOf(".ROS") > -1)
                {
                    outputFile = fileName.Substring(3, 4) + fileName.Substring(0, 3);
                    outputPath = inputPath + @"\Output\" + outputFile + @"\";
                    //  the @ tells the compiler to ignore special characters (\) in the string
                   eventIO.ReadWriteFiles(inputPath + @"\" + fileName,
                                          outputPath,
                                          outputFile,
                                          "player");
                    outputTextbox.Text += fileName + Environment.NewLine;
                }
                // team file
                else if (fileName.IndexOf("TEAM") > -1)
                {
                    outputFile = fileName.Substring(0, fileName.IndexOf("."));
                    outputPath = inputPath + @"\Output\";
                    eventIO.ReadWriteFiles(inputPath + @"\" + fileName,
                                           outputPath,
                                           outputFile,
                                           "team");
                    outputTextbox.Text += fileName + Environment.NewLine;
                }
            }
            startButton.Enabled = true;
            deleteFilesCheckbox.Enabled = true;
        }
    }
}
