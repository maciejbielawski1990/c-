using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Management_Projekt.Klasy
{
    public class Zlecenie
    {
        public string idklienta;
        public string nazwa_firmy;
        public string nazwa_sprzetu;
        public string opis;
        public int czas_wykonania;

        public Zlecenie(string id, string name1, string name2, string describe,string czaswykonania)
        {
            idklienta = id;
            nazwa_firmy = name1;
            nazwa_sprzetu = name2;
            opis = describe;
            czas_wykonania=int.Parse(czaswykonania);

        }
    }
}
