using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Collections.Generic;
using System.Linq;

namespace DataFormatConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string extension;
        private string jsonText;
        private string fileName;

        XmlDocument doc;

        public MainWindow()
        {
            InitializeComponent();
            Initialization();
        }

        private void Initialization()
        {
            MainTextBlock.Text = "Welcome to Data Format Converter!\nClick \"Read file\" and choose either JSON or XML file to start.";
            readFileBtn.FontWeight = FontWeights.Bold;
            fileName = jsonText = extension = string.Empty;
        }


        /// <summary>
        /// Reads file and converts it to desired form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //initialization
            string content = MainTextBlock.Text = extension = string.Empty;
            string filePath = string.Empty;

        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //Accepted input filetypes .json, .xml
            openFileDialog.Filter = "Json file |*.json|Xml file |*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.RestoreDirectory = true;
            
            //Dialog open successfully
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                fileName = (Path.GetFileName(filePath));
                MainTextBlock.Text += "File opened from: \n";
                MainTextBlock.Text += filePath;
                extension = Path.GetExtension(fileName);

                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                content  = (sr.ReadToEnd());
                sr.Close();
            }

            if (extension == ".xml")
            {
                doc = new XmlDocument();
                doc.LoadXml(content);
                MainTextBlock.Text += "\n\nXml file transformed to json\nSave as .json or .txt";
                readFileBtn.FontWeight = FontWeights.Normal;
                SaveFileBtn.FontWeight = FontWeights.Bold;
            }
            else if (extension == ".json")
            {
                //XML tags names cannot contain an "@"
                content = content.Replace("@", string.Empty);
                string contentNoSpaces = content.Replace(" ", string.Empty);
                int curlyB = contentNoSpaces.IndexOf(':');

                
                //checks if root json file has multiple properties
                if (contentNoSpaces[curlyB + 1] == '{')
                {
                    doc = JsonConvert.DeserializeXmlNode(content);
                }
                else
                {
                    //adds new root node because it's not possible to make xml from multiple root nodes
                    doc = JsonConvert.DeserializeXmlNode(content, "root");
                    MainTextBlock.Text += "\n\nAdded \"root\"-node because multiple root properties";
                }
                MainTextBlock.Text += "\n\nJson file transformed to xml\n\nSave as .xml or .txt by clicking \"Save Converted File\".";
                readFileBtn.FontWeight = FontWeights.Normal;
                SaveFileBtn.FontWeight = FontWeights.Bold;
            }
            else
                Initialization();


        }

        /// <summary>
        /// Saves files in given format (.xml/.json/.txt)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainTextBlock.Text = string.Empty;
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            //Take filename from previous file
            int indexOfExtension = fileName.IndexOf('.');
            fileName = fileName.Substring(0, indexOfExtension);
            saveFileDialog.FileName = fileName;

            if (extension == ".xml")
            {
                
                saveFileDialog.Filter = "Json file |*.json|Text file |*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    jsonText = JsonConvert.SerializeXmlNode(doc);
                    File.WriteAllText(saveFileDialog.FileName, jsonText);
                    MainTextBlock.Text += "\nNew file successfully saved!\nClick \"Read File\" to save new one.";
                }
                readFileBtn.FontWeight = FontWeights.Bold;
                SaveFileBtn.FontWeight = FontWeights.Normal;

            }
            else if (extension == ".json")
            {
                
                saveFileDialog.Filter = "XML File | *.xml|Text file | *.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    doc.Save(saveFileDialog.FileName);
                    MainTextBlock.Text += "\nNew file successfully saved!\nClick \"Read File\" to save new one.";
                }
                readFileBtn.FontWeight = FontWeights.Bold;
                SaveFileBtn.FontWeight = FontWeights.Normal;
            }
            else
            {
                MessageBox.Show("Something went wrong!\nMake sure that you choose .json, .xml or .txt file.");
            }

        }
    }
}
