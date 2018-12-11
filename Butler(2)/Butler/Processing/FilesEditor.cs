using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Butler
{
    class FilesEditor
    {
        public static void SaveButler(string nazwa_zapisu, List<ButlerPlayer> baza)
        {
            StreamWriter plik = new StreamWriter(nazwa_zapisu + ".xml");
            XmlSerializer serialize = new XmlSerializer(typeof(List<ButlerPlayer>));

            serialize.Serialize(plik, baza);
            plik.Close();
        }

        public static List<ButlerPlayer> LoadButlerFromXML(string nazwa_pliku)
        {
            StreamReader reader = new StreamReader(nazwa_pliku);
            XmlSerializer ser = new XmlSerializer(typeof(List<ButlerPlayer>));
            return (List<ButlerPlayer>)ser.Deserialize(reader);
        }


    }
}
