// Decompiled with JetBrains decompiler
// Type: Zarys.BazaKlientow
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ZarysManagment2018
{
    public class BazaKlientow
    {
        private string version;
        public List<Klient> Baza;

        public BazaKlientow()
        {
            Baza = new List<Klient>();
            if (!MainWindow.adminK)
                ReadMySQLKlienci();
        }

        public BazaKlientow(string XMLfilename)
        {
            Baza = LoadBazaXML(XMLfilename);
        }

        private void ReadMySQLKlienci()
        {
            string cmdText = "SELECT * FROM zarys.klienci";
            MySqlConnection connection = new MySqlConnection("server=localhost;user id = root;password=Mb&Ps#89$90");
            connection.Open();
            MySqlDataReader mySqlDataReader = new MySqlCommand(cmdText, connection).ExecuteReader();
            object[] values = new object[mySqlDataReader.FieldCount];
            while (mySqlDataReader.Read())
            {
                mySqlDataReader.GetValues(values);
                Baza.Add(new Klient(values));
            }
            connection.Close();
        }

        public void MakeMySQLBase()
        {
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;user id = root;password=Mb&Ps#89$90");
            List<string> stringList = new List<string>();
            mySqlConnection.Open();
            string str1 = "INSERT INTO zarys.klienci(NIP,nazwa_firmy,skrot,tel,dane,adres) VALUES(";
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            foreach (Klient klient in this.Baza)
            {
                string str2 = "'" + klient.NIP + "','" + klient.nazwa_firmy + "','" + klient.skrot + "','" + klient.nr_telefonu + "','" + klient.dane_do_fv + "','" + klient.dane_adresowe + "')";
                try
                {
                    mySqlCommand.CommandText = str1 + str2;
                    mySqlCommand.ExecuteNonQuery();
                }
                catch
                {
                    stringList.Add("Nie udało się dodać klienta o nazwie " + klient.skrot + " o NIPIE: " + klient.NIP + " do bazy MySQL");
                }
            }
            StreamWriter streamWriter = new StreamWriter("errors\\KlientMySQLErrors.txt");
            foreach (string str2 in stringList)
                streamWriter.WriteLine(str2);
            streamWriter.Close();
            mySqlConnection.Close();
        }

        public bool AddClientToMySQL(Klient k)
        {
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;user id = root;password=Mb&Ps#89$90");
            List<string> stringList = new List<string>();
            bool flag = true;
            mySqlConnection.Open();
            string str1 = "INSERT INTO zarys.klienci(NIP,nazwa_firmy,skrot,tel,dane,adres) VALUES(";
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            string str2 = "'" + k.NIP + "','" + k.nazwa_firmy + "','" + k.skrot + "','" + k.nr_telefonu + "','" + k.dane_do_fv + "','" + k.dane_adresowe + "')";
            try
            {
                mySqlCommand.CommandText = str1 + str2;
                mySqlCommand.ExecuteNonQuery();
            }
            catch
            {
                flag = false;
            }
            mySqlConnection.Close();
            return flag;
        }

        public bool EditClientInMySQL(Klient k)
        {
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;user id = root;password=Mb&Ps#89$90");
            bool flag = true;
            mySqlConnection.Open();
            string str1 = "UPDATE zarys.klienci SET ";
            MySqlCommand mySqlCommand = new MySqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            string str2 = "nazwa_firmy='" + k.nazwa_firmy + "',skrot='" + k.skrot + "',tel='" + k.nr_telefonu + "',dane='" + k.dane_do_fv + "'WHERE NIP='" + k.NIP + "'";
            try
            {
                mySqlCommand.CommandText = str1 + str2;
                mySqlCommand.ExecuteNonQuery();
            }
            catch
            {
                flag = false;
            }
            mySqlConnection.Close();
            return flag;
        }

        public void AddNewClients(string[] filenames, string XMLfilename = "BazaKlientow.xml")
        {
            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();
            List<DocFV> docFvList = ReaderDocFV.Make(filenames);
            this.version = DateTime.Now.ToFileTime().ToString();
            foreach (DocFV docFv in docFvList)
            {
                try
                {
                    Klient nowy = ClientFromDocFV.KlientParserFromString(docFv.nabywca, docFv.termin_zaplaty, docFv.sposob_zaplaty, docFv.filename);
                    if (this.Baza.Where<Klient>((Func<Klient, bool>)(n => n.NIP == nowy.NIP)).Count<Klient>() == 0)
                    {
                        this.Baza.Add(nowy);
                        stringList2.Add("Dodano do bazy firmę o skrócie " + nowy.skrot + " z pliku " + docFv.filename);
                    }
                    else
                        stringList2.Add("Pominięto firmę o skrócie " + nowy.skrot + ", ponieważ jest w bazie. Badany plik to " + docFv.filename);
                }
                catch
                {
                    stringList1.Add("Błąd parsowania danych do klasy Klient (BazaKlientow.CreateBazaFromDocFiles) z pliku " + docFv.filename);
                }
            }
            StreamWriter streamWriter1 = new StreamWriter("errors\\WordErrors" + this.version + ".txt");
            foreach (string str in stringList1)
                streamWriter1.WriteLine(str);
            streamWriter1.Close();
            StreamWriter streamWriter2 = new StreamWriter("errors\\WordRaport" + this.version + ".txt");
            foreach (string str in stringList2)
                streamWriter2.WriteLine(str);
            streamWriter2.Close();
            this.SaveXML(XMLfilename);
        }

        public void SaveXML(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Klient>));
            StreamWriter streamWriter1 = new StreamWriter(filename);
            xmlSerializer.Serialize((TextWriter)streamWriter1, (object)this.Baza);
            streamWriter1.Close();
            StreamWriter streamWriter2 = new StreamWriter("bazy\\" + ("BazaKlientow_" + this.version + ".xml"));
            xmlSerializer.Serialize((TextWriter)streamWriter2, (object)this.Baza);
            streamWriter2.Close();
        }

        private List<Klient> LoadBazaXML(string xmlfilname)
        {
            StreamReader streamReader1 = new StreamReader(xmlfilname);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Klient>));
            List<Klient> klientList1 = new List<Klient>();
            StreamReader streamReader2 = streamReader1;
            List<Klient> klientList2 = (List<Klient>)xmlSerializer.Deserialize((TextReader)streamReader2);
            streamReader1.Close();
            return klientList2;
        }

        public static List<Klient> PrzeszukajBazeKlientow(List<Klient> klienci, string searchingText, int method)
        {
            List<Klient> klientList = new List<Klient>();
            searchingText = searchingText.ToUpper();
            List<Klient> list;
            switch (method)
            {
                case 0:
                    list = klienci.Where(k => k.NIP.Contains(searchingText)).ToList<Klient>();
                    break;
                case 1:
                    list = klienci.Where(k => k.skrot.ToUpper().Contains(searchingText)).ToList<Klient>();
                    break;
                case 2:
                    list = klienci.Where(k => k.dane_do_fv.ToUpper().Contains(searchingText)).ToList<Klient>();
                    break;
                default:
                    list = klienci.Where(k => k.NIP.Contains(searchingText)).ToList<Klient>();
                    break;
            }
            return list;
        }
    }
}
