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
using Microsoft.Win32;
namespace ZarysManagment2018
{
    /// <summary>
    /// Logika interakcji dla klasy BazaXML.xaml
    /// </summary>
    public partial class BazaXML : Window
    {

        private BazaKlientow baza;

        // private bool _contentLoaded;

        public BazaXML()
        {
            InitializeComponent();
            GridNowaBazaXML.Visibility = Visibility.Hidden;
        }

        private void DodajElementy(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Podaj folder z fakturami";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                List<string> list = this.listBox1.Items.OfType<string>().ToList<string>();
                foreach (string fileName in openFileDialog.FileNames)
                    list.Add(fileName);

                list.Sort();
                listBox1.Items.Clear();
                foreach (object newItem in list)
                    listBox1.Items.Add(newItem);

                textBoxProgress.Text = "Czas wykonania ok. " +
                                       ((double)this.listBox1.Items.Count * 1.2 + 5.0).ToString() + " s.";
            }
        }

        private void MakeBazaXML()
        {
            string[] filenames = new string[listBox1.Items.Count];
            int index = this.listBox1.Items.Count - 1;
            int num = 0;
            for (; index >= 0; --index)
                filenames[num++] = listBox1.Items[index].ToString();
            baza.AddNewClients(filenames, "BazaKlientow.xml");
        }

        private void Button_EditBazeXML(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Podaj plik z bazą XML do edycji";
            openFileDialog.InitialDirectory =
                "C:\\Users\\Maciek\\Documents\\Visual Studio 2017\\Projects\\Zarys\\ZarysManagment\\ZarysManagment\\bin\\Debug\\bazy";
            listBox1.Items.Clear();
            textBoxProgress.Text = "";

            if (openFileDialog.ShowDialog() == true)
            {
                baza = new BazaKlientow(openFileDialog.FileName);
                GridNowaBazaXML.Visibility = Visibility.Visible;
            }

            MessageBox.Show("Zakończono edycję");
        }

        private void UsunElementy(object sender, RoutedEventArgs e)
        {
            foreach (var removeItem in listBox1.SelectedItems.OfType<string>().ToList<string>())
                listBox1.Items.Remove(removeItem);

            textBoxProgress.Text = "Czas wykonania ok. " + ((double)listBox1.Items.Count * 1.2 + 5.0).ToString() + " s.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridNowaBazaXML.Visibility = Visibility.Visible;
            listBox1.SelectionMode = SelectionMode.Multiple;
            baza = new BazaKlientow();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MakeBazaXML();
            MessageBox.Show("Utworzono nową bazę XML");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Podaj plik z bazą XML do edycji";
            openFileDialog.InitialDirectory = "C:\\Users\\Maciek\\Documents\\Visual Studio 2017\\Projects\\Zarys\\ZarysManagment\\ZarysManagment\\bin\\Debug\\bazy";

            if (openFileDialog.ShowDialog() == true)
            {
                baza = new BazaKlientow(openFileDialog.FileName);
                baza.MakeMySQLBase();
            }

            MessageBox.Show("Zakończono edycję bazy MySQL");
        }
    }
}
