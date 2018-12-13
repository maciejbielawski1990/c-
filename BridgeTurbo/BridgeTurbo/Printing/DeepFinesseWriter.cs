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
        protected double szerokosc_kolumny_df = 1.347;
        protected double szerokosc_borders_df = 1.0;
        protected double wysokosc_kolumny_df = 0.6;

        protected void SetDFWriterProperties(double szer_col_width = 1.347, double borders_width = 1.0, double row_height = 0.6)
        {
            szerokosc_kolumny_df = szer_col_width;
            szerokosc_borders_df = borders_width;
            wysokosc_kolumny_df = row_height;
        }

        /// <summary>
        /// Funkcja tworzy tabelę z analizą DF. Podajemy tylko tablicę z lewami ktorą trzeba wpisac. Funkcja ustala wysokosc wiersza na twardo(0.7cm).
        /// Szerokosc kolumn na podstawie zmiennej szerkosc_kolumny_df. Wpisuje na sztywno NSEW oraz suity. I ilosc lew na podstawie arg.
        /// </summary>
        /// <param name="lewy">Tablica z wypelnioną ilością lew, gdzie [i,j] i to gracz a j to kolor (j=0 to trefle)</param>
        /// <returns>Tabelę z analizą DF</returns>
        public Table WriteDeepFinesse(int[,] lewy, Font f = null)
        {
            Table table = new Table();

            //wysokosc wierszy
            table.Rows.Height = Unit.FromCentimeter(wysokosc_kolumny_df);

            for (int i = 0; i < 13; i++)
                table.AddColumn(Unit.FromCentimeter(szerokosc_kolumny_df));

            for (int i = 0; i < 3; i++)
                table.AddRow();

            table[1, 0].AddParagraph("N");
            table[2, 0].AddParagraph("S");
            table[1, 7].AddParagraph("E");
            table[2, 7].AddParagraph("W");

            // strona NS
            for (int i = 2; i < 6; i++)
            {
                Paragraph p = new Paragraph();
                WriteSuit((suits)(6 - i), p);

                table[0, i].Add(p);
                table[1, i].AddParagraph(lewy[0, 5 - i].ToString());
                table[2, i].AddParagraph(lewy[1, 5 - i].ToString());
            }
            table[0, 1].AddParagraph("NT");
            table[1, 1].AddParagraph(lewy[0, 4].ToString());
            table[2, 1].AddParagraph(lewy[1, 4].ToString());

            // strona EW
            for (int i = 9; i < 13; i++)
            {
                Paragraph p = new Paragraph();
                WriteSuit((suits)(13 - i),p);
                table[0, i].Add(p);
                table[1, i].AddParagraph(lewy[2, 12 - i].ToString());
                table[2, i].AddParagraph(lewy[3, 12 - i].ToString());
            }

            table[0, 8].AddParagraph("NT");
            table[1, 8].AddParagraph(lewy[2, 4].ToString());
            table[2, 8].AddParagraph(lewy[3, 4].ToString());

            // ustawienie kreski na kolorami
            table.Rows[0].Borders.Bottom.Width = szerokosc_borders_df;

            // ustawienie kreski po literkach
            table[0, 0].Borders.Right.Width = szerokosc_borders_df;
            table[1, 0].Borders.Right.Width = szerokosc_borders_df;
            table[2, 0].Borders.Right.Width = szerokosc_borders_df;

            table[0, 7].Borders.Right.Width = szerokosc_borders_df;
            table[1, 7].Borders.Right.Width = szerokosc_borders_df;
            table[2, 7].Borders.Right.Width = szerokosc_borders_df;

            // zlikwidowanie kreski w kolumnach przerwy
            table[0, 6].Borders.Bottom.Width = 0;

            table.Format.Alignment = ParagraphAlignment.Center;
            table.Rows.VerticalAlignment = VerticalAlignment.Center;

            if (f == null)
                table.Format.Font = Czcionki.font_deep;
            else
                table.Format.Font = f;

            return table;
        }


        // Mozna dopisac deepa z tego co jest w pbnie

    }
}
