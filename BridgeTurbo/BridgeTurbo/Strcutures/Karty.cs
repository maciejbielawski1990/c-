//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Bridge
{
    public class Karty
    {
        public string spades;
        public string hearts;
        public string diamonds;
        public string clubs;

        private string input;


        // Wypelnia jedna reka na podstawie stringa 
        // Wymagania : trzeba podac stringa, ktory zawiera oznaczenia S,H,D,C, przed S(pikami) moze byc cokolwiek, dalej musi byc super

        /// <summary>
        /// Wypelnia jedna reka na podstawie stringa 
        /// </summary>
        /// <param name="input">trzeba podac stringa, ktory zawiera oznaczenia S,H,D,C, przed S(pikami) moze byc cokolwiek, dalej musi byc super</param>
        public Karty(string input_)
        {
            input = input_.ToUpper();

            clubs = Find('C');
            diamonds = Find('D');
            hearts = Find('H');
            spades = Find('S');
        }

        public Karty() { }
        // Uzupelnia zadany kolor na podstawie code
        // Wymagania : nie moze byc juz w inpucie mlodszego koloru niz ten ktory szukamy !
        // Korzysta z calego inputu!

        private string Find(char code)
        {
            int idx = input.IndexOf(code);
            string output = "";
            if (idx >= 0)
            {
                output = input.Substring(idx + 1);
                input = input.Substring(0, idx); // odcina koncowke dlatego zaczynam od trefli
            }
            return output;
        }
    }
}
