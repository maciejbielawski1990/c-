using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge24_2._0
{
    public static class InfoBridge
    {
        public static int[] tabela_imp = { 19, 40, 80, 120, 160, 210, 260, 310, 360, 420, 490, 590, 740, 890, 1090, 1290, 1490, 1740, 1990, 2240, 2490, 2990, 3490, 3990, 100000 };
        public static int przedkoncowka = 300;
        public static int pokoncowka = 500;
        public static int przedszlemik = 500;
        public static int poszlemik = 750;
        public static int przedszlem = 1000;
        public static int poszlem = 1500;
        public static int tmp_score = 0;
        public static int[] NS_partia = { 1000, 0,0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0 };
        public static int[] WE_partia = { 1000, 1,0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1 };
      //  public static int[] NS_partia = { 1000, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0 };
       // public static int[] WE_partia = { 1000, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1 };

        public static int oblicz_zapis(InfoBoard node)
        {
            int wynik = 0;
            int podstawa = 0;
            bool partia;


            // ustalam zalozenia
            if (node.declarer == "N" || node.declarer == "S")
                node.NS = true;
            else
                node.WE = true;

            if (node.NS)
                partia = (InfoBridge.NS_partia[node.nr] > 0);
            else
                partia = (InfoBridge.WE_partia[node.nr] > 0);
            if (node.realizacja)
            {
                //obliczam podstawę
                string suit = node.suit;
                int level = node.level;
                if (suit == "C" || suit == "D")
                {
                    podstawa = 20 * level;
                }

                if (suit == "H" || suit == "S")
                {
                    podstawa = 30 * level;
                }
                if (suit == "N")
                {
                    podstawa = 30 * level + 10;
                }

                if (node.kontra)
                    podstawa = 2 * podstawa;
                if (node.rekontra)
                    podstawa = 2 * podstawa;



                //sprawdzam premie

                if (podstawa < 100)
                    wynik = podstawa + 50;
                else
                {
                    if (partia)
                        wynik = podstawa + InfoBridge.pokoncowka;
                    else
                        wynik = podstawa + InfoBridge.przedkoncowka;
                }
                //premia za szlemika
                if (level == 6 && partia)
                    wynik += InfoBridge.poszlemik;
                if (level == 6 && (!partia))
                    wynik += InfoBridge.przedszlemik;

                if (level == 7 && partia)
                    wynik += InfoBridge.poszlem;
                if (level == 7 && (!partia))
                    wynik += InfoBridge.przedszlem;

                if (node.kontra)
                    wynik += 50;
                if (node.rekontra)
                    wynik += 100;

                //premie za nadrobki
                if (!node.kontra)
                {
                    if (suit == "C" || suit == "D")
                    {
                        wynik += 20 * node.nadrobek;
                    }
                    else
                    {
                        wynik += 30 * node.nadrobek;
                    }
                }
                else
                {
                    if (partia && (!node.rekontra)) wynik += node.nadrobek * 200;
                    if ((!partia) && (!node.rekontra)) wynik += node.nadrobek * 100;
                    if (partia && (node.rekontra)) wynik += node.nadrobek * 400;
                    if ((!partia) && (node.rekontra)) wynik += node.nadrobek * 200;
                }

                if (node.WE)
                    wynik = -wynik;

            }

            else
            {
                int niedorobki = Math.Abs(node.nadrobek);

                if (partia && node.kontra)
                {
                    wynik = 200 + (niedorobki - 1) * 300;
                    if (node.rekontra) wynik = 2 * wynik;
                }

                if (!partia && node.kontra)
                {
                    wynik = 100 + (niedorobki - 1) * 200;
                    if (niedorobki > 3) wynik += (niedorobki - 3) * 100;
                    if (node.rekontra) wynik = 2 * wynik;
                }

                if (!node.kontra)
                {
                    wynik = 50 * niedorobki;
                    if (partia)
                        wynik = 2 * wynik;
                }

                if (node.NS)
                    wynik = -wynik;
            }

            return wynik;
        }

        public static int oblicz_podstawe_zapisu(InfoBoard node)
        {
            int podstawa_ = 0;
            string suit = node.suit;
            int level = node.level;

            if (suit == "C" || suit == "D")
            {
                podstawa_ = 20 * level;
            }

            if (suit == "H" || suit == "S")
            {
                podstawa_ = 30 * level;
            }
            if (suit == "N")
            {
                podstawa_ = 30 * level + 10;
            }

            if (node.kontra)
                podstawa_ = 2 * podstawa_;
            if (node.rekontra)
                podstawa_ = 2 * podstawa_;

            return podstawa_;
        }

        public static int wylicz_impy(int saldo)
        {
            int imp = 0;
            for (; imp < 24; imp++)
            {
                if (saldo <= InfoBridge.tabela_imp[imp])
                    break;

            }
            return imp;
        }

        public static double wylicz_vp(int imp,int boards)
        {
            double vp;

            double tau = (Math.Sqrt(5) - 1) / 2;
            double B = Math.Sqrt(boards) * 15;

            vp = 10 + 10 * (((1 - Math.Pow(tau, (3 * imp / B)))) / (1 - Math.Pow(tau, 3)));

            vp = 100 * vp;

            int tmpvp = (int)vp;

            vp = ((double)tmpvp) / 100;

            return vp;
        }

        public static int[,,] wylicz_DF(RozkladKart[] rozklad_kart)
        {
            int[,,] lewy = new int[Ustawienia.ilosc_rozdan+2, 4,5];
            
            Console.WriteLine("witamy");
            string YourApplicationPath = "C:\\Users\\Maciek\\Desktop";
            //   string argument = "bcalconsole -c \"T74.73.KQ87.7654 QJ53.AKT542..KQ3 8.Q96.AJT32.AJ92\"";

           
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.WindowStyle = ProcessWindowStyle.Normal;
           processInfo.WorkingDirectory = Path.GetDirectoryName(MainWindow.dir);
            processInfo.FileName = "cmd.exe";
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
         
            Process proc = new Process();
            proc.StartInfo = processInfo;
            proc.Start();
           proc.StandardInput.WriteLine("cd " + MainWindow.dir);

            for (int nr_rozdania = 0; nr_rozdania < Ustawienia.ilosc_rozdan; nr_rozdania++)
            {
                RozkladKart karty = rozklad_kart[nr_rozdania];
                string elem = "\"" + karty.N.piki + "." + karty.N.kiery + "." + karty.N.kara + "." + karty.N.trefle + " ";
                elem += karty.E.piki + "." + karty.E.kiery + "." + karty.E.kara + "." + karty.E.trefle + " ";
                elem += karty.S.piki + "." + karty.S.kiery + "." + karty.S.kara + "." + karty.S.trefle + " ";
                elem += "\"";



                string argument = "bcalconsole -e e -q -c " + elem;
                proc.StandardInput.WriteLine(argument);
                proc.StandardInput.WriteLine("A");

                StreamReader reader = proc.StandardOutput;
                string tekst;
                if (nr_rozdania == 0)
                {
                    for (int i = 0; i < 7; )
                    {
                        tekst = reader.ReadLine();
                        i++;
                    }
                }
                else
                {
                    for (int i = 0; i < 3; )
                    {
                        tekst = reader.ReadLine();
                        i++;
                    }
                }
                
                for (int i = 0; i < 4; i++)
                {
                     tekst = reader.ReadLine();
                    string[] tab = tekst.Split(' ');

                    int wyj;
                    int[] lew = new int[5];
                    int idx = 0;
                    for (int z = 0; z < tab.Count(); z++)
                    {
                        bool result = int.TryParse(tab[z], out wyj);

                        if (result)
                        {
                            lewy[nr_rozdania, i, idx++] = int.Parse(tab[z].ToString());
                        }
                    }
               
                }
               
            }
            return lewy;
        }

        
    }
}
