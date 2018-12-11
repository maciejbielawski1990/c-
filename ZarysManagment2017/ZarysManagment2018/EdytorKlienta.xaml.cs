using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy EdytorKlienta.xaml
    /// </summary>
    public partial class EdytorKlienta : Window
    {
        public EdytorKlienta()
        {
            InitializeComponent();
        }
        private bool czy_nowy = true;
        private int idx;
        public void Edit(Klient k, int id)
        {
            textNIP.Text = k.NIP;
            textNazwa.Text = k.nazwa_firmy;
            textSkrot.Text = k.skrot;
            textNrtel.Text = k.nr_telefonu;
            textDaneDoFV.Text = k.dane_do_fv;
            textNIP.IsEnabled = false;
            czy_nowy = false;
            idx = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Klient klient = new Klient();
            klient.NIP = textNIP.Text;
            klient.nazwa_firmy = textNazwa.Text;
            klient.skrot = textSkrot.Text;
            klient.nr_telefonu = textNrtel.Text;
            klient.dane_do_fv = textDaneDoFV.Text;
            if (czy_nowy)
                MainWindow.BKlienci.Baza.Add(klient);
            else
                MainWindow.BKlienci.Baza[idx] = klient;
        }
    }
}
