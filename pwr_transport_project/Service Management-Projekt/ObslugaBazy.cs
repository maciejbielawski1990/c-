using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using Service_Management_Projekt.Klasy;
using System.IO;
using System.Windows;
using System.Data;
using System.Globalization;

namespace Service_Management_Projekt
{
    public class Harmonogram
    {
        public int idHarmonogram;
        public List<int> idZlecenia;
        public List<int> idPracownika;
        public List<int> kolejnosc;
        public List<string> czas;
        public List<string> nazwaSprzetu;
        public List<string> nazwaUslugi;
        public List<int> idKlienta;
        public List<int> czasWykonania;
        public List<string> nazwaFirmy;
        public List<string> adresFirmy;
        public List<string> pracownik;
        public List<Wspolrzedne> lokalizacja;
        public DataTable dt;
        public Harmonogram()
        {
            dt = new DataTable();
            dt.Columns.Add("idZlecenia");
            dt.Columns.Add("idPracownika");
            dt.Columns.Add("kolejnosc");
            dt.Columns.Add("czas");
            dt.Columns.Add("nazwaSprzetu");
            dt.Columns.Add("nazwaUslugi");
            dt.Columns.Add("idKlienta");
            dt.Columns.Add("czasWykonania");
            dt.Columns.Add("nazwaFirmy");
            dt.Columns.Add("adresFirmy");
            dt.Columns.Add("pracownik");
            dt.Columns.Add("dlugoscGeograficzna");
            dt.Columns.Add("szerokoscGeograficzna");
            

            idZlecenia = new List<int>();
            idPracownika = new List<int>();
            kolejnosc = new List<int>();
            czas = new List<string>();
            nazwaSprzetu = new List<string>();
            nazwaUslugi = new List<string>();
            idKlienta = new List<int>();
            czasWykonania = new List<int>();
            nazwaFirmy = new List<string>();
            adresFirmy = new List<string>();
            pracownik = new List<string>();
            lokalizacja = new List<Wspolrzedne>();
        }
    }
   public class ObslugaBazy
    {
       private string query;
       private MySqlCommand sql;
       private MySqlDataReader datareader;
       string tmp;

        public List<string> wyszukaj_klientow()
        {
            
            query = "SELECT * FROM klienci";
            MainWindow.connection.Open();
            sql = new MySqlCommand(query,MainWindow.connection);
            datareader = sql.ExecuteReader();
            
            List<string> items = new List<string>();
            
            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    items.Add(datareader[1].ToString()); 
                   
                }
            }
            MainWindow.connection.Close();

            return items;

        }

    

        public List<string> wyszukuj_sprzet()
        {
           string query = "SELECT * from sprzet WHERE NIP='0'";
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();

            List<string> items = new List<string>();

            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    items.Add(datareader[1].ToString());
                }
            }
            MainWindow.connection.Close();

            return items;
        }

        public string uzupelnij_nip_od_nazwy(string query)
        {
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            string nip = "" ;

            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    nip = datareader[0].ToString();

                }
            }
            MainWindow.connection.Close();

            return nip;
        }

        public void dodaj_nowe_zlecenie_do_bazy(Zlecenie zlec)
        {
            string query = "INSERT INTO zlecenia (nazwasprzetu, nazwauslugi,idklienta,czaswykonania)" +
                         "  VALUES('" + zlec.nazwa_sprzetu + "','" + zlec.opis + "','" + zlec.idklienta + "','" + zlec.czas_wykonania.ToString() + "')";

            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            sql.ExecuteNonQuery();
            MainWindow.connection.Close();
        }

        public void dodaj_nowego_klienta_do_bazy(Klient klient)
        {
            string query = "INSERT INTO klienci VALUES('" + klient.NIP + "','" + klient.nazwa_firmy + "','" + klient.adres_firmy + "')";
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            try
            {
                sql.ExecuteNonQuery();
            }
            catch (global::System.InvalidCastException e)
            {
                throw new global::System.Data.StrongTypingException("The value for column \'dasdcolgdfgf\' in table \'dasd\' is DBNull.", e);
            }
            MainWindow.connection.Close();
        }
        public void dodaj_nowego_serwisanta_do_bazy(String serwisant)
        {
            string query = "INSERT INTO pracownicy(nazwiskoImie) VALUES('" + serwisant + "')";
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            try
            {
                sql.ExecuteNonQuery();
            }
            catch (global::System.InvalidCastException e)
            {
                throw new global::System.Data.StrongTypingException("The value for column \'dasdcdsdsolgdfgf\' in table \'dsasaasd\' is DBNull.", e);
            }
            MainWindow.connection.Close();
        }

        public void dodaj_nowy_sprzet(Sprzet sprzet)
        {
           
          // string query = "INSERT INTO sprzet (NazwaSprzetu,NIP) VALUES('" +sprzet.nazwa+"','" +
            string querytmp = "SELECT * FROM klienci WHERE NazwaFirmy='" + sprzet.wlasciciel + "'";
            MainWindow.connection.Open();
            sql = new MySqlCommand(querytmp, MainWindow.connection);
            datareader = sql.ExecuteReader();
           // string nip;

            
            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    tmp=datareader[0].ToString();
                }
            }
            
            MainWindow.connection.Close();


            string query = "INSERT INTO sprzet(NazwaSprzetu,NIP) VALUES('" + sprzet.nazwa + "','" + tmp + "')";
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            sql.ExecuteNonQuery();
            MainWindow.connection.Close();

            MessageBox.Show("Dodano sprzęt.");
        }

        public List<string> wyszukaj_sprzet_od_danego_klienta(string query)
        {
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();

            List<string> items = new List<string>();

            if (datareader.HasRows)
            {
                while (datareader.Read())
                {
                    items.Add(datareader[1].ToString());
                }
            }
            MainWindow.connection.Close();

            return items;

            
        }
        public SA wyszukaj_serwis_i_serwisantow()
        {
            MainWindow.connection.Open();
            string query = "SELECT COUNT(*) FROM pracownicy";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            datareader.Read();
            int pracownicy = int.Parse(datareader.GetString(0));
            datareader.Close();
            query = "SELECT COUNT(*) FROM zlecenia";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            datareader.Read();
            int zlecenia = int.Parse(datareader.GetString(0));
            datareader.Close();
            MainWindow.connection.Close();
            SA sa = new SA(zlecenia, pracownicy);
            return sa;
        }
        public Wspolrzedne getWspolrzedne(string miasto)
        {
            Wspolrzedne result = new Wspolrzedne();
            MainWindow.connection.Open();
            string query = "SELECT dlugosc, szerokosc FROM miejscowosci WHERE nazwa='" + miasto + "'";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            if (datareader.FieldCount > 0)
            {
                if (datareader.Read())
                {
                    double x = Convert.ToDouble(datareader[0]);
                    double dlugosc = Math.Floor(x) + (x - Math.Floor(x)) * 100 / 60;
                    double y = Convert.ToDouble(datareader[1]);
                    double szerokosc = Math.Floor(y) + (y - Math.Floor(y)) * 100 / 60;

                    result = new Wspolrzedne(dlugosc, szerokosc);
                }
            }
            datareader.Close();
            MainWindow.connection.Close();
            return result;
        }
        public List<String> getLastHarmonogram(bool pokazSzczegoly)
        {
            List<String> resultList = new List<String>();
            String result = "";
            MainWindow.connection.Open();
            string query = "SELECT MAX(idharmonogram) FROM harmonogram";

            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            int maxid = 0;
            if (datareader.Read() && datareader.HasRows)
            {
                maxid = int.Parse(datareader.GetString(0));
            }
            Harmonogram h = new Harmonogram();
            h.idHarmonogram = maxid;
            
            datareader.Close();
            query = "SELECT * FROM harmonogram WHERE idharmonogram='" + maxid + "'";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            if (datareader.HasRows)
            {
           
                while (datareader.Read())
                {
                    
                    h.idZlecenia.Add(int.Parse(datareader[2].ToString()));
                    h.idPracownika.Add(int.Parse(datareader[3].ToString()));
                    h.kolejnosc.Add(int.Parse(datareader[4].ToString()));
                    h.czas.Add(datareader[5].ToString());
                    
                }
            }
            else
            {
                return resultList;
            }
            datareader.Close();
            foreach (int x in h.idPracownika)
            {
                query = "SELECT * FROM pracownicy WHERE idPracownicy='" + x + "'";
                sql = new MySqlCommand(query, MainWindow.connection);
                datareader = sql.ExecuteReader();
                if (datareader.HasRows)
                {
                    if (datareader.Read())
                    {
                        h.pracownik.Add(datareader[1].ToString());
                    }
                }
                datareader.Close();
            }
            foreach ( int x in h.idZlecenia)
            {
                query = "SELECT * FROM zlecenia WHERE idzlecenia='" + x + "'";
                sql = new MySqlCommand(query, MainWindow.connection);

                datareader = sql.ExecuteReader();
                if (datareader.HasRows)
                {
                    if (datareader.Read())
                    {
                        h.nazwaSprzetu.Add(datareader[1].ToString());
                        h.nazwaUslugi.Add(datareader[2].ToString());
                        h.idKlienta.Add(int.Parse(datareader[3].ToString()));
                        h.czasWykonania.Add(int.Parse(datareader[4].ToString()));
                    }
                }
                datareader.Close();
            }
            foreach ( int x in h.idKlienta)
            {
                query = "SELECT * FROM klienci WHERE NIP='" + x + "'";
                sql = new MySqlCommand(query, MainWindow.connection);

                datareader = sql.ExecuteReader();
                if (datareader.HasRows)
                {
                    if (datareader.Read())
                    {
                        h.nazwaFirmy.Add(datareader[1].ToString());
                        h.adresFirmy.Add(datareader[2].ToString());
                    }
                }
                datareader.Close();
            }
            foreach (String x in h.adresFirmy)
            {
                query = "SELECT szerokosc,dlugosc FROM miejscowosci WHERE nazwa='" + x + "'";
                sql = new MySqlCommand(query, MainWindow.connection);

                datareader = sql.ExecuteReader();
                if (datareader.HasRows)
                {
                    if (datareader.Read())
                    {
                        double x1 = Convert.ToDouble(datareader[0]);
                        double dlugosc = Math.Floor(x1) + (x1 - Math.Floor(x1)) * 100 / 60;
                        double y1 = Convert.ToDouble(datareader[1]);
                        double szerokosc = Math.Floor(y1) + (y1 - Math.Floor(y1)) * 100 / 60;

                        Wspolrzedne lokalizacja = new Wspolrzedne(dlugosc, szerokosc);
                        h.lokalizacja.Add(lokalizacja);
                    }
                }
                datareader.Close();
            }
            for (int i=0; i < h.nazwaFirmy.Count; i++)
            {
                DataRow row = h.dt.NewRow();
                row[0] = h.idZlecenia[i];
                row[1] = h.idPracownika[i];
                row[2] = h.kolejnosc[i];
                row[3] = h.czas[i];
                row[4] = h.nazwaSprzetu[i];
                row[5] = h.nazwaUslugi[i];
                row[6] = h.idKlienta[i];
                row[7] = h.czasWykonania[i];
                row[8] = h.nazwaFirmy[i];
                row[9] = h.adresFirmy[i];
                row[10] = h.pracownik[i];
                row[11] = h.lokalizacja[i].dlugosc;
                row[12] = h.lokalizacja[i].szerokosc;
                h.dt.Rows.Add(row);
            }
            DataRow[] sortedrows = h.dt.Select("", "idPracownika, kolejnosc");
            int tmp = -1;
            int it = 1;
            String res = "";
            double cserwisanta = 0;
            Wspolrzedne lastCity = new Wspolrzedne(19.47, 51.78);
            foreach (DataRow r in sortedrows)
            {
                if (tmp != int.Parse(r[1].ToString()))
                {
                    if (res.Length > 0)
                    {
                        Wspolrzedne newcity = new Wspolrzedne(19.47, 51.78);
                        cserwisanta += lastCity.getDistance(newcity) / 90.0;
                        lastCity = newcity;
                        res += "+to:51.78+N,+19.47+E&ll=51.930718,20.720215&spn=6.057099,19.138184&t=m&z=6";
                        resultList.Add(res);
                        result = "c serwisanta = " + cserwisanta.ToString("f") + "\n" + result;
                        cserwisanta = 0;
                        result += "------------------------\n";
                        resultList.Add(result);
                        result = "";
                        
                    
                        //result += "\nLink do mapy google: ";
                        //result += res;
                        //result += "\n";
                    }
                    res = "https://maps.google.com/maps?saddr=51.78+N,+19.47+E";
                    result += r[10];
                    result += ":\n";
                    tmp = int.Parse(r[1].ToString());
                    it = 1;
                }
                res += "+to:";
                double sz =  double.Parse(r[11].ToString());
                res += sz.ToString("f", CultureInfo.InvariantCulture);
                res += "+N,+";
                double dl = double.Parse(r[12].ToString());
                res += dl.ToString("f", CultureInfo.InvariantCulture);
                res += "+E";
                Wspolrzedne city = new Wspolrzedne(dl, sz);
                cserwisanta += lastCity.getDistance(city) / 90.0;
                lastCity = city;
                cserwisanta += double.Parse(r[7].ToString());
                result += it;
                result += ". Firma ";
                result += r[8];
                result += " z siedzibą w miejscowości ";
                result += r[9];
                if (pokazSzczegoly)
                {
                    result += "\n\tSprzęt: ";
                    result += r[4];
                    result += "\n\tOpis problemu: ";
                    result += r[5];
                    result += "\n\tEstymowany czas: ";
                    result += r[7];
                }
                result += "\n";
                it++;
            }
            MainWindow.connection.Close();
            if (res.Length > 0)
            {
                Wspolrzedne newcity = new Wspolrzedne(19.47, 51.78);
                cserwisanta += lastCity.getDistance(newcity) / 90.0;
                lastCity = newcity;
                res += "+to:51.78+N,+19.47+E&ll=51.930718,20.720215&spn=6.057099,19.138184&t=m&z=6";
                resultList.Add(res);
                result = "c serwisanta = " + cserwisanta.ToString("f") + "\n" + result;
                resultList.Add(result);
            }
            return resultList;
        }
        public double getLastCMax()
        {
            MainWindow.connection.Open();
            double cmax = -1;
            string query = "SELECT MAX(idharmonogram) FROM harmonogram";

            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            int maxid = 0;
            if (datareader.Read() && datareader.HasRows)
            {
                maxid = int.Parse(datareader.GetString(0));
            }
            datareader.Close();
            query = "SELECT cmax FROM harmonogram WHERE idharmonogram='" + maxid + "'";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            if (datareader.HasRows)
            {
                if (datareader.Read())
                {
                    cmax = double.Parse(datareader[0].ToString());
                }
            }
            datareader.Close();
            MainWindow.connection.Close();
            return cmax;
        }
        public void zapiszHarmonogram(int[] harmonogram, double cmax)
        {
            MainWindow.connection.Open();
            
            string query = "SELECT MAX(idharmonogram) FROM harmonogram";
            
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            int maxid = 0;
            if (datareader.Read() && datareader.HasRows)
            {
                maxid = int.Parse(datareader.GetString(0));
            }
            int nextid = maxid + 1;
            datareader.Close();
            List<int> zlecenia = new List<int>();
            List<int> pracownicy = new List<int>();
            query = "SELECT * FROM pracownicy";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            while (datareader.Read())
            {
                pracownicy.Add(int.Parse(datareader[0].ToString()));
            }
            datareader.Close();
            query = "SELECT * FROM zlecenia";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            while (datareader.Read())
            {
                zlecenia.Add(int.Parse(datareader[0].ToString()));
            }
            datareader.Close();
            int IDpracownika = 0;
            int kolejnosc = 0;
            foreach (int x in harmonogram)
            {
                if (x == 0)
                {
                    IDpracownika++;
                    kolejnosc = 0;
                }
                else
                {
                    
                    query = "INSERT INTO harmonogram(idharmonogram, idzlecenia, idpracownika, kolejnosc, cmax) VALUES('" + nextid + "', '" + zlecenia[x-1] + "', '" + pracownicy[IDpracownika] + "', '" + kolejnosc + "', '" + cmax.ToString(CultureInfo.InvariantCulture) + "')";
                    sql = new MySqlCommand(query, MainWindow.connection);
                    sql.ExecuteNonQuery(); 
                    kolejnosc++;
                }
            }
            //string query = "INSERT INTO harmonogram(idharmonogram, idzlecenia, idpracownika, kolejnosc) VALUES('12030')";

            MainWindow.connection.Close();
            
            MessageBox.Show("Dodano harmonogram.");
        }
        public Algorytm wyszukaj_zlecenia()
        {
            List<string> items = new List<string>();
            string query = "SELECT COUNT(*) FROM zlecenia";
            
            MainWindow.connection.Open();
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            datareader.Read();
            Algorytm dane = new Algorytm(int.Parse(datareader.GetString(0)));
            datareader.Close();

            query = "SELECT * FROM zlecenia";
            sql = new MySqlCommand(query, MainWindow.connection);
            datareader = sql.ExecuteReader();
            if (datareader.HasRows)
            {
                int j = 0;
                while (datareader.Read())
                {
                    items.Add(datareader[3].ToString());
                    dane.czas_zlecen[j] = int.Parse(datareader[4].ToString());
                    j++;
                }
            }

            datareader.Close();

            for (int i = 0; i < items.Count; i++)
            {
                query = "SELECT * FROM klienci WHERE NIP='" + items[i] + "'";
                sql = new MySqlCommand(query, MainWindow.connection);
                datareader = sql.ExecuteReader();
                if (datareader.HasRows)
                {
                    while (datareader.Read())
                    {
                        dane.miejsca_zlecen[i] = datareader[2].ToString();
                    }
                }
                datareader.Close();
            }

            MainWindow.connection.Close();

            for (int i = 0; i < items.Count; i++)
            {        
                dane.wspolrzedne_miejsca_zlecen[i] = getWspolrzedne(dane.miejsca_zlecen[i].ToString());   
            }
           
            return dane;

        }

    
    }

    
}
