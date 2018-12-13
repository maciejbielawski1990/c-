using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using MigraDoc.RtfRendering;

using MigraDoc.DocumentObjectModel.Shapes;

using Bridge;
using Bridge.Reading;


namespace BridgeTurbo
{
    class bbogame:Printer
    {
        protected MainRoomLin game;
        protected string napisDF = "Liczba lew do wziecia :";
        protected string napisLicytacja = "LICYTACJA";

        public bbogame(MainRoomLin m)
        {         
            game = m;
        }

        public Document Print()
        {
            document = new Document();
            document.AddSection();
            // Ustawienia
            date = game.date;
            document.LastSection.PageSetup = SetMargin();
            document.LastSection.Headers.Primary.Add(SetHeader());
            document.LastSection.Footers.Primary.Add(SetFooter());


            for (int i = 0; i < game.boards.Count; i++)
            {
                document.AddSection();
                PrintBoards(i);
            }



            return document;
        }
        public void PrintBoards(int idx)
        {
            // dodanie rozkladu
            Table rozklad = PrintBoardRozklad(game.boards[idx].rozklad, idx + 1, game.boards[idx].vulnerability);
            rozklad.Rows.LeftIndent = "1.5cm";

            document.LastSection.Add(rozklad);

            // document.LastSection.LastTable.Format.Alignment = ParagraphAlignment.Center;
            document.LastSection.Add(BreakLine.Clone());

            // dodanie DF
            int[,] analizaDF = BridgeInfo.wylicz_DF(ref game.boards[idx].rozklad);

            if (idx == 4)
                rozklad.Rows.LeftIndent = "1.5cm";

            Contract minimax = BridgeInfo.FindOptimalContract(analizaDF, game.boards[idx].vulnerability, game.boards[idx].rozklad.dealer);

            Paragraph p = new Paragraph();
            p.AddFormattedText(napisDF, Czcionki.font_header);
            document.LastSection.Add(p);
            document.LastSection.Add(BreakLine.Clone());
            document.LastSection.Add(WriteDeepFinesse(analizaDF));

            document.LastSection.Add(WriteMinimax(minimax));

            // dodanie licytacji
            p = new Paragraph();
            p.AddFormattedText(napisLicytacja, Czcionki.font_header);
            document.LastSection.Add(BreakLine.Clone());
            document.LastSection.Add(p);
            document.LastSection.Add(BreakLine.Clone());
            document.LastSection.Add(PrintBiddingTable(game.boards[idx].bidding,game.boards[idx].rozklad.dealer, game.boards[idx].players));
            document.LastSection.Add(BreakLine.Clone());
            // linie z komentarzami
            document.LastSection.Add(AddCommentLines());
        }
    }
}
