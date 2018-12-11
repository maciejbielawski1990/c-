using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy BazaKlientow.xaml
    /// </summary>
    public partial class BazaKlient : Window
    {
        public BazaKlient()
        {
            InitializeComponent();
        }


        public void Laduj()
        {
            string cmdText = "SELECT * FROM zarys.klienci";
            MySqlConnection connection = new MySqlConnection("server=localhost;user id = root;password=Mb&Ps#89$90");
            connection.Open();

            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(new MySqlCommand(cmdText, connection));
            DataTable dataTable1 = new DataTable();
            mySqlDataAdapter.Fill(dataTable1);
            connection.Close();
            datagrid.ItemsSource = dataTable1.DefaultView;
            datagrid.RowHeight = 30.0;
        }
    }
}
