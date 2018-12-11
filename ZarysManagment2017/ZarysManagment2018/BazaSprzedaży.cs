// Decompiled with JetBrains decompiler
// Type: Zarys.BazaSprzedazy
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ZarysManagment2018
{
    public class BazaSprzedazy
    {
        public List<Sprzedaz> Baza;

        public BazaSprzedazy(string xmlfilename)
        {
            Baza = this.LoadBazaXML(xmlfilename);
        }

        public BazaSprzedazy(List<Sprzedaz> lista)
        {
            Baza = lista;
            SaveXML("BazaSprzedazyNew.xml");
        }

        public void SaveXML(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Sprzedaz>));
            StreamWriter streamWriter1 = new StreamWriter("bazy\\" + filename);
            StreamWriter streamWriter2 = new StreamWriter("bazy\\BazaSprzedazy" + DateTime.Now.ToFileTime().ToString());
            xmlSerializer.Serialize(streamWriter1, Baza);
            xmlSerializer.Serialize(streamWriter2, Baza);
            streamWriter1.Close();
            streamWriter2.Close();
        }

        private List<Sprzedaz> LoadBazaXML(string xmlfilname)
        {
            StreamReader streamReader1 = new StreamReader(xmlfilname);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Sprzedaz>));
            List<Sprzedaz> sprzedazList1 = new List<Sprzedaz>();
            StreamReader streamReader2 = streamReader1;
            List<Sprzedaz> sprzedazList2 = (List<Sprzedaz>)xmlSerializer.Deserialize(streamReader2);
            streamReader1.Close();
            return sprzedazList2;
        }
    }
}
