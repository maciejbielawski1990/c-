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
        protected double szerokosc_kolumny_rozkladu = 14.5 / 3;
        Color kolor_ramy_rozkladu = Colors.AntiqueWhite;
        protected string napis_rozdanie = "Rozdanie ";
        protected string[] napis_zalozenia = { "0", "obie przed partią", "NS po partii", "EW po partii", "obie po partii" };
        //POTRZEBA 2.8 CM NA ROZKLAD !!!

        protected double rozkladKart_borders = 2.0;

        void SetRozkladWriterProperties(double borders, Color kolor_ramy)
        {
            rozkladKart_borders = borders;
            kolor_ramy_rozkladu = kolor_ramy;
        }
        
        /// <summary>
        /// Tworzy całą tabelę z rozkładem. Szerokość danych kolumn zalezy od zmiennej szerokosc_kolumny_rozkladu. Odległość rozkładu od krawędzi dla
        /// ręki N wynosi 0.5(gora, dol) lewa i prawa jest zalezna od szerokosci kolumny, analogicznie inne. Ustawia środkową komórką na ramkę.
        /// Foramtuje tabele
        /// </summary>
        /// <param name="rozklad">Rozklad wg struktury RozkladKart do wydrukowania</param>
        /// <param name="nr">Nr potrzebny do ramki</param>
        /// <param name="vul">Zalozenia potrzebne do ramki</param>
        /// <returns></returns>
        public Table PrintBoardRozklad(RozkladKart rozklad, int nr, vulnerabilties vul)
        {
            Table table = new Table();
            //table.Borders.Width = 1.0;
            //table.Borders.Color = Colors.Red;
            double spaceBA = 0.3;
            
            Column column1 = table.AddColumn(Unit.FromCentimeter(szerokosc_kolumny_rozkladu));
            Column column2 = table.AddColumn(Unit.FromCentimeter(szerokosc_kolumny_rozkladu));
            Column column3 = table.AddColumn(Unit.FromCentimeter(szerokosc_kolumny_rozkladu));

            table.Format.Alignment = ParagraphAlignment.Justify;

            /////////////////////////////////N//////////////////////////////
            Row row = table.AddRow();
            Paragraph p = row.Cells[1].AddParagraph();
            double margin = (szerokosc_kolumny_rozkladu - 2.8) / 2;

            p.Format.LeftIndent = Unit.FromCentimeter(margin);
            p.Format.RightIndent = Unit.FromCentimeter(margin / 2);
            p.Format.SpaceBefore = Unit.FromCentimeter(spaceBA);
            p.Format.SpaceAfter = Unit.FromCentimeter(spaceBA);
            WriteHand(rozklad.N, p);
            //     p.AddLineBreak();

            /////////////////////////////////W//////////////////////////////

            Row row2 = table.AddRow();
            p = row2[0].AddParagraph();
            p.Format.LeftIndent = Unit.FromCentimeter(margin * 2);
            p.Format.SpaceBefore = Unit.FromCentimeter(spaceBA);
            p.Format.SpaceAfter = Unit.FromCentimeter(spaceBA);
            WriteHand(rozklad.W, p);

            /////////////////////////////////E//////////////////////////////
            p = row2[2].AddParagraph();
            p.Format.LeftIndent = Unit.FromCentimeter(0.5);
            p.Format.SpaceBefore = Unit.FromCentimeter(spaceBA);
            p.Format.SpaceAfter = Unit.FromCentimeter(spaceBA);
            WriteHand(rozklad.E, p);


            /////////////////////////////////////S//////////////////////////////////////////////
            Row row3 = table.AddRow();
            p = row3[1].AddParagraph();

            p.Format.LeftIndent = Unit.FromCentimeter(margin);
            p.Format.RightIndent = Unit.FromCentimeter(margin / 2);
            p.Format.SpaceBefore = Unit.FromCentimeter(spaceBA);
            p.Format.SpaceAfter = Unit.FromCentimeter(spaceBA);

            WriteHand(rozklad.S, p);

            Cell cell = new Cell();
            cell = row2.Cells[1];

            CreateFrame(ref cell, nr, vul);
           

            //ustawienie ramki tabeli 
            //  table.SetEdge(0, 0, 3, 1, Edge.Box, BorderStyle.None, 0.0, Colors.White);

            return table;
        }

        /// <summary>
        /// Tworzy ramkę tytułową w rozkładzie. Ustawie szerokość krawędzi(na sztywno), kolor ramy wg zmiennej kolor_ramy_rozkladu.
        /// Formatuje całą tabele, tworzy napisy(na sztywno) można wpłynąć na założenia i nr rozdania.
        /// </summary>
        /// <param name="cell">Komórka tabeli do której chcemy wpisać tą ramkę </param>
        /// <param name="nr"> Cyfra, która ma stac przy nr rozdania</param>
        /// <param name="zalozenia">Załozenie do wypisania - enmum vulnerabilities</param>
        /// <returns></returns>
        protected void CreateFrame(ref Cell cell, int nr, vulnerabilties zalozenia, Font fRozd = null, Font fVul = null)
        {

            if (fRozd == null)
                fRozd = Czcionki.font_normalBold;
            if (fVul == null)
                fVul = Czcionki.font_smallbold;

            Paragraph naglowek1 = new Paragraph();
            string rozdanie_string = napis_rozdanie + nr;

            //Formatowanie ramki
            cell.Borders.Width = rozkladKart_borders;
            cell.Shading.Color = kolor_ramy_rozkladu;



            // Formatowanie napisu rozdanie
            naglowek1 = cell.AddParagraph();
            naglowek1.Format.Alignment = ParagraphAlignment.Center;
            naglowek1.Format.Font = fRozd;

            naglowek1.AddText(rozdanie_string);

            naglowek1.AddLineBreak(); naglowek1.AddLineBreak();

            // Formatowanie napisu zalozenia
            Paragraph naglowek2 = new Paragraph();
            naglowek2 = cell.AddParagraph();
            naglowek2.Format.Font = fVul;
            naglowek2.Format.Alignment = ParagraphAlignment.Center;

            string zal_ = "";

            zal_ = napis_zalozenia[(int)zalozenia];

            // Przyklad korzysci z zastosowania typu wyliczeniowego, a raczej z wpisania czegos w zalozenia

            //if (InfoBridge.NS_partia[nr] == 0 && InfoBridge.WE_partia[nr] == 0) zal_ = zalozenie[0];
            //if (InfoBridge.NS_partia[nr] == 1 && InfoBridge.WE_partia[nr] == 0) zal_ = zalozenie[1];
            //if (InfoBridge.NS_partia[nr] == 0 && InfoBridge.WE_partia[nr] == 1) zal_ = zalozenie[2];
            //if (InfoBridge.NS_partia[nr] == 1 && InfoBridge.WE_partia[nr] == 1) zal_ = zalozenie[3];
            naglowek2.AddText(zal_);

            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        /// <summary>
        /// Wypisuje caly uklad w jednej linii, ale niestety nie da sie go wkleić w jakiś fragment tekstu. Bo tworzy caly paragraf.
        /// </summary>
        /// <param name="karty">Karty wg struktury karty</param>
        /// <param name="p"></param>
        /// <returns>Paragraf z rozkladem w jednej linii</returns>
        public static Paragraph WriteHandInOneLine(Karty karty)
        {
            Paragraph p = new Paragraph();
            
            p = WriteHandSuit(suits.spade, karty.spades, p);

            p = WriteHandSuit(suits.heart, karty.hearts, p);

            p = WriteHandSuit(suits.diamond, karty.diamonds, p);

            p = WriteHandSuit(suits.club, karty.clubs, p);

            return p;
        }

        /// <summary>
        /// Wypisuje rękę gracza. Każdy kolor w osobnej linii
        /// </summary>
        /// <param name="karty">Karty gracza w strukturze Karty</param>   
        /// <returns>Paragraf z ręką</returns>
        public static Paragraph WriteHand(Karty karty, Paragraph p = null)
        {
            if (p == null)
            {
                p = new Paragraph();
            }
          
            p = WriteHandSuit(suits.spade, karty.spades, p); p.AddLineBreak();

            p = WriteHandSuit(suits.heart, karty.hearts, p); p.AddLineBreak();

            p = WriteHandSuit(suits.diamond, karty.diamonds, p); p.AddLineBreak();

            p = WriteHandSuit(suits.club, karty.clubs, p); p.AddLineBreak();

            return p;
        }


        /// <summary>
        /// Funkcja dodaje(lub domyslnie tworzy) do paragrafu karty z zadnego koloru.
        /// </summary>
        /// <param name="suit">Zadany znaczek do wstawienia przy kolorze</param>
        /// <param name="karty">Karty do wypisania</param>
        /// <param name="p">Ewentualny paragraf w ktorym ma byc dodany kolor</param>
        /// <returns></returns>
        protected static Paragraph WriteHandSuit(suits suit, string karty, Paragraph p = null)
        {
            if (p == null)
            {
                p = new Paragraph();
            }
            int idx = (int)suit;

            p.AddFormattedText(cardSymbols[idx].ToString(), znakiKart[idx]);
            p.AddText(" " + karty);

            return p;
        }
    }
}
