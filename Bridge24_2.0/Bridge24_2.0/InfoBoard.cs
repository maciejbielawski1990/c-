using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge24_2._0
{
    public class InfoBoard
    {
        public int nr;
        public int level;
        public string suit;
        public string declarer;
        public string wist;
        public bool kontra = false;
        public bool rekontra = false;
        public string lew;
        public int nadrobek = 0;
        public bool realizacja = true;
        public bool alert = false;

        public int score;
        public string suitS = "BA";
        public bool partia;
        public bool NS;
        public bool WE;

        public static string[] nicki = new string[100];
        public static string[] nazwiska = new string[100];

    }
}
