using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;

namespace ZarysManagment2018
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static BazaKlientow BKlienci;
        public static BazaZamowien BZamowienia;
        public static BazaSprzedazy BSprzedazy;
        public static bool adminK;
        private bool newZam = true;

        public MainWindow()
        {
            InitializeComponent();

            MainWindow.BKlienci = new BazaKlientow("bazy\\BazaKlientow.xml");
            MainWindow.BSprzedazy = new BazaSprzedazy("bazy\\BazaSprzedazy131478739417504196");
            MainWindow.BZamowienia = new BazaZamowien("bazy\\BazaZamowien.xml");
            MainWindow.BZamowienia.Baza.Reverse();
            datagridZam.ItemsSource = MainWindow.BZamowienia.Baza;
            ZamowienieWindow w1 = new ZamowienieWindow();
            tabitem2.Content = w1.Content;

            // datagridZam.ItemsSource = MainWindow.BZamowienia.Baza;
        }
       

        private void BaseXMLWindow(object sender, RoutedEventArgs e)
        {
            BazaXML bazaXml = new BazaXML();
            MainWindow.adminK = true;
            bazaXml.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tabitem2.Content = (new ZamowienieWindow()).Content;
            
            tabitem2.IsSelected = true;
          //  new ZamowienieWindow().ShowDialog();
        //    datagridZam.ItemsSource = null;
        //    datagridZam.ItemsSource = MainWindow.BZamowienia.Baza;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MainWindow.BZamowienia.Save("bazy//BazaZamowien.xml");
            MainWindow.BKlienci.SaveXML("bazy\\BazaKlientow.xml");
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Podaj plik ze sprzedazą";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {

                string[] filenames = openFileDialog.FileNames;
                ReaderDocSprzedaz readerDocSprzedaz = new ReaderDocSprzedaz();
                readerDocSprzedaz.Make(filenames);
                BazaSprzedazy bazaSprzedazy = new BazaSprzedazy(readerDocSprzedaz.listaSprzedazy);
            }

            MessageBox.Show("Utworzono bazę sprzedaży");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string xmlfilename = (string) null;
            openFileDialog1.Title = "Podaj bazę sprzedaży do edycji";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == true)
            {
                xmlfilename = openFileDialog1.FileName;
                BazaSprzedazy bazaSprzedazy = new BazaSprzedazy(xmlfilename);
                ReaderDocSprzedaz readerDocSprzedaz = new ReaderDocSprzedaz(bazaSprzedazy.Baza);
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog2.Title = "Podaj plik ze sprzedazą";
                openFileDialog2.Multiselect = true;
                if (openFileDialog1.ShowDialog() == true)
                {

                    string[] filenames = openFileDialog2.FileNames;
                    readerDocSprzedaz.Make(filenames);
                    bazaSprzedazy.SaveXML("editedBazaSprzedazy");
                }
            }
        }

        private void datagridZam_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           //   new ZamowienieWindow((Zamowienie)this.datagridZam.SelectedItem).Show();
           tabitem2.Content = (new ZamowienieWindow((Zamowienie) datagridZam.SelectedItem)).Content;
            newZam = false;
            tabitem2.IsSelected = true;
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            BazaKlient bazaKlient = new BazaKlient();
            bazaKlient.Laduj();
            bazaKlient.Show();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 0)
            {
                datagridZam.ItemsSource = MainWindow.BZamowienia.Baza;
            }

            if (tabcontrol1.SelectedIndex == 1)
            {
                datagridZam.ItemsSource = null;
                //tabitem2.Content = (new ZamowienieWindow()).Content;
                if (newZam)
               {
                   
                   // ZamowienieWindow z = new ZamowienieWindow();

               //     tabitem2.Content = z.Content;
                }
                   // tabitem2.Content = (new ZamowienieWindow()).Content;
            }
        }

 
    }
}
