using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    /// <summary>
    /// \brief Struktura dla kontraktu
    /// 
    /// </summary>
    public class Contract
    {
        /// wysokosc kontraktu
        public int level;
        /// miano kontraktu (C,D,H,S,N)
        public suits suit;
        /// ilosc lew
        public int tricks;
        
        ///  rozgrywajacy (NESW)
        public positions declarer { get; set; } 

        /// czy była kontra
        public bool dbl = false;
        /// czy byla rekontra
        public bool rdbl = false;
    
        /// Zapis kontraktu w postaci stringu
        public string contract_str { get; set; }

        public int score;

        /// <summary>
        /// Wypelnia strukturę kontrakt na podstawie stringu.
        /// </summary>
        /// <param name="input">Kontrakt w formie tekstowej np. 2HS=, 2HSx-1</param>
        public Contract(string input)
        {
            if (input == "P")
            {
                level = 0;
                contract_str = "pass";
            }
            //jesli kontrakt inny niz 4 pasy
            else
            {
                level = int.Parse(input[0].ToString());
                suit = ConvertSuit(input[1]);
                declarer = ConvertPosition(input[2]);
                contract_str = level.ToString() + suit.ToString().ToUpper()[0];

                // sprawdzenie kontr i rekontr
                int iter = 3;
                if (input[iter] == 'x')
                {
                    dbl = true;
                    contract_str += "x";
                    iter++;
                    if (input[iter] == 'x')
                    {
                        rdbl = true;
                        contract_str += "x";
                        iter++;
                    }
                }

                if (input[iter] == '-')
                {
                    tricks = -int.Parse(input[++iter].ToString());
                }
                else
                {
                    if (input[iter] != '=')
                        tricks = int.Parse(input[++iter].ToString());
                }
            }
        }

        public Contract() { }
        /// <summary>
        /// Oblicza zapis. Obiekt Contract musi byc wypelniony.
        /// </summary>
        /// <returns> Zwraca zapis z rozdania</returns>
        public int CalculateScore(bool partia)
        {
            int score = 0;
            int podstawa = oblicz_podstawe();
            if (tricks >= 0)
            {
                // realizacja
                score = oblicz_realizacje(podstawa, partia);
            }
            else
            {
                // wpadki 
                score = oblicz_wpadki(tricks, partia);
            }

            return score;
        }

        #region obliczenia wynikow

        private int oblicz_podstawe()
        {
            int podstawa_ = 0;


            if (suit == suits.club || suit == suits.diamond)
            {
                podstawa_ = 20 * level;
            }

            if (suit == suits.heart || suit == suits.spade)
            {
                podstawa_ = 30 * level;
            }
            if (suit == suits.nt)
            {
                podstawa_ = 30 * level + 10;
            }

            if (dbl)
                podstawa_ = 2 * podstawa_;
            if (rdbl)
                podstawa_ = 2 * podstawa_;

            return podstawa_;

        }

        private int oblicz_realizacje(int podstawa, bool partia)
        {
            int wynik = 0;


            //sprawdzam premie

            if (podstawa < 100)
                wynik = podstawa + 50;
            else
            {
                if (partia)
                    wynik = podstawa + BridgeInfo.pokoncowka;
                else
                    wynik = podstawa + BridgeInfo.przedkoncowka;
            }
            //premia za szlemika
            if ((level == 6) && (partia))
                wynik += BridgeInfo.poszlemik;
            if (level == 6 && (!partia))
                wynik += BridgeInfo.przedszlemik;

            if (level == 7 && partia)
                wynik += BridgeInfo.poszlem;
            if (level == 7 && (!partia))
                wynik += BridgeInfo.przedszlem;

            if (dbl && (!partia))
                wynik += 50;
            if (dbl && partia)
                wynik += 100;

            //premie za nadrobki
            if (!dbl)
            {
                if (suit == suits.club || suit == suits.diamond)
                {
                    wynik += 20 * tricks;
                }
                else
                {
                    wynik += 30 * tricks;
                }
            }
            else
            {
                if (partia && (dbl)) wynik += tricks * 200;
                if ((!partia) && (dbl)) wynik += tricks * 100;
                if (partia && (rdbl)) wynik += tricks * 400;
                if ((!partia) && (rdbl)) wynik += tricks * 200;
            }
            return wynik;
        }

        private int oblicz_wpadki(int tricks, bool partia)
        {
            int niedorobki = Math.Abs(tricks);
            int wynik = 0;

            if (partia && dbl)
            {
                wynik = 200 + (niedorobki - 1) * 300;
                if (rdbl) wynik = 2 * wynik;
            }

            if (!partia && dbl)
            {
                wynik = 100 + (niedorobki - 1) * 200;
                if (niedorobki > 3) wynik += (niedorobki - 3) * 100;
                if (rdbl) wynik = 2 * wynik;
            }

            if (!dbl)
            {
                wynik = 50 * niedorobki;
                if (partia)
                    wynik = 2 * wynik;
            }

            return -wynik;

        }

        #endregion

        #region konwersje - inicjalizacja zmiennych skladowych Board
        /// <summary>
        /// Ustawia rozgrywajacego. Dokonuyje konwersji z char do position
        /// </summary>
        /// <param name="input">Wielka litera (N,S,W,E) oznaczajaca rozgrywajacego</param>
        /// <returns>Rozgrywajacy</returns>
        public positions ConvertPosition(char input)
        {
            positions output = 0;
            if (input == 'N') output = positions.N;
            if (input == 'S') output = positions.S;
            if (input == 'W') output = positions.W;
            if (input == 'E') output = positions.E;

            return output;
        }
        /// <summary>
        /// Ustawia wartosc zmiennej suit w strukturze Contract. Dokonuje konwersji z char to suits.
        /// </summary>
        /// <param name="input">Znak koloru (C,D,H,S,N). Musi to być wielka litera</param>
        /// <returns> Miano kontraktu</returns>
        public suits ConvertSuit(char input)
        {
            suits output = 0;
            if (input == 'C') output = suits.club;
            if (input == 'D') output = suits.diamond;
            if (input == 'H') output = suits.heart;
            if (input == 'S') output = suits.spade;
            if (input == 'N') output = suits.nt;
       

            return output;
        }
        #endregion

        public static bool operator >(Contract x, Contract y)
        {
            bool result = false;
            if (x.level == y.level)
            {
                if (x.suit > y.suit)
                    result = true;
            }

            else
            {
                if (x.level > y.level)
                    result = true;
            }

            return result;
        }

        public static bool operator <(Contract x, Contract y)
        {
            bool result = true;
            if (x.level == y.level)
            {
                if (x.suit > y.suit)
                    result = false;
            }

            else
            {
                if (x.level > y.level)
                    result = false;
            }

            return result;
        }

        public static Contract operator ++(Contract x)
        {
            if (x.suit == suits.nt)
            {
                x.level = x.level + 1;
                x.suit = suits.club;
            }
            else
            {
                x.suit = x.suit + 1;
            }

            return x;
        }
        public Contract Clone()
        {
            Contract y = new Contract();
            y.level = level;
            y.suit = suit;
            y.dbl = dbl;
            y.rdbl = rdbl;
            y.tricks = tricks;
            y.score = score;
            y.declarer = declarer;
            

            return y;
        }

    }
}
