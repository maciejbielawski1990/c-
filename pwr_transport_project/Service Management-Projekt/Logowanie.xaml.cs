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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Service_Management_Projekt
{
    /// <summary>
    /// Interaction logic for Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {
        public static string login;
        public static string password;
        public static bool _log_state;
        public Logowanie()
        {
            InitializeComponent();
            _log_state = false;
        }

        public void loguj()
        {
            string login_ = textBox1.Text;
            string password_ = passwordBox1.Password;
            string query = "SELECT * FROM users WHERE login='";
            query += login_ + "'";
            login = login_;
            password = password_;
            MainWindow.connection.Open();

            MySqlCommand cmd = new MySqlCommand(query, MainWindow.connection);

            MySqlDataReader datareader = cmd.ExecuteReader();

            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    if (datareader["haslo"].ToString() == password_)
                    {
                        zalogowano();
                        break;
                    }


                    else
                        MessageBox.Show("Błędne hasło");


                }
            }
            else
                MessageBox.Show("Błędny login");
            MainWindow.connection.Close();
        }

        private void zalogowano()
        {
            //MessageBox.Show("Zalogowano");
            _log_state = true;
            Close();
        }

        public bool IsOk
        {
            get { return _log_state; }
        }

        public string getLogin
        {
            get { return login; }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            loguj();
        }
    }

}
