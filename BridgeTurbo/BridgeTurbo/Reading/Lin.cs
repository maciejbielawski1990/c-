using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace Bridge.Reading
{
    /// <summary>
    /// \brief Struktura do wygodnej reprezentacji danych z plikow lin, oraz funkcje tworzące tę strukturę. Klasa pobiera tylko te elementy, pozniej trzeba je ustawic w innych, dziedzicznych klasach
    /// Możliwości klasy : 
    /// znajduje wszystkie linie z zadanym przedrostkiem (FindLines), 
    /// znajduje pojedyncza linie z zadanym przedrostkiem (FindLine), 
    /// filtruje fragment linu, az do zadanego znaku, najczesciej '|' (Read), 
    /// usuniecie komentarzy.
    /// 
    /// analiza linii tytułu, rezultatow, numerow rozdan(bn), nazwisk(pn), linii wynikow (mp)  
    /// 
    /// Pobranie(analiza) ilosci lew, wistu, kontraktow(z linii rs), nr rozdan(qx), rozdajecego (z rozkladu md), 
    /// 
    /// Pobranie nr rozdania z linii rozdania, z przedrostka ah
    /// 
    /// Pobranie rozkladu
    /// 
    /// Sprawdzenie litery pokoju (o,c) open/closed
    /// 
    /// Rzutowanie symbolu partii na tą enumeryczna ze stuktury Board
    /// 
    /// Pobranie licytacji(GetBidding)
    /// 
    /// 
    /// Pobranie zalozen(SetVulnerability)
    /// 
    /// Pobranie partii rozgrywajego w celu obliczenia wyników (GetVulnerability)
    /// 
    /// Uzupelnienie(konwersja) zmiennej players w strukturze (SurnamesToBidding), ale ustawic trzeba
    /// 
    /// Utworzenie gotowego obiektu klasy Contract (SetContract)
    /// </summary>
    public class Lin
    {
        /// <summary>
        /// zawiera tresc pliku lin
        /// </summary>
        protected string content;
        public string date;
        /// <summary>
        /// lista rozdan, zawiera wszystkie rozdania z danego lina (z pokoju otwartego!)
        /// </summary>
        public List<Board> boards;
       
        private static char[] delimiterP = { ',' };

        /// <summary>
        /// wczutuje tresc linu do Lin.content
        /// </summary>
        /// <param name="path">sciezka zawierajaca lin</param>
        public Lin(string path)
        {
            StreamReader reader = new StreamReader(path);
            content = reader.ReadToEnd();
        }

        public Lin() { }




        #region analiza linii tytulu
        
       ///<summary>
       /// Na podstawie podanej linii lub w innym przypadku wyszukaniej z lina funkcją findline wypelnia strukture Title skladajaca sie z 
       /// tytulu - pierwsze dwa informacje z linu , i nazwy teamow
       /// Linia musi byc dobra tzn seperator przecinki, i minimum odpowiednia ilosc elementow w linii (7)
       /// <param name="titleline">Nalezy poodac linie wg wzorca jak w linie lub podac, wtedy sam poszuka tej linii w pliku.
       /// Element 0 i 1 stanowia tytul, a 5 i 7 nazwy teamów</param>
       /// </summary>
        protected Title ReadTitleLine(string titleline = null)
        {
            Title title = new Title();
            if (titleline == null)
            {
                titleline = FindLine(content, Settings.title_line);

            }

            string[] input = titleline.Split(',');
            try
            {
                title.title = GetTitle(input);
            }
            catch { }
            input = GetTeams(input);
            title.team1 = input[0];
            title.team2 = input[1];

            return title;
        }

        protected string GetTitle(string[] titlelineTAB = null)
        {
            if (titlelineTAB == null)
            {
                titlelineTAB = (FindLine(content, Settings.title_line)).Split(',');
            }

            return titlelineTAB[0] + " " + titlelineTAB[1];
        }

        protected string[] GetTeams(string[] titlelineTAB)
        {
            if (titlelineTAB == null)
            {
                titlelineTAB = (FindLine(content, Settings.title_line)).Split(',');
            }
            return new string[] { titlelineTAB[5], titlelineTAB[7] };
        }

        #endregion

        #region analiza linii kontraktow rs
        /// <summary> Funkcja tworzy liste kontraktow juz w strukturze naszego programu (contracts) i zwraca tą listę. Jest ona wykorzystywana
        /// tylko do pozniejszego utworzenia struktury Board.
        /// <param name="contractline">Podaje sie taka linie lub odszukuje domyslnie ja w linie </param>
        /// </summary>

        public List<Contract> ConvertContractLine(string contractline = null)
        {
            if (contractline == null)
            {
                contractline = FindLine(content, Settings.result_line);
            }

            List<Contract> contracts = new List<Contract>();

            string[] input = contractline.Split(delimiterP, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in input)
            {
                try
                {
                    contracts.Add(new Contract(s)); // tu mozna wywolac setcontract
                }
                catch { }
            }

            return contracts;
        }

        /// <summary>
        /// Funkcja tworzy kontrakt na podstawie stringu!
        /// </summary>
        /// <param name="contractString">zapis kontraktu wg konwencji linowej</param>
        /// <returns>wypelnioną strukturę kontrakt</returns>
        public static Contract SetContract(string contractString)
        {
            return new Contract(contractString);
        }

        #endregion

        #region analiza linii nazwisk
        /// <summary> Tworzy tablice nickow/nazwisk wystepujacych w pliku lub podanej linii. Wykorzystywana pozniej do zainicjowania w klasie
        /// Board skladowej players </summary>

        public string[] GetNicks(string nicksline = null)
        {
            if (nicksline == null)
            {
                nicksline = FindLine(content, Settings.surnames_line, 3, '|');
            }

            string[] nicks = nicksline.Split(delimiterP, StringSplitOptions.RemoveEmptyEntries);
      
            return nicks;
        }

        public string[] SetSurnames(string[] nicks)
        {
            for (int i = 0; i < nicks.Count(); i++)
            {
                nicks[i] = SetSurname(nicks[i]);
            }

            return nicks;
        }

        public string SetSurname(string nick)
        {
            nick = nick.ToUpper();
          var it =  Settings.BazaNazwisk.Find(item => item.nick == nick);

          if (it != null)
              return it.nazwisko;
          else
              return nick;
            
        }

        #endregion

        #region analiza linii wynikow
        ///<summary>
        /// Tworzy tablice przechowujaca wyniki z rozdania w procentach. Glownie do gier turniejowych.
        /// </summary>
        // Zwraca tablice wynikow z bbo na podstawie linii
        public double[] GetPercentage(string mpline = null)
        {
            if (mpline == null)
            {
                mpline = FindLine(content, Settings.score_line);
                mpline = mpline.Remove(mpline.Count() - 1).Substring(3);
            }

            string[] input = mpline.Split(delimiterP, StringSplitOptions.RemoveEmptyEntries);
            //   string[] input = mpline.Split(delimiterP, StringSplitOptions.RemoveEmptyEntries);
            double[] output = new double[input.Count()];
            int idx = 0;

            foreach (string s in input)
            {
                string t = s.Split('%')[0];
                t = t.Replace('.', ',');

                output[idx++] = double.Parse(t);
            }

            return output;
        }

        #endregion

        #region analiza linii numerow bn

        public int[] GetNumbersFromBNLines(string bnline = null)
        {
            if (bnline == null)
            {
                bnline = FindLine(content, Settings.numbers_line, 3, '|');
                //  bnline.Remove(bnline.Count() - 1).Substring(3);
            }

            string[] outputS = bnline.Split(',');
            int[] output = new int[outputS.Count()];
            int idx = 0;
            foreach (string s in outputS)
            {
                output[idx++] = int.Parse(s);
            }

            return output;
        }

        #endregion

        #region analiza linii z rozdaniem qx
        ///<summary>
        ///Zwraca rozklad na podstawie linii rozdania oraz rozgrywajacego
        /// <param name="boardline"> linia z rozdaniem, minimalne wejscie to md|....</param>
        /// </summary> 
        public RozkladKart GetRozklad(string boardline, int delay = 3)
        {
            string rozklad = FindLine(boardline, Settings.rozklad, 3, '|');

            return new RozkladKart(rozklad);
        }

        public positions GetDealer(string boardline, int delay = 3)
        {
            string rozklad = FindLine(boardline, Settings.rozklad, 3, '|');
            int z = int.Parse(rozklad[0].ToString());
            positions pos = (positions)z;

            return pos;
        }

        public int GetNumberFromAh(string boardline, int delay = 3)
        {
            string input = FindLine(boardline, Settings.numberAH_pre, 3, '|');

            int output = int.Parse(input.Substring(5));

            return output;
        }
        /// <summary>
        /// Pobiera zalozenia rozdania na podstawie linii rozdania
        /// </summary>
        public vulnerabilties SetVulnerability(string rozkladline, int delay = 3)
        {
            string input = FindLine(rozkladline, Settings.vulnerab, 3, '|');
            //  string w = Read(rozkladline, idx + delay);

            return rzutujpartie(input);

        }

        protected static vulnerabilties rzutujpartie(string input)
        {
            input = input.ToUpper();
            vulnerabilties output = vulnerabilties.none;

            if (input == "N") return vulnerabilties.ns;
            if (input == "E") return vulnerabilties.we;
            if (input == "B") return vulnerabilties.both;

            return output;

        }
        /// <summary>
        /// Tworzy licytacje rozdania
        /// </summary>
        /// <param name="input">linia z rozdaniem</param>
        /// <returns>listę z licytacją</returns>
        public List<Bidding> GetBidding(string input)
        {
            input = input.Replace("\r", "");
            input = input.Replace("\n", "");

            int idx_koniec = input.IndexOf("pg");
            int idx_start = input.IndexOf(Settings.vulnerab);
            string licyt = input.Substring(idx_start + 4, (idx_koniec - idx_start));
            string[] separ = { "|mb|" };
            string[] bid = licyt.Split(separ, StringSplitOptions.RemoveEmptyEntries);

            Bidding licytacja = new Bidding();
            List<Bidding> bidding = new List<Bidding>();
            bid[bid.Count() - 1] = "p";
            for (int i = 0; i < bid.Count(); i++)
            {
                Bidding odzywka = new Bidding();

                if (bid[i].Contains("an"))
                {
                    odzywka.alert = true;
                    odzywka.odzywka = Read(bid[i]);
                    int tmp = bid[i].IndexOf("an");
                    odzywka.wyjasnienie = bid[i].Substring(tmp + 3);
                }
                else
                {
                    odzywka.alert = false;
                    odzywka.odzywka = bid[i];
                }


                bidding.Add(odzywka);

            }

            return bidding;
        }

        public static int GetNumberQx(string rozkladline, int delay = 4)
        {
            //  string nrS = rozkladline[delay].ToString();
            //  int nr = int.Parse(nrS);

            string s = Read(rozkladline, 4);
            return int.Parse(s);
        }

        protected static bool IfOpenRoom(string boardline, int delay = 3)
        {
            bool open = true;
            if (boardline[delay].ToString().ToUpper() == "C")
                open = false;

            return open;
        }

        public string GetLead(string boardline, int delay = 3)
        {
            return FindLine(boardline, Settings.lead_pre, 3, '|');
        }

        public static int GetCountOfTricks(string rozkladline, int delay = 3)
        {
            int idx = rozkladline.IndexOf(Settings.tricks_pre);

            return int.Parse(Read(rozkladline, idx + delay));
        }



        #endregion



        /// <summary>
        /// Słuzy do ustalenia czy rozgrywajacy byl po partii. Odbywa się to na podstawie nr rozdania
        /// </summary>
        /// <param name="nr">nr rozdania (do zalozen)</param>
        /// <param name="declarer"> rozgrywający </param>
        /// <returns></returns>
        public static bool GetVulnerability(int nr, positions declarer)
        {
            if (declarer == positions.N || declarer == positions.S)
                if (BridgeInfo.NS_partia[nr % 16] > 0) return true;
                else
                    return false;
            else
                if (BridgeInfo.WE_partia[nr % 16] > 0) return true;
                else
                    return false;
        }

        protected string[] SurnamesToBidding(string[] surnames, int nr_stolu = 1)
        {
            string[] output = new string[5];
            int delay = (nr_stolu - 1) * 4;
            output[(int)positions.N] = surnames[2 + delay];
            output[(int)positions.S] = surnames[0 + delay];
            output[(int)positions.W] = surnames[1 + delay];
            output[(int)positions.E] = surnames[3 + delay];

            return output;
        }


        // Wyszukuje w argumencie input kodu code i zwraca caly tekst z przesunieciem startu o delay az do znaku delim (koniec linii
        //szuka 1 danej linii
        protected string FindLine(string input, string code, int delay = 3, char delim = '\n')
        {
            int idx = input.IndexOf(code);
            string output = null;
            if (idx >= 0)
            {
                idx += delay;

                while (input[idx] != delim)
                {
                    output += input[idx++];
                }
            }
            return output;
        }

        // z wejscia czyta az do napotkania zadanego znaku - sluzy do wyciecia pojedynczych fragmentow
        private static string Read(string input, int pos = 0, char delimiter = '|')
        {
            string output = "";

            while (input[pos] != delimiter)
            {
                output += input[pos++];
            }
            return output;
        }


        // wczytuje wszystkie linie z zadnamy kodem 
        protected virtual List<string> FindLines(string code, char delim = '\n')
        {
            int idx = 0;

            List<string> output = new List<string>();

            while ((idx = content.IndexOf(code, idx)) >= 0)
            {
                string t = "";
                while (content[idx] != delim)
                {
                    t += content[idx++];
                }
                output.Add(t);
            }

            return output;
        }



    }
}
