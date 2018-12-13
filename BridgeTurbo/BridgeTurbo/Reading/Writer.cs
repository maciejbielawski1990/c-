
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using MigraDoc.RtfRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bridge.Printing
{
    partial class Printer
    {
        public static Font[] znaki_kart;
        public static char[] symbols;

        protected static string napisKontra = "ktr";
        protected static string napisRe = "rktr";
        protected static string napisPas = "pas";


        public static void Initialize()
        {
            
        }
        /// <summary>
        /// Pisze odzywke licytacyjna, wiec na praktyke moze tez pisac wist
        /// </summary>
        /// <param name="p"></param>
        /// <param name="odzywka">Odzywka, np. 2H, 7N, p,d,r. Moga byc male lub wielkie litery</param>
        /// <returns></returns>
        public static Paragraph WriteOdzywka(Paragraph p, string odzywka)
        {
            odzywka = odzywka.ToUpper();

            if (odzywka.Count() > 1)
            {
                p.AddText(odzywka[0].ToString());

                char suit = odzywka[1];

                WriteSuit(p, suit);
            }
            else
            {
                if (odzywka.ToUpper() == "D")
                {
                    p.AddText(napisKontra);
                }
                if (odzywka.ToUpper() == "R")
                {
                    p.AddText(napisRe);
                }
                if (odzywka.ToUpper() == "P")
                {
                    p.AddText(napisPas);
                }
            }
            return p;
        }
        /// <summary>
        /// Wpisuje kontrakt w podany paragraf. 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static Paragraph WriteContract(Paragraph p, Contract contract)
        {
            if (contract.level > 0)
            {
                p.AddFormattedText(contract.level.ToString());

                WriteSuit(p, contract.suit);

                if (contract.dbl) p.AddFormattedText("x");
                if (contract.rdbl) p.AddFormattedText("x");
            }
            else
            {
                p.AddFormattedText("4 pasy");
            }

            return p;
        }
        /// <summary>
        /// Wstawia sam znaczek brydzowy
        /// </summary>
        /// <param name="p"></param>
        /// <param name="suit"></param>
        /// <returns></returns>
        public static Paragraph WriteSuit(Paragraph p, suits suit)
        {

            if (suit == suits.club) p.AddFormattedText(symbols[1].ToString(), znaki_kart[1]);
            if (suit == suits.diamond) p.AddFormattedText(symbols[2].ToString(), znaki_kart[2]);
            if (suit == suits.heart) p.AddFormattedText(symbols[3].ToString(), znaki_kart[3]);
            if (suit == suits.spade) p.AddFormattedText(symbols[4].ToString(), znaki_kart[4]);
            if (suit == suits.nt) p.AddFormattedText("NT", znaki_kart[4]);

            return p;
        }

        public static Paragraph WriteSuit(Paragraph p, char suit)
        {
            suit = char.ToUpper(suit);
            if (suit == 'C') p.AddFormattedText(symbols[1].ToString(), znaki_kart[1]);
            if (suit == 'D') p.AddFormattedText(symbols[2].ToString(), znaki_kart[2]);
            if (suit == 'H') p.AddFormattedText(symbols[3].ToString(), znaki_kart[3]);
            if (suit == 'S') p.AddFormattedText(symbols[4].ToString(), znaki_kart[4]);
            if (suit == 'N') p.AddFormattedText("NT", znaki_kart[4]);

            return p;
        }
        /// <summary>
        /// Wypisuje wist w zadany paragraf
        /// </summary>
        /// <param name="p"></param>
        /// <param name="lead">wist, 2 znaki, pierwszy to kolor wistu, drugi to numer karty</param>
        /// <returns></returns>
        public static Paragraph WriteLead(Paragraph p, string lead)
        {
            if (lead != null)
            {
                p.AddFormattedText(lead[1].ToString());

                WriteSuit(p, lead[0]);

            }


            return p;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tricks">ilosc nadrobek/niedorobek</param>
        /// <returns></returns>
        public static Paragraph WriteTricks(Paragraph p, int tricks)
        {
            if (tricks > 0)
                p.AddFormattedText("+" + tricks.ToString());
            else
            {
                if (tricks == 0)
                    p.AddFormattedText("=");
                else
                    p.AddFormattedText(tricks.ToString());
            }


            return p;
        }
    }
}
