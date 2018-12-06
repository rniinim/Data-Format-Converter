using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml;

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
            string content = MainTextBlock.Text = path = "";
            
            var fileContent = string.Empty;
            var filePath = string.Empty;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Json file |*.json|Xml file |*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.RestoreDirectory = true;
            
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

                MainTextBlock.Text += "\n xml file transformed to json";
                
            }
            else if(path == ".json"){

                doc = JsonConvert.DeserializeXmlNode(content);
                MainTextBlock.Text += "\n json file transformed to xml";
            }
            else
            {
                
            }
            
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (path == ".xml")
            {
                saveFileDialog.Filter = "Json file |*.json|Text file |*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, jsonText);
                    MainTextBlock.Text += "\n New file successfully saved!";
                }
            }
            else if (path == ".json")
            {
                saveFileDialog.Filter = "XML File | *.xml|Text file | *.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    doc.Save(saveFileDialog.FileName);
                    MainTextBlock.Text += "\n New file successfully saved!";
                }

            }

        }
    }
}
