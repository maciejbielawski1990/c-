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
using MigraDoc.DocumentObjectModel.Shapes;
using Bridge;

namespace BridgeTurbo
{
    partial class Printer
    {
        protected static string napisKontra = "ktr";
        protected static string napisRe = "rktr";
        protected static string napisPas = "pas";

        protected string contractLineTitle = "Kontrakt: "; 

        /// <summary>
        /// Przechowuje dane o czcionce odpowiedniego koloru(treflu,karze,kierze,piku). Indeksy w tablicy sa zgodne z enumem suits.
        /// </summary>
        public static Font[] znakiKart;
        /// <summary>
        /// Przechowuje tekstowy znak brydzowy (kolorek). Indeksy w tablicy są zgodne z enumem suits.
        /// </summary>
        public static char[] cardSymbols;

        /// <summary>
        /// Wypisuje odzywkę w licytacji. Moze wypisać ktr,rktr,pas lub odzywkę XY. Aby zmienić wyświetlane napisy(pas,ktr,rktr) należy zmienić
        /// wartości odpowiednich stringów w klasie Printer (plik Writer)
        /// </summary>
        /// <param name="odzywka">string licytacyjnej odzywki</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteOdzywka(string odzywka, Paragraph p = null)
        {
            odzywka = odzywka.ToUpper();

            if (p == null)
                p = new Paragraph();

            if (odzywka.Count() > 1)
            {
                p.AddText(odzywka[0].ToString());

                char suit = odzywka[1];

                WriteSuit(suit,p);
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
        /// Wypisuje zagrany kontrakt. Moze wypisać PASS lub XY(x)(x). Aby zmienić wyświetlane napisy należy zmienić je wewnątrz tej funkcji
        /// </summary>
        /// <param name="odzywka">Obiekt typu Contract z wypelnionym levelem, suitem, dbl, rdbl</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteContract(Contract contract, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

            if (contract.level > 0)
            {
                p.AddFormattedText(contract.level.ToString());

                WriteSuit(contract.suit,p);

                if (contract.dbl) p.AddFormattedText("x");
                if (contract.rdbl) p.AddFormattedText("x");
            }
            else
            {
                p.AddFormattedText("PASS");
            }

            return p;
        }


        /// <summary>
        /// Wpisuje do podanego(lub nowego) Paragraphu znaczek miana. Moze wypisać trefla,karo,kiera,pika lub "NT".
        /// Aby zmienić napis 'NT' trzeba to zrobić wewnątrz funkcji.
        /// </summary>
        /// <param name="odzywka">Enum suits</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteSuit(suits suit, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

            if (suit == suits.club) p.AddFormattedText(cardSymbols[1].ToString(), znakiKart[1]);
            if (suit == suits.diamond) p.AddFormattedText(cardSymbols[2].ToString(), znakiKart[2]);
            if (suit == suits.heart) p.AddFormattedText(cardSymbols[3].ToString(), znakiKart[3]);
            if (suit == suits.spade) p.AddFormattedText(cardSymbols[4].ToString(), znakiKart[4]);
            if (suit == suits.nt) p.AddFormattedText("NT", znakiKart[4]);

            return p;
        }



        /// <summary>
        /// Wpisuje do podanego(lub nowego) Paragraphu znaczek miana. Moze wypisać trefla,karo,kiera,pika lub "NT".
        /// Aby zmienić napis 'NT' trzeba to zrobić wewnątrz funkcji.
        /// </summary>
        /// <param name="odzywka">Pierwszą literę miana (ang) "C","D","H","S","N"</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteSuit(char suit, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

            suit = char.ToUpper(suit);
            if (suit == 'C') p.AddFormattedText(cardSymbols[1].ToString(), znakiKart[1]);
            if (suit == 'D') p.AddFormattedText(cardSymbols[2].ToString(), znakiKart[2]);
            if (suit == 'H') p.AddFormattedText(cardSymbols[3].ToString(), znakiKart[3]);
            if (suit == 'S') p.AddFormattedText(cardSymbols[4].ToString(), znakiKart[4]);
            if (suit == 'N') p.AddFormattedText("NT", znakiKart[4]);

            return p;
        }


        /// <summary>
        /// Dodaje(lub tworzy) do paragraphu ilość lew w formacie +1, -1 
        /// </summary>
        /// <param name="tricks">ilość nadróbek/niedoróbek</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteTricks(int tricks, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

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


        /// <summary>
        /// Dodaje lub tworzy paragraph z wistem.
        /// </summary>
        /// <param name="lead">Dwuznakowy string, gdzie pierwszy znak to miano (ang. 1 litera) a drugi to wartosc karty</param>
        /// <param name="p">Parametr nieobowiązkowy. Podajemy paragraph w którym chcemy coś dopisać. Wartość domyślna spowoduje dodanie nowego parametru</param>     
        /// <returns>Zedytowany lub nowy paragraph</returns>
        public static Paragraph WriteLead(string lead,Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

            if (lead != null)
            {
                p.AddFormattedText(lead[1].ToString());

                WriteSuit(lead[0],p);

            }

            return p;
        }

        /// <summary>
        /// Wypisuje linię kontraktu w formacie: Kontrakt:  (kontrakt) lew rozgrywacz , wist     score 
        /// </summary>
        /// <param name="board">Obiekt typu Board z wypełnionym wynikiem, całą struktura Contract</param>
        /// <returns>Zedytowany lub nowy paragraph</returns>
        protected virtual Paragraph WriteContractLine(Board board, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();

            p.AddFormattedText(contractLineTitle);
            p.AddSpace(2);
            WriteContract(board.contract,p);
            p.AddSpace(1);
            p.AddFormattedText(board.contract.declarer.ToString());            
            p.AddSpace(1);
            WriteTricks(board.contract.tricks, p);
            

            p.AddFormattedText(", ");

            WriteLead(board.lead,p);
            p.AddTab(); //p.AddTab();
            p.AddFormattedText(board.score.ToString());


            p.Format.Font = Czcionki.font_normal;

            return p;
        }

        protected virtual Paragraph WriteMinimax(Contract contract, Paragraph p = null)
        {
            if (p == null)
                p = new Paragraph();
            p.AddLineBreak();
            p.AddFormattedText("Minimaks teoretyczny: ");
            WriteContract(contract, p);
            p.AddFormattedText(" " + contract.declarer.ToString() + ", " + contract.score.ToString());

            p.Format.Font = Czcionki.font_normal;
            return p;

        }
    }
}
