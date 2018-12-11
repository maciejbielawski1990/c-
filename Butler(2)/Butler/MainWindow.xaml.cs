using HtmlAgilityPack;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Butler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int option;
        Settings setting;
        List<ButlerPlayer> Baza;
        int tournament_id;
        int round_id;


        public MainWindow()
        {
            InitializeComponent();
            setting = new Settings();
          //  test();
            //Printer p = new Printer();
            //p.Print();
                  
        }

        void test()
        {
            List<string> result = new List<string>();
            string glowna = "http://www.worldbridge.org/repository/tourn/wroclaw.16/Microsite/RunningScores/Asp/knockoutphaseconditstatMultievColours.asp?qtournid=1240&qphase=16";
            WebClient client = new WebClient();
            string html = client.DownloadString(glowna);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//a[@href]"); //pobieram wszystkie linki z tej strony

            //foreach (var link in links)
            //{
            //    if (link.Attributes["href"].Value.Contains("BoardDetails")) // jesli zawiera rozdanie, dodaje do listy
            //    {
            //        string url = serwer + link.Attributes["href"].Value;
            //        result.Add(url);
            //    }
            //}

            
        }


        // obsluga buttona Enter
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            option = combo1.SelectedIndex;
            switch (option)
            {
                case 0: // opcja WBF
                  //  Loadsettings("settingsWBFRR.xml");
                    ReadSettingsWBFRR();
                    SaveSettings();
                    MakeButlerFromWBFRR();
                   
                    break;
            }

            SaveSettings();
        }

        // robi butler WBF
        public void MakeButlerFromWBFRR()
        {
           
            if (!setting.loadfile)
                Baza = new List<ButlerPlayer>();

            foreach (int r in setting.rounds)
            {
                string url = setting.url + r.ToString(); 
                WBFReader reader = new WBFReader(url, setting.serwer,tournament_id,r);
                List<TablesButlerData> tablesData = reader.ReadWBFPage();
                Calculator.CalculateImps(ref tablesData, setting.count_boards);
                BaseEditor.ObslugaBazy(ref Baza, tablesData);
            
            }

            FilesEditor.SaveButler(setting.save_as, Baza);
            MessageBox.Show("Done");
        }


        // czyta dane z okna i przetwarza je do ustawien konwertowania
        public void ReadSettingsWBFRR()
        {
            setting.count_boards = int.Parse(textboxBoards.Text);
            setting.count_tables = int.Parse(textboxTable.Text);
            string roundstring = textboxRounds.Text;
            setting.rounds = new List<int>();
            string[] rounds = roundstring.Split(',');
            foreach (string r in rounds)
            {
                if (r.Contains('-'))
                {
                    string[] tmp = r.Split('-');
                    int ll = int.Parse(tmp[0]);
                    int rr = int.Parse(tmp[1]);
                    for (int i = ll; i <= rr; i++)
                    {
                        setting.rounds.Add(i);
                    }
                }
                else
                {
                    setting.rounds.Add(int.Parse(r));
                }
            }
            setting.save_as = textboxSaveAs.Text;
            setting.serwer = "http://www.worldbridge.org/repository/tourn/" + textboxUrl.Text + "/Microsite/Asp/";
            setting.loadfile = chechboxLoadFileXML.IsChecked.Value;
       
            setting.openfile = checkboxOpen.IsChecked.Value;

            tournament_id = int.Parse(textboxID.Text);
           
            setting.url = setting.serwer + "RoundTeams.asp?qtournid=" + int.Parse(textboxID.Text) + "&qroundno=";


        }

     

        private string LoadFileXMLBaza()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            setting.loadfile = true;
            Baza = FilesEditor.LoadButlerFromXML(dialog.FileName);
 
            return dialog.FileName;
        }

        private void SaveSettings()
        {
            StreamWriter plik = new StreamWriter("settings.xml");
            XmlSerializer serialize = new XmlSerializer(typeof(Settings));

            serialize.Serialize(plik, setting);
            plik.Close();
        }
        private void Loadsettings(string url)
        {
            try
            {
                StreamReader reader = new StreamReader(url);
                XmlSerializer ser = new XmlSerializer(typeof(Settings));
                Settings tmp = (Settings)ser.Deserialize(reader);
                setting = tmp;

                textboxBoards.Text = setting.count_boards.ToString();
                string[] s = setting.url.Split('/');
                textboxUrl.Text = s[5];
                textboxID.Text = s[8].Split('=')[1].Split('&')[0];
                textboxTable.Text = setting.count_tables.ToString();

                chechboxLoadFileXML.IsChecked = setting.loadfile;
                checkboxOpen.IsChecked = setting.openfile;
                combo1.SelectedIndex = 0;
            }
            catch { }
        
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo1.SelectedIndex == 0)
               Loadsettings("settingsWBFRR.xml");
            
        }

        private void chechboxLoadFileXML_Checked(object sender, RoutedEventArgs e)
        {
           string filename = LoadFileXMLBaza();
           string[] s = filename.Split('\\');
           string nazwa = s[s.Count() - 1].Split('.')[0];

           textboxSaveAs.Text = nazwa;

           chechboxLoadFileXML.Content += " - " + nazwa + ".xml "  + "is loaded";
     
        }
    }
}
