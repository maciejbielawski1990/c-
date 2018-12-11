// Decompiled with JetBrains decompiler
// Type: Zarys.BazaZamowien
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ZarysManagment2018
{
    public class BazaZamowien
    {
        public List<Zamowienie> Baza;

        public BazaZamowien(string xmlfilename)
        {
            Baza = LoadBase(xmlfilename);
        }

        public BazaZamowien()
        {
        }

        public void DodajZamowienie(Zamowienie zam, int nr)
        {
            if (nr < 0)
            {
                zam.idx = Baza.Count;
                Baza.Add(zam);
            }
            else
                Baza[nr] = zam;
        }

        private List<Zamowienie> LoadBase(string xmlfilename)
        {
            StreamReader streamReader1 = new StreamReader(xmlfilename);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Zamowienie>));
            List<Zamowienie> zamowienieList1 = new List<Zamowienie>();
            StreamReader streamReader2 = streamReader1;
            List<Zamowienie> zamowienieList2 = (List<Zamowienie>)xmlSerializer.Deserialize(streamReader2);
            streamReader1.Close();
            return zamowienieList2;
        }

        public void Save(string xmlfilename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Zamowienie>));
            StreamWriter streamWriter1 = new StreamWriter(xmlfilename);
            StreamWriter streamWriter2 = streamWriter1;
            List<Zamowienie> baza = Baza;
            xmlSerializer.Serialize(streamWriter2, baza);
            streamWriter1.Close();
        }
    }
}
