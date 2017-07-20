using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Retrosheet_Settings
{
    public class Settings
    {
        public string ReferenceDataInputPath { get; private set; }
        public string ReferenceDataInputFile { get; private set; }
        public bool ReferenceDataLoaded { get; private set; }

        public string PersonalDataInputPath { get; private set; }
        public string PersonalDataInputFile { get; private set; }
        public bool PersonalDataLoaded { get; private set; }

        public string BallparkDataInputPath { get; private set; }
        public string BallparkDataInputFile { get; private set; }
        public string BallparkDataOutputPath { get; private set; }
        public string BallparkDataOutputFile { get; private set; }
        public bool BallparkDataLoaded { get; private set; }

        public string EventDataID { get; private set; }
        public string EventDataInputPath { get; private set; }
        public string EventDataOutputPath { get; private set; }
        public bool EventDataLoaded { get; private set; }

        // constructor
        public Settings()
        {
            XmlDocument xmlSettings = new XmlDocument();
            //xmlSettings.Load(@"C:\users\mmr\documents\retrosheet\settings.xml");
            // relative path
            // Documents /Visual Studio 2015/Projects/Retrosheet/Retrosheet_Console/bin
            string sourcePath = Environment.CurrentDirectory;

            xmlSettings.Load(sourcePath + @"\settings.xml");

            //var settingsNode = xmlSettings.SelectNodes("/settings/reference_data");
            var settingsNode = xmlSettings.SelectNodes("/settings");
            StringBuilder result = new StringBuilder();

            foreach (XmlNode node in settingsNode)
            {
                foreach (XmlNode subNode in node)
                {
                    if (subNode.Name == "reference_data")
                    {
                        result.Append(subNode.Name).Append(", ");
                        result.Append(subNode.Attributes["input_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["input_file"].Value).Append(", ");
                        result.Append(subNode.Attributes["data_loaded"].Value).Append(Environment.NewLine);

                        ReferenceDataInputPath = subNode.Attributes["input_path"].Value;
                        ReferenceDataInputFile = subNode.Attributes["input_file"].Value;
                        if (subNode.Attributes["data_loaded"].Value == "TRUE")
                        {
                            ReferenceDataLoaded = true;
                        }
                        else
                        {
                            ReferenceDataLoaded = false;
                        }
                    }
                    else if (subNode.Name == "personal_data")
                    {
                        result.Append(subNode.Name).Append(", ");
                        result.Append(subNode.Attributes["input_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["input_file"].Value).Append(", ");
                        result.Append(subNode.Attributes["data_loaded"].Value).Append(Environment.NewLine);

                        PersonalDataInputPath = subNode.Attributes["input_path"].Value;
                        PersonalDataInputFile = subNode.Attributes["input_file"].Value;
                        if (subNode.Attributes["data_loaded"].Value == "TRUE")
                        {
                            ReferenceDataLoaded = true;
                        }
                        else
                        {
                            ReferenceDataLoaded = false;
                        }
                    }
                    else if (subNode.Name == "ballpark_data")
                    {
                        result.Append(subNode.Name).Append(", ");
                        result.Append(subNode.Attributes["input_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["input_file"].Value).Append(", ");
                        result.Append(subNode.Attributes["output_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["output_file"].Value).Append(", ");
                        result.Append(subNode.Attributes["data_loaded"].Value).Append(Environment.NewLine);

                        BallparkDataInputPath = subNode.Attributes["input_path"].Value;
                        BallparkDataInputFile = subNode.Attributes["input_file"].Value;
                        BallparkDataOutputPath = subNode.Attributes["output_path"].Value;
                        BallparkDataOutputFile = subNode.Attributes["output_file"].Value;
                        if (subNode.Attributes["data_loaded"].Value == "TRUE")
                        {
                            BallparkDataLoaded = true;
                        }
                        else {
                            BallparkDataLoaded = false;
                        }
                    }
                    else
                    {
                        result.Append(subNode.Name).Append(", ");
                        result.Append(subNode.Attributes["data_id"].Value).Append(", ");
                        result.Append(subNode.Attributes["input_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["output_path"].Value).Append(", ");
                        result.Append(subNode.Attributes["data_loaded"].Value).Append(Environment.NewLine);

                        EventDataID = subNode.Attributes["data_id"].Value;
                        EventDataInputPath = subNode.Attributes["input_path"].Value;
                        EventDataOutputPath = subNode.Attributes["output_path"].Value;
                        if (subNode.Attributes["data_loaded"].Value == "TRUE")
                        {
                            EventDataLoaded = true;
                        }
                        else
                        {
                            EventDataLoaded = false;
                        }
                        
                    }
                }
            }

            Console.WriteLine(result);
            //Console.ReadLine();
        }

        public void BackupSettings()
        {
            string fileName = "settings.xml";
            string sourcePath = Environment.CurrentDirectory;
            string targetPath = sourcePath + @"\backup";
            string sourceFile = Path.Combine(sourcePath, fileName);
            string targetFile = Path.Combine(targetPath, fileName + "_" + GetTimestamp(DateTime.Now));

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            File.Copy(sourceFile, targetFile,true);

        }

        private String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }

        public void WriteSettings(string[ ] settingsArray)
        {
            string[] settingsElement;

            XmlDocument xmlDocument = new XmlDocument();
            XmlNode settingsNode;
            XmlNode dataNode;

            XmlAttribute xmlAttribute;

            settingsNode = xmlDocument.CreateElement("settings");
            xmlDocument.AppendChild(settingsNode);
                
            foreach (string settingsEntry in settingsArray)
            {
                settingsElement = settingsEntry.Split('|');

                dataNode = xmlDocument.CreateElement(settingsElement[0]);

                xmlAttribute = xmlDocument.CreateAttribute("data_loaded");
                xmlAttribute.Value = settingsElement[1];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);

                xmlAttribute = xmlDocument.CreateAttribute("input_path");
                xmlAttribute.Value = settingsElement[2];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);


                xmlAttribute = xmlDocument.CreateAttribute("input_file");
                xmlAttribute.Value = settingsElement[3];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);

                xmlAttribute = xmlDocument.CreateAttribute("output_path");
                xmlAttribute.Value = settingsElement[4];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);

                xmlAttribute = xmlDocument.CreateAttribute("output_file");
                xmlAttribute.Value = settingsElement[5];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);

                xmlAttribute = xmlDocument.CreateAttribute("data_id");
                xmlAttribute.Value = settingsElement[6];
                dataNode.Attributes.Append(xmlAttribute);
                settingsNode.AppendChild(dataNode);
            }
            xmlDocument.Save(Environment.CurrentDirectory + @"\settings.xml");
        }
    }
}
