using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ZarysManagment2018
{
    /// <summary>
    /// Logika interakcji dla klasy ZamowienieWindow.xaml
    /// </summary>

    public partial class ZamowienieWindow : Window
    {
        private int nr_edytowanego_zamowienia = -1;

        private string[] terminy_platnosci = new string[6]
        {
            "przedpłata",
            "7 dni",
            "14 dni",
            "21 dni",
            "30 dni",
            "inny"
        };

        private string[] sposoby_platnosci = new string[2]
        {
            "przelew",
            "gotówka"
        };

        private Zamowienie zamowienie;

        public ObservableCollection<string> typ_produktu { get; set; }

        public ZamowienieWindow()
        {
           
            ObservableCollection<string> observableCollection = new ObservableCollection<string>();
            observableCollection.Add("Włókno PA6");
            observableCollection.Add("Szczecina PA6");
            observableCollection.Add("Zyłka PA6");
            observableCollection.Add("Usługa transportowa");
            observableCollection.Add("Szpule");
            typ_produktu = observableCollection;
            InitializeComponent();
            comboTypProduktu.ItemsSource = typ_produktu;
            zamowienie = new Zamowienie();
            OnStart();
            
        }

        public ZamowienieWindow(Zamowienie zam)
        {
            

            ObservableCollection<string> observableCollection = new ObservableCollection<string>();
            observableCollection.Add("Włókno PA6");
            observableCollection.Add("Szczecina PA6");
            observableCollection.Add("Zyłka PA6");
            observableCollection.Add("Usługa transportowa");
            observableCollection.Add("Szpule");
            typ_produktu = observableCollection;
            InitializeComponent();
            zamowienie = zam;
            
            nr_edytowanego_zamowienia = zamowienie.idx;
            comboTypProduktu.ItemsSource = typ_produktu;
            labelDataUtworzenia.Content = zam.czas_utworzenia.ToString();
            labelDataEdycji.Content = zam.czas_ostatniej_edycji.ToString();
            dataGrid1.ItemsSource = zamowienie.towary;
            textblockKlient.Text = zamowienie.nabywca.dane_do_fv;
            datapicker1.SelectedDate = new DateTime?(zam.data_sprzedazy);

            for (int index = 0; index < terminy_platnosci.Count(); ++index)
            {
                if (terminy_platnosci[index] == zam.termin_zaplaty)
                {
                    comboTermin.SelectedIndex = index;
                    break;
                }
            }

            for (int index = 0; index < sposoby_platnosci.Count(); ++index)
            {
                if (sposoby_platnosci[index] == zam.sposob_zaplaty)
                {
                    comboSposobZaplaty.SelectedIndex = index;
                    break;
                }
            }
        }

        private void OnStart()
        {
            zamowienie.towary = new List<Towar>();
            dataGrid1.ItemsSource = zamowienie.towary;
            comboboxSposobSzukania.SelectedIndex = 1;
            datapicker1.SelectedDate = new DateTime?(DateTime.Now);
            zamowienie.czas_utworzenia = DateTime.Now;
            labelDataEdycji.Content = zamowienie.czas_utworzenia.ToString();
            labelDataUtworzenia.Content = zamowienie.czas_utworzenia.ToString();
            listViewFindedClients.ItemsSource = MainWindow.BKlienci.Baza;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Klient k = (Klient) this.listViewFindedClients.SelectedItem;
            if (k != null)
            {
                textblockKlient.Text = k.dane_do_fv;
                listViewSprzedaz.ItemsSource = new ZamowienieWindow.HistoriaTowarow(MainWindow.BSprzedazy.Baza
                 .Where (s => s.klient.skrot == k.skrot).ToList<Sprzedaz>()).lista;
                zamowienie.nabywca = k;
            }
            else
            {
                k = (Klient) null;
                textblockKlient.Text = "";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewFindedClients.ItemsSource = BazaKlientow.PrzeszukajBazeKlientow(MainWindow.BKlienci.Baza,
                textboxSearchKlient.Text, this.comboboxSposobSzukania.SelectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zamowienie.czas_ostatniej_edycji = DateTime.Now;
            zamowienie.data_sprzedazy = datapicker1.SelectedDate.Value;
            MainWindow.BZamowienia.DodajZamowienie(zamowienie, nr_edytowanego_zamowienia);
            Close();
        }

        private void dataGrid1_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            dataGrid1.CurrentColumn = comboTypProduktu;
            comboTypProduktu.DisplayIndex = 0;
        }

        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                double num1 = 0.0;
                foreach (Towar towar in this.zamowienie.towary)
                {
                    double num2 = double.Parse(towar.ilosc.Replace('.', ','));
                    double num3 = double.Parse(towar.cena_jednostkowa.Replace('.', ','));
                    num1 += num2 * num3;
                }

                textboxNetto.Content = num1.ToString("0.00");
                textboxVat.Content = (0.23 * num1).ToString("0.00");
                textboxBrutto.Content = (1.23 * num1).ToString("0.00");
            }
            catch
            {
            }
        }

        private bool CheckValuesInDataGrid(List<Towar> lista)
        {
            foreach (Towar towar in lista)
            {
                try
                {
                    double.Parse(towar.cena_jednostkowa);
                    double.Parse(towar.ilosc);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            zamowienie.data_sprzedazy = datapicker1.SelectedDate.Value;
            zamowienie.czas_ostatniej_edycji = DateTime.Now;
            Nr_fv nrFv = new Nr_fv();
            nrFv.ShowDialog();
            Faktura faktura = new Faktura(zamowienie, nrFv.nr);
            if (CheckValuesInDataGrid(zamowienie.towary))
            {
                MainWindow.BZamowienia.DodajZamowienie(zamowienie, nr_edytowanego_zamowienia);
                faktura.Print();
            }
            else
            {
                MessageBox.Show("Błędna wartość ceny lub ilości");
            }
            
        }

        private void comboboxSposobSzukania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewFindedClients.ItemsSource = BazaKlientow.PrzeszukajBazeKlientow(MainWindow.BKlienci.Baza,
                textboxSearchKlient.Text, comboboxSposobSzukania.SelectedIndex);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zamowienie.sposob_zaplaty = sposoby_platnosci[comboSposobZaplaty.SelectedIndex];
        }

        private void comboTermin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zamowienie.termin_zaplaty =terminy_platnosci[comboTermin.SelectedIndex];
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new EdytorKlienta().Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Klient selectedItem = (Klient)listViewFindedClients.SelectedItem;
            int selectedIndex = listViewFindedClients.SelectedIndex;
            if (selectedItem != null)
            {
                EdytorKlienta edytorKlienta = new EdytorKlienta();
                edytorKlienta.Show();
                edytorKlienta.Edit(selectedItem, selectedIndex);
            }
        }





        private class HistoriaTowarow
        {
            public List<ZamowienieWindow.HistoriaTowarow> lista;

            public string date { get; set; }

            public string nr_fv { get; set; }

            public string nazwa_towaru { get; set; }

            public string cena { get; set; }

            public string ilosc { get; set; }

            public HistoriaTowarow()
            {
            }

            public HistoriaTowarow(List<Sprzedaz> dane)
            {
                lista = new List<ZamowienieWindow.HistoriaTowarow>();
                foreach (Sprzedaz sprzedaz in dane)
                {
                    foreach (Towar towar in sprzedaz.towary)
                        lista.Add(new ZamowienieWindow.HistoriaTowarow()
                        {
                            date = sprzedaz.date,
                            nr_fv = sprzedaz.nr_fv,
                            nazwa_towaru = towar.nazwa_towaru,
                            ilosc = towar.ilosc,
                            cena = towar.cena_jednostkowa
                        });
                }

                lista.Reverse();
            }
        }
    }
}

