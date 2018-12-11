using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Management_Projekt.Klasy
{
    public class Sprzet
    {
        public string nazwa;
        public string wlasciciel;

        public Sprzet(string nazwa_, string wlasciciel_)
        {
            nazwa = nazwa_;
            wlasciciel = wlasciciel_;
        }
    }
}
