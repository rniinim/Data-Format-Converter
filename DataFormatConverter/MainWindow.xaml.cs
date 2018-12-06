using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Windows.Controls;

namespace DataFormatConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = "";
        private string jsonText = "";
        XmlDocument doc;
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Reads file and converts it to desired form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            
            //initialization
            string content = MainTextBlock.Text = path = "";
            jsonItem.Visibility = xmlItem.Visibility = csvItem.Visibility = Visibility.Visible;
            var fileContent = string.Empty;
            var filePath = string.Empty;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //Accepted input filetypes .json, .xml
            openFileDialog.Filter = "Json file |*.json|Xml file |*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.RestoreDirectory = true;
            
            //Dialog open successfully
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = (Path.GetFileName(openFileDialog.FileName));
                MainTextBlock.Text += "File opened from: \n";
                MainTextBlock.Text += openFileDialog.FileName;
                path = Path.GetExtension(filePath);

                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog.FileName);
                content  = (sr.ReadToEnd());
                sr.Close();
            }

            if (path == ".xml")
            {
                doc = new XmlDocument();
                doc.LoadXml(content);
                jsonText = JsonConvert.SerializeXmlNode(doc);

                MainTextBlock.Text += "\nXml file transformed to json\nSave as .json or .txt";
                xmlItem.Visibility = Visibility.Hidden;
            }
            else if(path == ".json"){
                content = content.Replace("@", string.Empty);
                string contentNoSpaces = content.Replace(" ", string.Empty);
                int curlyB = contentNoSpaces.IndexOf(':');
                //checks if root json file has multiple properties
                if (contentNoSpaces[curlyB+1] == '{')
                {
                    doc = JsonConvert.DeserializeXmlNode(content);
                }
                
                else
                {
                    //adds new root node because it's not possible to make xml from multiple root nodes
                    doc = JsonConvert.DeserializeXmlNode(content, "root");
                    MainTextBlock.Text += "\nAdded \"root\"-node because multiple root properties";
                }
                MainTextBlock.Text += "\nJson file transformed to xml\n\nSave as .xml or .txt";
                jsonItem.Visibility = Visibility.Hidden;
            }
            else if(path== ".csv")
            {

                csvItem.Visibility = Visibility.Hidden;
            }
            
        }

        /// <summary>
        /// Saves files in given format (.xml/.json/.txt)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
  
            MainTextBlock.Text = "";
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (path == ".xml")
            {
                saveFileDialog.Filter = "Json file |*.json|Text file |*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, jsonText);
                    MainTextBlock.Text += "\nNew file successfully saved!";
                }
            }
            else if (path == ".json")
            {
                saveFileDialog.Filter = "XML File | *.xml|Text file | *.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    doc.Save(saveFileDialog.FileName);
                    MainTextBlock.Text += "\nNew file successfully saved!";
                }

            }

        }
    }
}
