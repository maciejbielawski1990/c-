using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Service_Management_Projekt.Klasy;
using System.Data;
using System.Globalization;
namespace Service_Management_Projekt
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MySqlConnection connection;
        public static bool _login;
        public static bool _szczegoly;
        ObslugaBazy ob = new ObslugaBazy();
        
        Algorytm algorytm;
        
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = "server=localhost;User Id=root;password=170490;port=3306;database=teba;charset=utf8";
      //      connection = new MySqlConnection(connectionString);
            _login = false;
            _szczegoly = false;
            algorytm = new Algorytm();
        //    SA s = new SA(45,8);
            
        }

       
        // otwieranie okna logowania
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            String logoff = "Wyloguj się";
            String logon = "Zaloguj się";
            if (button1.Content.ToString().Equals(logoff))
            {
                _login = false;
                button1.Content = logon;
                powitanie.Text = "";
                this.button2.SetResourceReference(StyleProperty, "diamondactive");
                this.button3.SetResourceReference(StyleProperty, "diamond");
                this.button4.SetResourceReference(StyleProperty, "diamond");
                this.button5.SetResourceReference(StyleProperty, "diamond");
                this.button6.SetResourceReference(StyleProperty, "diamond");
                button6.Visibility = Visibility.Hidden;
                button5.Visibility = Visibility.Hidden;
                button4.Visibility = Visibility.Hidden;
                tb1.Visibility = Visibility.Hidden;
                this.dockPanel1.Visibility = Visibility.Visible;
                this.dockPanel2.Visibility = Visibility.Hidden;
                this.dockPanel3.Visibility = Visibility.Hidden;
                this.dockPanel4.Visibility = Visibility.Hidden;
                this.dockPanel5.Visibility = Visibility.Hidden;
            }
            else
            {
                Logowanie w = new Logowanie();
                w.ShowInTaskbar = false;//nie pokazujesz na pasku zadań
                w.ShowDialog();

                if (w.IsOk)
                {
                    _login = true;
                    button1.Content = logoff;
                    powitanie.Text = "Witaj, " + w.getLogin + "!";

                    tb1.Visibility = Visibility.Visible;
                    button6.Visibility = Visibility.Visible;
                    button5.Visibility = Visibility.Visible;
                    button4.Visibility = Visibility.Visible;
                }
                else
                {
                    _login = false;
                }
            }
        }

        //nowe zlecenie
        void nowe_zlecenie()
        {
          
            comboBox1.ItemsSource = ob.wyszukaj_klientow();
            comboBox2.ItemsSource = ob.wyszukuj_sprzet();
        }

        void nowy_sprzet()
        {
            comboBox4.ItemsSource = ob.wyszukaj_klientow();
            comboBox3.ItemsSource = ob.wyszukuj_sprzet();
        }

        //wybor czynnosci w combobox dodawania do bazy
        private void comboBoxAddToDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int opcja = comboBoxAddToDatabase.SelectedIndex;

            switch (opcja)
            {
                case 0:
                    gb1add.Visibility = Visibility.Visible;
                    gb2add.Visibility = Visibility.Hidden;
                    gb3add.Visibility = Visibility.Hidden;
                    gb4add.Visibility = Visibility.Hidden;
                    //dodaj nowe zlecenie
                    nowe_zlecenie();
                    break;
                case 1:
                    gb1add.Visibility = Visibility.Hidden;
                    gb2add.Visibility = Visibility.Visible;
                    gb3add.Visibility = Visibility.Hidden;
                    gb4add.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    gb1add.Visibility = Visibility.Hidden;
                    gb2add.Visibility = Visibility.Hidden;
                    gb3add.Visibility = Visibility.Visible;
                    gb4add.Visibility = Visibility.Hidden;
                    nowy_sprzet();
                    break;
                case 3:
                    gb1add.Visibility = Visibility.Hidden;
                    gb2add.Visibility = Visibility.Hidden;
                    gb3add.Visibility = Visibility.Hidden;
                    gb4add.Visibility = Visibility.Visible;
                    break;
                default:
                    gb1add.Visibility = Visibility.Hidden;
                    gb2add.Visibility = Visibility.Hidden;
                    gb3add.Visibility = Visibility.Hidden;
                    gb4add.Visibility = Visibility.Hidden;
                    
                    break;
            }
        }

 
        private void background_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
           this.button2.SetResourceReference(StyleProperty, "diamondactive");
           this.button3.SetResourceReference(StyleProperty, "diamond");
           this.button4.SetResourceReference(StyleProperty, "diamond");
           this.button5.SetResourceReference(StyleProperty, "diamond");
           this.button6.SetResourceReference(StyleProperty, "diamond");
           this.dockPanel1.Visibility = Visibility.Visible;
           this.dockPanel2.Visibility = Visibility.Hidden;
           this.dockPanel3.Visibility = Visibility.Hidden;
           this.dockPanel4.Visibility = Visibility.Hidden;
           this.dockPanel5.Visibility = Visibility.Hidden;
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.button2.SetResourceReference(StyleProperty, "diamond"); 
            this.button3.SetResourceReference(StyleProperty, "diamondactive");
            this.button4.SetResourceReference(StyleProperty, "diamond");
            this.button5.SetResourceReference(StyleProperty, "diamond");
            this.button6.SetResourceReference(StyleProperty, "diamond");
            this.dockPanel1.Visibility = Visibility.Hidden;
            this.dockPanel2.Visibility = Visibility.Visible;
            this.dockPanel3.Visibility = Visibility.Hidden;
            this.dockPanel4.Visibility = Visibility.Hidden;
            this.dockPanel5.Visibility = Visibility.Hidden;
            ObslugaBazy obs_baz = new ObslugaBazy();
            List<String> ls = obs_baz.getLastHarmonogram(_szczegoly);
            
            double cmax = obs_baz.getLastCMax();
            if (ls.Count < 1)
            {
                infoHarmonogram.Content = "Nie posiadamy przeliczonych harmonogramów.";
                pokazSzczegoly.Visibility = Visibility.Hidden;
            }
            else
            {
                tbharmonogram.Text = "";
                String result = ls.ElementAt<String>(ls.Count - 1);
                infoHarmonogram.Content = "Oto ostatni przeliczony harmonogram: (cmax=" + cmax.ToString("f") + ")";
                int i = 0;
                foreach (String x in ls)
                {
                    i++;
                    if (i % 2 == 1) //nieparzyste = mapa
                    {
                        Hyperlink hyperLink = new Hyperlink()
                        {
                            NavigateUri = new Uri(x)
                        };
                        hyperLink.Inlines.Add("Link do mapy w googlemaps");
                        hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
                        tbharmonogram.Inlines.Add(hyperLink);
                        tbharmonogram.Inlines.Add("\n");
                        //tbharmonogram.Inlines.Add(new Hyperlink(new Run(x)));
                        
                    }
                    else // parzysta = opis trasy
                    {
                        tbharmonogram.Inlines.Add(x);
                    }
                }
                

                pokazSzczegoly.Visibility = Visibility.Visible;
            }
            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.button2.SetResourceReference(StyleProperty, "diamond"); 
            this.button3.SetResourceReference(StyleProperty, "diamond");
            this.button4.SetResourceReference(StyleProperty, "diamondactive");
            this.button5.SetResourceReference(StyleProperty, "diamond");
            this.button6.SetResourceReference(StyleProperty, "diamond");
            this.dockPanel1.Visibility = Visibility.Hidden;
            this.dockPanel2.Visibility = Visibility.Hidden;
            this.dockPanel3.Visibility = Visibility.Visible;
            this.dockPanel4.Visibility = Visibility.Hidden;
            this.dockPanel5.Visibility = Visibility.Hidden;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            this.button2.SetResourceReference(StyleProperty, "diamond");
            this.button3.SetResourceReference(StyleProperty, "diamond");
            this.button4.SetResourceReference(StyleProperty, "diamond");
            this.button5.SetResourceReference(StyleProperty, "diamondactive");
            this.button6.SetResourceReference(StyleProperty, "diamond");
            this.dockPanel1.Visibility = Visibility.Hidden;
            this.dockPanel2.Visibility = Visibility.Hidden;
            this.dockPanel3.Visibility = Visibility.Hidden;
            this.dockPanel4.Visibility = Visibility.Visible;
            this.dockPanel5.Visibility = Visibility.Hidden;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            this.button2.SetResourceReference(StyleProperty, "diamond");
            this.button3.SetResourceReference(StyleProperty, "diamond");
            this.button4.SetResourceReference(StyleProperty, "diamond");
            this.button5.SetResourceReference(StyleProperty, "diamond");
            this.button6.SetResourceReference(StyleProperty, "diamondactive");
            this.dockPanel1.Visibility = Visibility.Hidden;
            this.dockPanel2.Visibility = Visibility.Hidden;
            this.dockPanel3.Visibility = Visibility.Hidden;
            this.dockPanel4.Visibility = Visibility.Hidden;
            this.dockPanel5.Visibility = Visibility.Visible;
        }
        //uzupelnienie wybranych elementow w nowym zleceniu
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string query = "SELECT * FROM klienci WHERE NazwaFirmy='";
            query+= comboBox1.SelectedItem.ToString();
            query += "'";
            textBox1.Text = ob.uzupelnij_nip_od_nazwy(query);
            query = "SELECT * FROM sprzet WHERE NIP='" + textBox1.Text + "'";
            comboBox2.ItemsSource = ob.wyszukaj_sprzet_od_danego_klienta(query);
           
        }

        //dodanie nowego zlecenia do bazy danych
        private void button7_Click(object sender, RoutedEventArgs e)
        {
           
            Zlecenie nowe_zlecenie = new Zlecenie(textBox1.Text, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), textBox2.Text,textBox3.Text);
            ob.dodaj_nowe_zlecenie_do_bazy(nowe_zlecenie);
            MessageBox.Show("dodano zlecenie");
            
           

        
        }
        //dodanie nowego klienta
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Klient nowy_klient = new Klient(textBox4.Text, textBox5.Text, textBox6.Text);
            ob.dodaj_nowego_klienta_do_bazy(nowy_klient);
            MessageBox.Show("dodano nowego klienta");
        }
        //dodanie nowego sprzetu
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            Sprzet nowy_sprzet = new Sprzet(comboBox3.SelectedItem.ToString(), comboBox4.SelectedItem.ToString());
            ob.dodaj_nowy_sprzet(nowy_sprzet);

        }

        private void comboBoxDelFromDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO!
            int opcja = comboBoxDelFromDatabase.SelectedIndex;

            switch (opcja)
            {
                case 0:
                    gb1del.Visibility = Visibility.Visible;
                    gb2del.Visibility = Visibility.Hidden;
                    gb3del.Visibility = Visibility.Hidden;
                    gb4del.Visibility = Visibility.Hidden;
                    //dodaj nowe zlecenie
                   // nowe_zlecenie();
                    break;
                case 1:
                    gb1del.Visibility = Visibility.Hidden;
                    gb2del.Visibility = Visibility.Visible;
                    gb3del.Visibility = Visibility.Hidden;
                    gb4del.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    gb1del.Visibility = Visibility.Hidden;
                    gb2del.Visibility = Visibility.Hidden;
                    gb3del.Visibility = Visibility.Visible;
                    gb4del.Visibility = Visibility.Hidden;
                    //nowy_sprzet();
                    break;
                case 3:
                    gb1del.Visibility = Visibility.Hidden;
                    gb2del.Visibility = Visibility.Hidden;
                    gb3del.Visibility = Visibility.Hidden;
                    gb4del.Visibility = Visibility.Visible;
                    break;
                default:
                    gb1del.Visibility = Visibility.Hidden;
                    gb2del.Visibility = Visibility.Hidden;
                    gb3del.Visibility = Visibility.Hidden;
                    gb4del.Visibility = Visibility.Hidden;

                    break;
            }
        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            // TODO! //Usuń zlecenie
        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            // TODO! //Usuń klienta
        }

        private void button19_Click(object sender, RoutedEventArgs e)
        {
            // TODO! //Usuń sprzęt
        }

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            // TODO! //Usuń serwisanta
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            ob.dodaj_nowego_serwisanta_do_bazy(nazwiskoimie.Text);
            MessageBox.Show("dodano nowego serwisanta");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double Tmin = 1;

            double testTmin = double.Parse(TMIN.Text, System.Globalization.CultureInfo.InvariantCulture);
            if (testTmin > 0)
                Tmin = testTmin;
            double testTfrac = double.Parse(TSTEP.Text, System.Globalization.CultureInfo.InvariantCulture);
            double deltaFrac = 1.001;
            if (testTfrac > 1)
                deltaFrac = testTfrac;
            double cmax = algorytm.run(Tmin, deltaFrac);
            algorytm.save(cmax);
        }

        private void pokazSzczegoly_Click(object sender, RoutedEventArgs e)
        {
            ObslugaBazy obs_baz = new ObslugaBazy();
            List<String> ls = obs_baz.getLastHarmonogram(!_szczegoly);
            tbharmonogram.Text = "";
            if (_szczegoly)
            {
                _szczegoly = false;
                pokazSzczegoly.Content = "Pokaż szczegóły";
                String result = ls.ElementAt<String>(ls.Count - 1);
                int i = 0;
                foreach (String x in ls)
                {
                    i++;
                    if (i % 2 == 1) //nieparzyste = mapa
                    {
                        Hyperlink hyperLink = new Hyperlink()
                        {
                            NavigateUri = new Uri(x)
                        };
                        hyperLink.Inlines.Add("Link do mapy w googlemaps");
                        hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
                        tbharmonogram.Inlines.Add(hyperLink);
                        tbharmonogram.Inlines.Add("\n");
                        //tbharmonogram.Inlines.Add(new Hyperlink(new Run(x)));

                    }
                    else // parzysta = opis trasy
                    {
                        tbharmonogram.Inlines.Add(x);
                    }
                }
            }
            else
            {
                _szczegoly = true;
                pokazSzczegoly.Content = "Ukryj szczegóły";
                String result = ls.ElementAt<String>(ls.Count - 1);
                int i = 0;
                foreach (String x in ls)
                {
                    i++;
                    if (i % 2 == 1) //nieparzyste = mapa
                    {
                        Hyperlink hyperLink = new Hyperlink()
                        {
                            NavigateUri = new Uri(x)
                        };
                        hyperLink.Inlines.Add("Link do mapy w googlemaps");
                        hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
                        tbharmonogram.Inlines.Add(hyperLink);
                        tbharmonogram.Inlines.Add("\n");
                        //tbharmonogram.Inlines.Add(new Hyperlink(new Run(x)));

                    }
                    else // parzysta = opis trasy
                    {
                        tbharmonogram.Inlines.Add(x);
                    }
                }
            }
        }

    }
}
