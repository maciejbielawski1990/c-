using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bridge24_2._0
{
    [Serializable]
    public class Ustawienia
    {
        public static int ilosc_rozdan;
        public static string tytul;
        public static string wyniki;
        public static string nazwiska;
        public static string rozdanie;
        public static string rozklad;
        public static string licytacja;
        public static string komentarze;
        public static string wisty;
        public static string koniec;
        public static bool linoc;
        public static bool deepfin = false;

        public string tytul_, wyniki_, nazwiska_, rozdanie_, rozklad_, licytacja_, komentarze_, wisty_, koniec_, linoc_;
        public int ilosc_rozdan_;
        public  void set_settings()
        {
            tytul = tytul_;
            wyniki = wyniki_;
            nazwiska = nazwiska_;
            rozdanie = rozdanie_;
            rozklad = rozklad_;
            licytacja = licytacja_;
            ilosc_rozdan = ilosc_rozdan_;
            komentarze = komentarze_;
            wisty = wisty_;
            koniec = koniec_;
            if (linoc_ == "True")
            {
                linoc = true;
            }
            else
            {
                linoc = false;
            }
            
            
        }

        public void save_settings()
        {

            tytul_ = tytul;
            wyniki_ = wyniki;
            nazwiska_ = nazwiska;
            rozdanie_ = rozdanie;
            rozklad_ = rozklad;
            licytacja_ = licytacja;
            ilosc_rozdan_ = ilosc_rozdan;
            komentarze_ = komentarze;
            wisty_ = wisty;
            koniec_ = koniec;
           

        }

        public static void LoadSettingsFromFile()
        {
            StreamReader reader = new StreamReader("images\\Settings.xml");
            XmlSerializer ser = new XmlSerializer(typeof(Ustawienia));

           Ustawienia ustawienia_ = new Ustawienia();
           ustawienia_  = (Ustawienia)ser.Deserialize(reader);
           ustawienia_.set_settings();
        }

        public static void SaveSettingsToFile(Ustawienia ustawienia_)
        {
            StreamWriter writer = new StreamWriter("images\\Settings.xml");
            XmlSerializer ser = new XmlSerializer(typeof(Ustawienia));

            ser.Serialize(writer, ustawienia_);
            writer.Close();

        }

        public static void ReadNameBase()
        {
            FileStream file = File.OpenRead("images\\Nazwiska.txt");
            StreamReader reader = new StreamReader(file);

            int i = 0;
            while (!reader.EndOfStream)
            {

                string[] line = reader.ReadLine().Split(';');
                InfoBoard.nicki[i] = line[0].ToString();
                InfoBoard.nazwiska[i] = line[1];
                i++;
            }
            reader.Close();
            file.Close();
        }
    }
}
