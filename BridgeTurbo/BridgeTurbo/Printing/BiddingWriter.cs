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
using Bridge.Reading;

namespace BridgeTurbo
{
    partial class Printer
    {
        /// <summary>
        /// kolor tla pierwszej linii
        /// </summary>
        protected Color kolor_tla_licytacja = Colors.LightGray;
        /// <summary>
        /// Szerokosc kolumny w licytacji
        /// </summary>
        protected double licytacja_szerokosc_kolumny = 2.0;

        protected double licytacja_wysokosc_kolumny = 0.6;

        protected double licytacja_szerkosc_borders = 1.0;
        /// <summary>
        /// napisy do 1 linii licytacji
        /// </summary>
        protected string[] pozycjaNESW = { "N", "E", "S", "W" };

        protected bool linia_z_nazwiskami = true;

        void SetBiddingWriterProperties(Color kolor_tla, double szerokosc_kolumny, double wysokosc_kolumny, double borders, string[] naglowki)
        {
            kolor_tla_licytacja = kolor_tla;
            licytacja_szerokosc_kolumny = szerokosc_kolumny;
            licytacja_wysokosc_kolumny = wysokosc_kolumny;
            licytacja_szerkosc_borders = borders;
            pozycjaNESW = naglowki;
        }

        /// <summary>
        /// Funkcja tworzy całą tabelkę z licytacją z pojedynczego stołu. Tworzy kolumny na podstawie zmiennej licytacja_szerkosc_kolumny. 
        /// Tworzy wiersz nagłówkowy, oraz z nazwiskami, i jesli licytacja nie jest pusta to tworzy poszczegolne odzywki. Formatuje tabele
        /// </summary>
        /// <param name="bidding">licytacja do wypisania</param>
        /// <param name="dealer">Rozdajacy, zeby wiedziec skad zaczac pisac</param>
        /// <param name="surnames">Nazwiska do wpisania w strukturze enuma position</param>
        /// <returns></returns>
        public Table PrintBiddingTable(List<Bidding> bidding, positions dealer, string[] surnames)
        {
            Table table = new Table();

            for (int i = 0; i < 4; i++)
                table.AddColumn(Unit.FromCentimeter(licytacja_szerokosc_kolumny));

            Row row = table.AddRow();
            CreateBiddingFirstRow(ref row);
            if (linia_z_nazwiskami)
            {
                row = table.AddRow();
                CreateBiddingSurnameRow(ref row, surnames);
            }

            if (bidding.Count > 0)
                WriteBids(ref table, bidding, dealer);

            table.Format.Alignment = ParagraphAlignment.Center;
            table.Rows.Height = Unit.FromCentimeter(licytacja_wysokosc_kolumny);
            table.Borders.Width = licytacja_szerkosc_borders;
            table.Rows.VerticalAlignment = VerticalAlignment.Center;

            return table;
        }


        /// <summary>
        /// Wpisuje do zadanej tabeli licytację. 
        /// </summary>
        /// <param name="table">Zadana tabela</param>
        /// <param name="bidding">Lista z licytacja</param>
        /// <param name="dealer">Rozdający w celu poprawnego miejsca otwarcia licytacji domyslnie N</param>
        /// <returns></returns>
        private Table WriteBids(ref Table table, List<Bidding> bidding, positions dealer = positions.N, Font f = null)
        {
            int dealer_pos = ((int)dealer + 1) % 4; // N = 0, S = 2 E = 1
            int suma = dealer_pos + bidding.Count;
            Row row = null;
            if (dealer_pos != 0)
            {
                row = table.AddRow();
                if (f == null)
                    row.Format.Font = Czcionki.font_normal;
                else
                    row.Format.Font = f;

            }

            for (int i = 0; i < bidding.Count; i++)
            {

                int colNr = (dealer_pos + i) % 4;

                if (colNr == 0)
                {
                    row = table.AddRow();
                    if (f == null)
                        row.Format.Font = Czcionki.font_normal;
                    else
                        row.Format.Font = f;

                }
                if (bidding[i].odzywka != null)
                    row[colNr].Add(WriteOdzywka(bidding[i].odzywka));

            }

            return table;
        }

        /// <summary>
        /// Uzupelnia wiersz nagłówkowy licytacji. Zgodnie z tablicą stringów pozycjaNESW. Ustawia czcionkę oraz kolor tła licytacji.
        /// </summary>
        /// <param name="row"> Zadany wiersz</param>
        private void CreateBiddingFirstRow(ref Row row, Font f = null)
        {
            for (int i = 0; i < 4; i++)
                row.Cells[i].AddParagraph(pozycjaNESW[i]);


            row.Shading.Color = kolor_tla_licytacja;

            if (f == null)
                row.Format.Font = Czcionki.font_normal;
            else
                row.Format.Font = f;
        }

        /// <summary>
        /// Funkcja wpisuje nazwiska do zadanego wiersza. 
        /// </summary>
        /// <param name="row">Zadany wiersz</param>
        /// <param name="players">Nazwiska w tabeli wg enuma position</param>
        private void CreateBiddingSurnameRow(ref Row row, string[] players, Font f = null)
        {
            if (f == null)
                row.Format.Font = Czcionki.font_small;
            else
                row.Format.Font = f;

            row.Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2].AddParagraph(players[(int)positions.S]); // S
            row.Cells[3].AddParagraph(players[(int)positions.W]); // W
            row.Cells[0].AddParagraph(players[(int)positions.N]); // N
            row.Cells[1].AddParagraph(players[(int)positions.E]); // E
        }

        /// <summary>
        /// Funkcja wypelnia zadany wiersz nazwiskami
        /// </summary>
        /// <param name="row">Zadany wiersz</param>
        /// <param name="surnames">Nazwiska dowolne SWNE</param>
        private void CreateBiddingSurnameRow2(ref Row row, string[] surnames, Font f = null)
        {
            if (f == null)
                row.Format.Font = Czcionki.font_small;
            else
                row.Format.Font = f;

            row.Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2].AddParagraph(surnames[0]); // S
            row.Cells[3].AddParagraph(surnames[1]); // W
            row.Cells[0].AddParagraph(surnames[2]); // N
            row.Cells[1].AddParagraph(surnames[3]); // E
        }
    }
}
