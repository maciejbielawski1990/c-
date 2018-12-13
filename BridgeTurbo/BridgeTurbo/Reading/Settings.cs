using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bridge
{
    class Settings
    {
        public static string title_line = "vg";
        public static string result_line = "rs";
        public static string board_line = "qx";
        public static string surnames_line = "pn";
        public static string numbers_line = "bn";
        public static string score_line = "mp";
        public static string rozklad = "md";
        public static string vulnerab = "sv";
        public static int boards = 15;
        public static string surnames_board = "pn";
        public static string numberAH_pre = "ah";
        public static string tricks_pre = "mc";
        public static string lead_pre = "pc";

        public static string naglowek_logo = "files\\mini_b24.jpg";

        public static List<Nazwiska> BazaNazwisk;

        public class Nazwiska
        {
            public string nick;
            public string nazwisko;

            public Nazwiska(string[] tab)
            {
                nick = tab[0];
                nazwisko = tab[1];
            }
        }

        public static void SettingStart()
        {
            InicjujBazeNazwisk();
        }


        private static void InicjujBazeNazwisk()
        {
            StreamReader reader = new StreamReader("files\\nazwiska.txt",Encoding.Default);
            BazaNazwisk = new List<Nazwiska>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] konwersja = line.Split(';');

                Nazwiska gracz = new Nazwiska(konwersja);
                BazaNazwisk.Add(gracz);
            }

        }
    }
}
