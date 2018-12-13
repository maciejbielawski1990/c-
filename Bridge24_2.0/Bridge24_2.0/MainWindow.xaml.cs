using MigraDoc.RtfRendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using MigraDoc.DocumentObjectModel;
using System.Diagnostics;
using System.Net;
namespace Bridge24_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        VugraphLin vugraph,vugraph2;
        BBOGameReader game;
        public static string dir;
        public static bool gra = true;

        public MainWindow()
        {
            /*
            WebRequest request = WebRequest.Create("http://www.brydz.ampio.pl//Gawel2.txt");
            WebResponse response;
            bool aktualna = true;
            try
            {
               response =  request.GetResponse();               
            }

            catch
            {
                
                aktualna = false;
            }

            if (!aktualna)
            {
                MessageBox.Show("Wersja nieaktualna");
                Close();
            }
            */
            InitializeComponent();

            dir = Directory.GetCurrentDirectory();
            Ustawienia.ReadNameBase();
            Ustawienia.LoadSettingsFromFile();
            
            
        }

        #region obsluga buttonow
        private void buttonTrening_Click(object sender, RoutedEventArgs e)
        {
            panelTrening.Visibility = Visibility.Visible;
            panelUstawienia.Visibility = Visibility.Hidden;
        }

        private void buttonUstawiwnia_Click(object sender, RoutedEventArgs e)
        {
            panelUstawienia.Visibility = Visibility.Visible;
            panelTrening.Visibility = Visibility.Hidden;
            panelTest.Visibility = Visibility.Hidden;
           
            PrintUstawienieFrom();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vugraph = NowyTrening();
            game = NowyTrening2();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //openxml
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //savexml
        }

        private void buttonprint_Click(object sender, RoutedEventArgs e)
        {
            //print
            PrintTrening();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // previous
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //next
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
          //save settings
            ReadUstawieniaForm();
            Ustawienia ustawienia_ = new Ustawienia();
            ustawienia_.save_settings();
            Ustawienia.SaveSettingsToFile(ustawienia_);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //ok settings
            ReadUstawieniaForm();
        }
        #endregion


        #region funkcje ustawien
        void PrintUstawienieFrom()
        {
            TextBoxUst1.Text = Ustawienia.ilosc_rozdan.ToString();
            TextBoxUst2.Text = Ustawienia.tytul;
            TextBoxUst3.Text = Ustawienia.wyniki;
            TextBoxUst4.Text = Ustawienia.nazwiska;
            TextBoxUst5.Text = Ustawienia.rozdanie;
            TextBoxUst6.Text = Ustawienia.rozklad;
            TextBoxUst7.Text = Ustawienia.licytacja;
            TextBoxUst8.Text = Ustawienia.komentarze;
            TextBoxUst9.Text = Ustawienia.wisty;
            TextBoxUst10.Text = Ustawienia.koniec;
            TextBoxUst11.Text = Ustawienia.linoc.ToString();
        }

        void ReadUstawieniaForm()
        {
            try
            {
                Ustawienia.ilosc_rozdan = int.Parse(TextBoxUst1.Text);

                Ustawienia.tytul = TextBoxUst2.Text;
                Ustawienia.wyniki = TextBoxUst3.Text;
                Ustawienia.nazwiska = TextBoxUst4.Text;
                Ustawienia.rozdanie = TextBoxUst5.Text;
                Ustawienia.rozklad = TextBoxUst6.Text;
                Ustawienia.licytacja = TextBoxUst7.Text;
                Ustawienia.komentarze = TextBoxUst8.Text;
                Ustawienia.wisty = TextBoxUst9.Text;
                Ustawienia.koniec = TextBoxUst10.Text;
                if (TextBoxUst11.Text == "true")
                    Ustawienia.linoc = true;
                else
                    Ustawienia.linoc = false;
            }

            catch
            {
                MessageBox.Show("Blad formatu");
            }
        }

        #endregion

        VugraphLin NowyTrening()
        {
            Microsoft.Win32.OpenFileDialog lin = new Microsoft.Win32.OpenFileDialog();

            lin.Title = "Zaladuj lina z vugrapha";
            lin.ShowDialog();
            VugraphLin vugraph_ = new VugraphLin();


            FileStream plik1 = File.OpenRead(lin.FileName);
            StreamReader reader = new StreamReader(plik1);

            string calosc = "";
            
            while (!reader.EndOfStream)
            {
                calosc += reader.ReadLine();
            }
            reader.Close();
            plik1.Close();
            vugraph_.ReadLinVugraph(calosc);

            return vugraph_;

        }

        BBOGameReader NowyTrening2()
        {
            Microsoft.Win32.OpenFileDialog lin = new Microsoft.Win32.OpenFileDialog();

            BBOGameReader game_ = new BBOGameReader();
           
            lin.Title = "Zaladuj plik lin z Twojej gry";
            lin.ShowDialog();

            FileStream plik1 = File.OpenRead(lin.FileName);
            StreamReader reader = new StreamReader(plik1);
            plik1 = File.OpenRead(lin.FileName);
            reader = new StreamReader(plik1);
            game_.data = File.GetCreationTime(plik1.Name);
            string calosc = "";
            while (!reader.EndOfStream)
            {
                calosc += reader.ReadLine();
            }
            game_.ReadLinBBO(calosc);
            reader.Close();
            plik1.Close();

            return game_;
        }

        void PrintTrening()
        {
            PrinterVu print = new PrinterVu(vugraph);
            print.CreateTreningDOC();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            bool vu = false;
            bool ga = false;

            if (checkDF.IsChecked == true)
                Ustawienia.deepfin = true;

            try
            {
                vugraph = NowyTrening();
                vu = true;
            }
            catch
            {
                MessageBox.Show("cos nie tak z plikiem z vugraphu");
            }

            try
            {
                game = NowyTrening2();
                
                ga = true;
            }

             catch
            {
                MessageBox.Show("cos nie tak z plikiem treningowym");
            }

            if (ga && vu)
            {
                Printer print = new Printer(vugraph, game);
                Document document = print.CreateTreningDOC();

                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = vugraph.tytul;
                dialog.Filter = "doc|.doc";
                dialog.ShowDialog();



                string nazwa_pliku = dialog.FileName;


                RtfDocumentRenderer renderer = new RtfDocumentRenderer();
                renderer.Render(document, nazwa_pliku, null);
                Process.Start(nazwa_pliku);
        
            }
        }

        private void buttonprinttest_Click(object sender, RoutedEventArgs e)
        {
            bool vu = false;
           try
            {
                vugraph = NowyTrening();
                vu = true;
            }
            catch
           {
                MessageBox.Show("cos zle z plikiem z vugraphu");
            }
//
            if (vu)
            {
                PrinterVu print = new PrinterVu(vugraph);
                Document document = print.CreateTreningDOC();

                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = vugraph.tytul;
                dialog.Filter = "doc|.doc";
                dialog.ShowDialog();


                string nazwa_pliku = dialog.FileName;

               
                RtfDocumentRenderer renderer = new RtfDocumentRenderer();
                renderer.Render(document, nazwa_pliku, null);
                Process.Start(nazwa_pliku);
             
            }
        }

        private void buttonTesty_Click(object sender, RoutedEventArgs e)
        {
            panelTest.Visibility = Visibility.Visible;
            panelUstawienia.Visibility = Visibility.Hidden;
        }

        private void buttonprinttest_Copy_Click(object sender, RoutedEventArgs e)
        {

            gra = false;
            game =  NowyTrening2();

           
            
            Treprinter tmp = new Treprinter(game);
            tmp.CreateTreningDOC();

        }
        //dwa mecze
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Ustawienia.deepfin = true;
            game = NowyTrening2();
            
            PrinterMEJ print = new PrinterMEJ(game);
            Document document = print.CreateTreningDOC();
            string nazwa_pliku = "lalal";


            RtfDocumentRenderer renderer = new RtfDocumentRenderer();
            renderer.Render(document, nazwa_pliku, null);
            Process.Start(nazwa_pliku);
        }

      
    }
}
