using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Management_Projekt.Klasy
{
    public class Klient
    {
        public string NIP;
        public string adres_firmy;
        public string nazwa_firmy;

        public Klient(string _nip, string _nazwa, string _adres)
        {
            NIP = _nip;
            adres_firmy = _adres;
            nazwa_firmy = _nazwa;
        }
        
    }
}
