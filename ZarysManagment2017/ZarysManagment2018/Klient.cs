using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarysManagment2018
{

    public class Klient
    {
        public string NIP { get; set; }

        public string nazwa_firmy { get; set; }

        public string skrot { get; set; }

        public string nr_telefonu { get; set; }

        public string dane_do_fv { get; set; }

        public string dane_do_wysylki { get; set; }

        public string dane_adresowe { get; set; }

        public override string ToString()
        {
            return this.skrot;
        }

        public Klient(object[] values)
        {
            NIP = values[0].ToString();
            nazwa_firmy = values[1].ToString();
            skrot = values[2].ToString();
            nr_telefonu = values[3].ToString();
            dane_do_fv = values[4].ToString();
        }

        public Klient()
        {
        }
    }

}
