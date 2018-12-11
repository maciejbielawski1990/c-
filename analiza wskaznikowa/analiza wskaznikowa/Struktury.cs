using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analiza_wskaznikowa
{
    class Sprawozdanie
    {
       
       public Pozycja[] dane;
       public string nazwa_spolki;
       public Sprawozdanie()
        {
            dane = new Pozycja[231];
        }
    }

    class Pozycja
    {
        public string name;
        public int pos;
        public double[] value;

        public Pozycja()
        {
            value = new double[5];
            for (int i = 0; i < 5; i++)
                value[i] = -9999999;
        }

        public Pozycja(string n, int p, double val)
        {
            name = n;
            pos = p;
           
        }
    }

    class Wskazniki
    {
        public string name;
        public string category;
        public string wzor;
        public double[] value;

        public Wskazniki()
        {
            value = new double[5];
        }
    }
}
