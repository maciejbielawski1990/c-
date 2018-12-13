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
        protected const string stopkaTekst = "maciej.bielawski@bridge24.pl";
        protected string napis_strona = "Strona ";
        protected Paragraph BreakLine = new Paragraph();
            
        public Document document;

        /// <summary>
        /// szerokosc strony w cm, do tego rozmiaru dopasowane są elementy wydruków brydżowych
        /// </summary>
        protected double szerokosc_strony = 17.5;

        /// <summary>
        /// napis w nagłówku
        /// </summary>
        protected string napisTytulowy = "Raport treningowy";

        /// <summary>
        /// data do naglowka
        /// </summary>
        protected string date = DateTime.Now.ToShortDateString();

        public Printer(VugraphLin l, MainRoomLin m)
        {
            document = new Document();
            document.AddSection();
            InitializeBridge();
            document.LastSection.PageSetup = SetMargin();
            document.LastSection.Headers.Primary.Add(SetHeader());
            document.LastSection.Footers.Primary.Add(SetFooter());

            int[,] tab = new int[6, 6];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    tab[i, j] = j;
                }
            }

            document.LastSection.Add(WriteDeepFinesse(tab));

            RtfDocumentRenderer renderer = new RtfDocumentRenderer();
            renderer.Render(document, "Test.doc", null);

            Process.Start("Test.doc");
        }

        public Printer()
        {
            InitializeBridge();
            BreakLine.AddLineBreak(); //BreakLine.AddLineBreak();
        }


        /// <summary>
        /// Inicjalizuje tablice znakiKart i cardSymbols, niezbędne do dodawania znaków brydżowych
        /// </summary>
        private void InitializeBridge()
        {
            znakiKart = new Font[5];
            for (int i = 0; i < 5; i++)
            {
                znakiKart[i] = new Font();
            }
            //trefl
            znakiKart[1].Color = Colors.Green;
            //karo
            znakiKart[2].Color = Colors.Orange;
            znakiKart[3].Color = Colors.Red;
            znakiKart[4].Color = Colors.Black;


            int piczek = 9824;
            int kierek = 9829;
            int karko = 9830;
            int trefelek = 9827;

            cardSymbols = new char[5];

            cardSymbols[1] = (char)trefelek;
            cardSymbols[2] = (char)karko;
            cardSymbols[3] = (char)kierek;
            cardSymbols[4] = (char)piczek;
           
        }

        /// <summary>
        /// Ustawia marginesy strony na sztywno. Oraz odległość stopki i nagłówka
        /// </summary>
        /// <returns>Ustawienia strony, pozniej trzeba je przypisac do dokumentu</returns>
        protected PageSetup SetMargin()
        {
            PageSetup settings = new PageSetup();

            settings.BottomMargin = Unit.FromCentimeter(0.1);
            settings.TopMargin = Unit.FromCentimeter(2.5);
            settings.LeftMargin = Unit.FromCentimeter(2.2);
            settings.RightMargin = Unit.FromCentimeter(1.3);
            settings.FooterDistance = Unit.FromCentimeter(1.0);
            settings.HeaderDistance = Unit.FromCentimeter(1.2);

            return settings;
        }  

        /// <summary>
        /// Tworzy nagłówek. Formatowania na sztywno, ale zależne od szeokosci strony. Obrazek na sztywno, wymaga istnienia pliku z obrazkiem
        /// </summary>
        /// <returns>Tabele z zawartoscia tego co ma byc w nagłówku</returns>
        protected Table SetHeader()
        {
            Table header = new Table();
            
            header.AddColumn(Unit.FromCentimeter(0.4 * szerokosc_strony)); // kolumna na tytul
            header.AddColumn(Unit.FromCentimeter(szerokosc_strony / 4)); //kolumna na obrazek
            header.AddColumn(Unit.FromCentimeter(0.35 * szerokosc_strony)); // kolumna na datę

            header.AddRow();
            header.Borders.Bottom.Width = 1.0;

            // Przygotowanie napisu nagłowkowego
            Paragraph title = new Paragraph();
            title.AddFormattedText(napisTytulowy, new Font("Verdana", 10));
            header[0, 0].Format.Alignment = ParagraphAlignment.Left;
            header[0, 0].VerticalAlignment = VerticalAlignment.Bottom;
            header[0, 0].Add(title);

            // Przygotowanie loga
            Paragraph logo = new Paragraph();
            logo.AddImage("mini_b24.jpg");
            header[0, 1].Format.Alignment = ParagraphAlignment.Left;
            header[0, 1].Add(logo);

            // Przygotowanie daty

            Paragraph data = new Paragraph();
            data.AddFormattedText(date, new Font("Verdana", 10));
            header[0, 2].Format.Alignment = ParagraphAlignment.Right;
            header[0, 2].VerticalAlignment = VerticalAlignment.Bottom;
            header[0, 2].Add(data);

            return header;
        }

        /// <summary>
        /// Tworzy stopkę - na podstawie zmiennej tekststopki. Dodaje także nr strony "Strona X". Formatowanie na sztywno, ale zalezne od szerokosci strony.
        /// </summary>
        /// <returns>Ramka tekstowa z zawartoscią tego co ma być w stopce</returns>
        protected TextFrame SetFooter()
        {
            TextFrame frame = new TextFrame();
            Table footer = new Table();
            double szerokosc_kolumny = szerokosc_strony / 2;

            frame.Width = Unit.FromCentimeter(szerokosc_strony);
            frame.Height = Unit.FromCentimeter(0.6);

            for (int i = 0; i < 2; i++)
                footer.AddColumn(Unit.FromCentimeter(szerokosc_kolumny));
            footer.AddRow();
            footer.Borders.Top.Width = 1.0;

            Paragraph p = new Paragraph();
            p.AddFormattedText(stopkaTekst,new Font("Verdana",10));
            p.Format.Alignment = ParagraphAlignment.Left;
            footer[0,0].Add(p);

            p = new Paragraph();
            p.AddFormattedText(napis_strona, new Font("Verdana", 10));
            p.AddPageField();
            footer[0,1].Add(p);
            footer.Format.Alignment = ParagraphAlignment.Right;

            frame.Add(footer);

            return frame;
 
        }

        /// <summary>
        /// Funkcja tworzy paragraph, z liniami do wpisania komentarzy. Ustawia czcionkę tekstu na Czcionki.font_header
        /// </summary>
        /// <returns>Utworzony paragraf</returns>
        protected virtual Paragraph AddCommentLines()
        {
            Paragraph p = new Paragraph();
            p.Format.Font = Czcionki.font_header;

            p.AddFormattedText("WYJAŚNIENIE ODZYWEK KONWENCYJNYCH :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("KOMENTARZ DO LICYTACJI :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("KOMENTARZ DO ROZGRYWKI :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("KOMENTARZ DO WISTU :"); p.AddLineBreak(); p.AddLineBreak();


            return p;
        }

    }






}
