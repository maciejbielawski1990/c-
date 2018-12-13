//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Bridge
{
    public class RozkladKart
    {
        public Karty N;
        public Karty S;
        public Karty W;
        public Karty E;

        public positions dealer;
        
        /// <summary>
        /// Wypelnia dealera oraz inicjuje karty NSWE wywolujac konstruktor klasy Karty.
        /// </summary>
        /// <param name="input">Linia wejscia to "4SA7HJ986DT5CKT972,S92H743DQ743CA843,SJ854HQ2DAKJ92CQ5,SKQT63HAKT5D86CJ6" w kolejności SWNE</param>
        public RozkladKart(string input)
        {
            dealer = (positions)(int.Parse(input[0].ToString()));
            string[] rozklad = input.Substring(1).Split(',');

            N = new Karty(rozklad[2]);
            S = new Karty(rozklad[0]);
            W = new Karty(rozklad[1]);
            E = new Karty(rozklad[3]);
        }
        
    }
}
