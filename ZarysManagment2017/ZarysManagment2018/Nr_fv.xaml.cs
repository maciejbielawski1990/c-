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
    /// Logika interakcji dla klasy Nr_fv.xaml
    /// </summary>
    public partial class Nr_fv : Window
    {
        public Nr_fv()
        {
            InitializeComponent();
        }

       
            public int nr;


            private void Button_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    this.nr = int.Parse(this.textBox1.Text);
                    this.Close();
                }
                catch
                {
                   MessageBox.Show("Podano zły numer faktury. Podaj liczbę całkowitą.");
                }
            }
        }
}
