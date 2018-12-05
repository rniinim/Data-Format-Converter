using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void ReadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            MainTextBlock.Text = "";
            string content = "";
            XmlDocument doc;
            var fileContent = string.Empty;
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (.json)|*.json|Xml files (.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //openFileDialog.RestoreDirectory = true;
            string path = "";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                filePath = (Path.GetFileName(filename));
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
                string jsonText = JsonConvert.SerializeXmlNode(doc);

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
    }
}
