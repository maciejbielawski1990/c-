using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    class BridgeInfo
    {
        public static int[] tabela_imp = { 19, 40, 80, 120, 160, 210, 260, 310, 360, 420, 490, 590, 740, 890, 1090, 1290, 1490, 1740, 1990, 2240, 2490, 2990, 3490, 3990, 100000 };
        public static int przedkoncowka = 300;
        public static int pokoncowka = 500;
        public static int przedszlemik = 500;
        public static int poszlemik = 750;
        public static int przedszlem = 1000;
        public static int poszlem = 1500;
        public static int[] NS_partia = { 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0 };
        public static int[] WE_partia = { 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1 };

        /// <summary>
        /// Wylicza ilosc impow na podstawie podanej roznicy punktow
        /// </summary>
        /// <param name="saldo">Roznica w zapisie (dodatnie albo ujemne)</param>
        /// <returns>Ilosc impow (dodanie albo ujemne)</returns>
        public static int wylicz_impy(int saldo)
        {
            int imp = 0;
            int abs_saldo = Math.Abs(saldo);
            for (; imp < 24; imp++)
            {
                if (abs_saldo <= tabela_imp[imp])
                    break;

            }

            if (saldo < 0)
                return -imp;
            else
                return imp;

        }

        public static int[] wylicz_mecz(ref List<Board> table1, ref List<Board> table2)
        {
            int impNS = 0;
            int impWE = 0;

            for (int i = 0; i < table2.Count; i++)
            {
                int wynikImp = wylicz_impy(table1[i].score - table2[i].score);
                if (wynikImp > 0)
                    impNS += wynikImp;
                else
                    impWE -= wynikImp;
            }
            int[] output = { impNS, impWE };

            return output;
        }

        /// <summary>
        /// Wylicza Vp wg ułamkowej skali międzynarodowej. Trzeba podac ilosc impów oraz rozdan. 
        /// </summary>
        /// <param name="imp">Roznica impow, dowolna liczba całkowita</param>
        /// <param name="boards">Ilosc rozdan</param>
        /// <returns>Ilosc vp dla zadanej ilosci imp</returns>
        public static double wylicz_vp(int imp, int boards)
        {
            double vp;

            double tau = (Math.Sqrt(5) - 1) / 2;
            double B = Math.Sqrt(boards) * 15;

            vp = 10 + 10 * (((1 - Math.Pow(tau, (3 * Math.Abs(imp) / B)))) / (1 - Math.Pow(tau, 3)));

            vp = 100 * vp;

            int tmpvp = (int)vp;

            vp = ((double)tmpvp) / 100;

            if (vp > 20)
                vp = 20;

            if (imp < 0) 
                vp = 20 - vp;
            return vp;
        }
        /// <summary>
        /// Funkcja wylicza ilosc lew w poszczegolne miana z poszczegolnych rąk.  
        /// </summary>
        /// <param name="karty">Rozklad kart w rozdaniu</param>
        /// <returns>Tablica z iloscia lew [gracz, kolor] </returns>
        public static int[,] wylicz_DF(ref RozkladKart karty)
        {
            int[,] lewy = new int[4, 5];

            Console.WriteLine("witamy");
            //  string YourApplicationPath = "C:\\Users\\Maciek\\Desktop";
            //   string argument = "bcalconsole -c \"T74.73.KQ87.7654 QJ53.AKT542..KQ3 8.Q96.AJT32.AJ92\"";


            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.WindowStyle = ProcessWindowStyle.Normal;
            processInfo.WorkingDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            processInfo.FileName = "cmd.exe";
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;

            Process proc = new Process();
            proc.StartInfo = processInfo;
            proc.Start();
            proc.StandardInput.WriteLine("cd " + processInfo.WorkingDirectory);


            string elem = "\"" + karty.N.spades + "." + karty.N.hearts + "." + karty.N.diamonds + "." + karty.N.clubs + " ";
            elem += karty.E.spades + "." + karty.E.hearts + "." + karty.E.diamonds + "." + karty.E.clubs + " ";
            elem += karty.S.spades + "." + karty.S.hearts + "." + karty.S.diamonds + "." + karty.S.clubs + " ";
            elem += "\"";



            string argument = "bcalconsole -e e -q -c " + elem;
            proc.StandardInput.WriteLine(argument);
            proc.StandardInput.WriteLine("A");

            StreamReader reader = proc.StandardOutput;

            string tekst="";

            //while (!reader.EndOfStream)
            //{
            //    try
            //    {
            //        tekst+=reader.ReadLine();
            //    }
            //    catch (Exception e)
            //    {
                    
            //    }

            //}


            for (int i = 0; i < 7;)
            {
                tekst = reader.ReadLine();
                i++;
            }

            //for (int i = 0; i < 3;)
            //{
            //    tekst = reader.ReadLine();
            //    i++;
            //}


            for (int i = 0; i < 4; i++)
            {
                tekst = reader.ReadLine();
                if (tekst != "")
                {
                    string[] tab = tekst.Split(' ');

                    int wyj;
                    int[] lew = new int[5];
                    int idx = 0;
                    for (int z = 0; z < tab.Count(); z++)
                    {
                        bool result = int.TryParse(tab[z], out wyj);

                        if (result)
                        {
                            lewy[i, idx++] = int.Parse(tab[z].ToString());
                        }
                    }

                }



            }
            return lewy;
        }


        public static void wyliczMinimkaksTeoretyczny10(int[,] lewy, vulnerabilties vul)
        {
            int maxNS = 0;
            Contract maxNSContract = new Contract();
            int[] tmpgracz={3,1,4,2};
            
            // minimaks dodatni dla NS
            for (int gracz = 0; gracz < 2; gracz++)
            {
                for (int i = 4; i >= 0; i--)
                {
                    Contract c = new Contract();
                    bool partia = false;
                    if ((vul == vulnerabilties.ns) || (vul == vulnerabilties.both))
                        partia = true;
                    c.level = lewy[gracz, i] - 6;
                    c.suit = (suits)(i + 1);
                    c.declarer = (positions)(tmpgracz[gracz]);
                    int score = c.CalculateScore(partia);
                    if (score > maxNS)
                    {
                        maxNS = score;
                        maxNSContract = c;
                    }
                }
            }

            // minimaks dodatni dla EW
            int maxEW = 0;
            Contract maxEWContract = new Contract();
            for (int gracz = 2; gracz < 4; gracz++)
            {
                for (int i = 4; i >= 0; i--)
                {
                    Contract c = new Contract();
                    bool partia = false;
                    if ((vul == vulnerabilties.we) || (vul == vulnerabilties.both))
                        partia = true;
                    c.level = lewy[gracz, i] - 6;
                    c.suit = (suits)(i + 1);
                    c.declarer = (positions)(tmpgracz[gracz]);
                    int score = c.CalculateScore(partia);
                    if (score > maxEW)
                    {
                        maxEW = score;
                        maxEWContract = c;
                    }
                }
            }
            bool atakNS;
            int obronaScore = -maxNS;
            suits Superobrona = suits.diamond;
            if (maxNSContract > maxEWContract)
            {
                atakNS = true;
                Contract obrona = maxNSContract;
                bool partia = false;
                if ((vul == vulnerabilties.we) || (vul == vulnerabilties.both))
                    partia = true;
                // sprawdzam czy na EW jest obrona
                for (int i = 0; i < 5; i++)
                {
                    obrona++;
                    obrona.declarer = positions.W;
                    obrona.tricks = -obrona.level - 6 + lewy[3, ((int)obrona.suit - 1)];
                    obrona.dbl = true;
                    int s = obrona.CalculateScore(partia);
                    if (s > obronaScore)
                    {
                        Superobrona = obrona.suit; 
                        obronaScore = s;
                    }
                    obrona.declarer = positions.E;
                    obrona.tricks = -obrona.level - 6 + lewy[2, ((int)obrona.suit - 1)];
                    obrona.dbl = true;
                    s = obrona.CalculateScore(partia);
                    if (s > obronaScore)
                    {
                        obronaScore = s;
                        Superobrona = obrona.suit;
                    }
                }
            }

            else
                atakNS = false;


        }

        public static Contract wyliczMinimaksTeoretyczny(int[,] lewy, vulnerabilties vul)
        {
            bool partiaNS = false, partiaEW = false;
            int[] tmpgracz = { 3, 1, 4, 2 };
            Contract maxNSContract = new Contract();
            Contract maxEWContract = new Contract();

            Contract result;

            if ((vul == vulnerabilties.ns) || vul == vulnerabilties.both)
                partiaNS = true;
            if ((vul == vulnerabilties.we) || vul == vulnerabilties.both)
                partiaEW = true;

            // szukam ile w ataku moze ugrac NS
            int maxNS = 0;
            for (int gracz = 0; gracz < 2; gracz++)
            {
                for (int i = 0; i < 5; i++)
                {
                    Contract c = new Contract();
                    c.level = lewy[gracz, i] - 6;
                    c.suit = (suits)(i + 1);
                    c.declarer = (positions)(tmpgracz[gracz]);
                    int score = c.CalculateScore(partiaNS);
                    if (score > maxNS)
                    {
                        maxNS = score;
                        maxNSContract = c;
                    }
                }
            }

            // szukam ile w ataku mozna ugrac EW
            int maxEW = 0;
            for (int gracz = 2; gracz < 4; gracz++)
            {
                for (int i = 4; i >= 0; i--)
                {
                    Contract c = new Contract();
                    c.level = lewy[gracz, i] - 6;
                    c.suit = (suits)(i + 1);
                    c.declarer = (positions)(tmpgracz[gracz]);
                    int score = c.CalculateScore(partiaEW);
                    if (score > maxEW)
                    {
                        maxEW = score;
                        maxEWContract = c;
                    }
                }
            }

            //sprawdzam obrony
            Contract obronaTop = null ;

            if (maxNSContract > maxEWContract)
            {
                //obronaTop = maxNSContract;
                maxNS = -maxNS;
                result = maxNSContract;
                //sprawdz czy EW ma obronę
                Contract obrona = maxNSContract;
                for (int i = 0; i < 5; i++)
                {
                    obrona++;
                    obrona.tricks = -obrona.level - 6 + lewy[2,((int)obrona.suit - 1)];
                    obrona.declarer = positions.E;
                    obrona.dbl = true;
                    int score = obrona.CalculateScore(partiaEW);

                    if (score > maxNS)
                    {
                        maxNS = score;
                        obronaTop = new Contract();
                        obronaTop.level = obrona.level;
                        obronaTop.suit = obrona.suit;
                        obronaTop.declarer = obrona.declarer;
                        obronaTop.dbl = true;
                        obronaTop.tricks = obrona.tricks;
                    }

                    obrona.declarer = positions.W;
                    obrona.tricks = -obrona.level - 6 + lewy[3, ((int)obrona.suit - 1)];
                    score = obrona.CalculateScore(partiaEW);

                    if (score > maxNS)
                    {
                        maxNS = score;
                        obronaTop = new Contract();
                        obronaTop.level = obrona.level;
                        obronaTop.suit = obrona.suit;
                        obronaTop.declarer = obrona.declarer;
                        obronaTop.dbl = true;
                        obronaTop.tricks = obrona.tricks;
                    }
                }

                if (obronaTop == null)
                    result = maxNSContract;
            }
            else
            {
               // obronaTop = maxEWContract;
                maxEW = -maxEW;
                //sprawdz czy NS ma obronę
                Contract obrona = maxEWContract;
                result = maxNSContract;
                for (int i = 0; i < 5; i++)
                {
                    obrona++;
                    obrona.tricks = -obrona.level - 6 + lewy[0, ((int)obrona.suit - 1)];
                    obrona.declarer = positions.N;
                    obrona.dbl = true;
                    int score = obrona.CalculateScore(partiaNS);

                    if (score > maxEW)
                    {
                        maxEW = score;
                        obronaTop = new Contract();
                        obronaTop.level = obrona.level;
                        obronaTop.suit = obrona.suit;
                        obronaTop.declarer = obrona.declarer;
                        obronaTop.dbl = true;
                        obronaTop.tricks = obrona.tricks;
                    }

                    obrona.declarer = positions.W;
                    obrona.tricks = -obrona.level - 6 + lewy[1, ((int)obrona.suit - 1)];
                    score = obrona.CalculateScore(partiaNS);

                    if (score > maxEW)
                    {
                        maxEW = score;
                        obronaTop = new Contract();
                        obronaTop.level = obrona.level;
                        obronaTop.suit = obrona.suit;
                        obronaTop.declarer = obrona.declarer;
                        obronaTop.dbl = true;
                        obronaTop.tricks = obrona.tricks;
                    }
                }
                if (obronaTop == null)
                    result = maxEWContract;
                
            }

            return obronaTop;

        }//koniec funkcji

        public static Contract zabawaMAX(int[,] lewy, vulnerabilties vul)
        {

            List<int[]> tabela = new List<int[]>();
            int[] tmpgracz = { 3, 1, 4, 2 };
            for (int i = 0; i < 4; i++)
            {
                int[] tabelaGracz = new int[35];
                for (int l = 0; l < 7; l++)
                {
                    for (int s = 0; s < 5; s++)
                    {
                        // tu wyliczam ile za kontrakt na danym levelu
                        Contract c = new Contract();
                        c.level = l + 1;
                        c.suit = (suits)(s + 1);
                        c.declarer = (positions)(tmpgracz[i]); // tu oddac tabele konwersujaca
                        c.tricks = lewy[i, s] - 6 - c.level;
                        if (c.tricks < 0)
                            c.dbl = true;

                        tabelaGracz[l * 5 + s] = c.CalculateScore(false);
                    }
                }
                tabela.Add(tabelaGracz);
            }

            int[] tabelaNS = new int[35];
            int[] tabelaEW = new int[35];

            // budowa powyzszych tabel
            for (int i = 0; i < 35; i++)
            {

                if (tabela[0][i] > tabela[1][i])
                    tabelaNS[i] = tabela[0][i];
                else
                    tabelaNS[i] = tabela[1][i];

                if (tabela[2][i] > tabela[3][i])
                    tabelaEW[i] = tabela[2][i];
                else
                    tabelaEW[i] = tabela[3][i];

            }

            int max = 0;
            int top = 12345;

            for (int i = 0; i < 35; )
            {

                if (tabelaNS[i] > max)
                {
                    max = tabelaNS[i];
                    top = i;
                }

                if (-tabelaEW[i] < max)
                {
                    max = -tabelaEW[i];
                    top = -i;
                }

                i++;
            }
            
        

            Contract res = new Contract();
            res.level = Math.Abs(top) / 5 + 1;
            int suit = (Math.Abs(top) % 5 + 1);
            res.suit = (suits)suit;
            bool nsplay = (top > 0);

            if (nsplay)
            {
                res.declarer = positions.N;
                if (tabelaNS[Math.Abs(top)] < 0)
                    res.dbl = true;
            }
            else
            {
                res.declarer = positions.S;
                if (tabelaEW[Math.Abs(top)] < 0)
                    res.dbl = true;
            }

            res.score = max;
            return res;
        }

        public static Contract FindOptimalContract(int[,] lewy, vulnerabilties vul, positions dealer)
        {
            DeefFinesseAnalize df = new DeefFinesseAnalize(lewy, vul, dealer);
            df.FindOptimalContract();

            return df.optimalContract;
        }

        private class DeefFinesseAnalize
        {
            public int[,] lewy;
            public Contract optimalContract;
            public int max = 0;
            private vulnerabilties vul;
            private positions dealer;
            private Contract actualContract;
            private int[] tmpgracz = { 0, 1, 3, 0, 2 };


            public DeefFinesseAnalize(int[,] lewy_, vulnerabilties vul_, positions dealer_)
            {
                lewy = lewy_;
                vul = vul_;
                dealer = dealer_;

            }

            public void FindOptimalContract()
            {
                Contract tmpContract = new Contract();
                tmpContract.level = 1;
                tmpContract.suit = suits.club;

                for (int i = 0; i < 35; i++)
                {
                    tmpContract.declarer = dealer;

                    for (int pos = 0; pos < 4; pos++)
                    {
                        int actualScore = CalculateScoreForPlay(tmpContract);
                        if ((tmpContract.declarer == positions.N) || (tmpContract.declarer == positions.S))
                        {
                            if (actualScore > max)
                            {
                                max = actualScore;
                                optimalContract = tmpContract.Clone();
                                optimalContract.score = max;
                            }
                        }
                        else
                        {
                            if (-actualScore < max)
                            {
                                max = -actualScore;
                                optimalContract = tmpContract.Clone();
                                optimalContract.score = max;
                            }
                        }

                        int d = ((int)tmpContract.declarer) % 4 + 1;
                        tmpContract.declarer = (positions)d;
                        tmpContract.dbl = false;
                    }

                    tmpContract++;
               }          
            }

            private int CalculateScoreForPlay(Contract contract)
            {
                int score;
                bool partia = false;

                if (((contract.declarer == positions.N) || contract.declarer == positions.S) && ((vul == vulnerabilties.both) || (vul == vulnerabilties.ns)))
                    partia = true;
                if (((contract.declarer == positions.W) || contract.declarer == positions.E) && ((vul == vulnerabilties.both) || (vul == vulnerabilties.we)))
                    partia = true;

                int idxGracz = tmpgracz[(int)contract.declarer];
                int idxSuit = (int)contract.suit - 1;
                contract.tricks = lewy[idxGracz, idxSuit] - 6 - contract.level;
                if (contract.tricks < 0)
                    contract.dbl = true;
                score = contract.CalculateScore(partia);

                return score;
            }
               
        } 

       


    }
}
