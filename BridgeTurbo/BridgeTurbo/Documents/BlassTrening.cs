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
    class BlassTrening : Printer
    {
        protected VugraphLin vugraph;
        protected MainRoomLin game;
        protected string rezultatMeczuVu = "Rezultat segmentu w meczu oryginalnym ";
        protected string napisPorownania = "Poniżej porównanie meczu treningowego do dwóch pozostałych stołów";
        protected string napisLicytacja = "LICYTACJA";
        protected string napisDF = "Liczba lew do wziecia :";
        protected string napisMecz = "Mecz ";

        public BlassTrening(VugraphLin vu, MainRoomLin m)
        {
            vugraph = vu;
            game = m;
        }

        public BlassTrening()
        {
        }

        public Document Print()
        {
            document = new Document();
            document.AddSection();
            // Ustawienia
            napisTytulowy = vugraph.title.title;
            date = game.date;
            document.LastSection.PageSetup = SetMargin();
            document.LastSection.Headers.Primary.Add(SetHeader());
            document.LastSection.Footers.Primary.Add(SetFooter());

            //strona 1
            Paragraph FirstPageTitle = new Paragraph();
            rezultatMeczuVu += vugraph.title.team1 + " - " + vugraph.title.team2;
            int[] wynikMeczu = BridgeInfo.wylicz_mecz(ref vugraph.boards, ref vugraph.boards_closed);
            rezultatMeczuVu += " " + wynikMeczu[0].ToString() + ":" + wynikMeczu[1].ToString();
            FirstPageTitle.AddFormattedText(rezultatMeczuVu, Czcionki.font_header);
            FirstPageTitle.AddLineBreak();
            FirstPageTitle.AddLineBreak();
            document.LastSection.Add(FirstPageTitle);
            FirstPageTitle = new Paragraph();
            FirstPageTitle.AddFormattedText(napisPorownania, Czcionki.font_normal);
            FirstPageTitle.AddLineBreak();
            FirstPageTitle.AddLineBreak();
            document.LastSection.Add(FirstPageTitle);

            Paragraph TabelaTitle = new Paragraph();
            TabelaTitle.AddFormattedText(
                napisMecz + "nr 1 : " + PrepareTableTitle(game.boards[0].players, vugraph.boards[0].players),
                Czcionki.font_red);
            TabelaTitle.AddLineBreak();
            TabelaTitle.AddLineBreak();
            document.LastSection.Add(TabelaTitle);

            document.LastSection.Add(MakeMatchScoreTable(game.boards, vugraph.boards));

            //strona 2
            document.AddSection();
            TabelaTitle = new Paragraph();
            TabelaTitle.AddFormattedText(
                napisMecz + "nr 2 : " + PrepareTableTitle(game.boards[0].players, vugraph.boards_closed[0].players),
                Czcionki.font_red);
            TabelaTitle.AddLineBreak();
            TabelaTitle.AddLineBreak();
            document.LastSection.Add(TabelaTitle);

            document.LastSection.Add(MakeMatchScoreTable(game.boards, vugraph.boards_closed));

            // strony z rozdaniami 
            licytacja_szerokosc_kolumny = 1.375;
            for (int i = 0; i < game.boards.Count; i++)
            {
                document.AddSection();
                PrintBoards(i);
            }
            //document.LastSection.Add(PrintBoardRozklad(main.boards[0].rozklad, 1, vulnerabilties.none));

            //Paragraph p = new Paragraph();
            //p.AddFormattedText("Liczba lew do wziecia", Czcionki.font_normal);
            //document.LastSection.Add(p);



            //string[] a = {"a","b","c","d","E"};
            //p = new Paragraph();
            //p.AddFormattedText("LICYTACJA", Czcionki.font_header);
            //document.LastSection.Add(p);
            //document.LastSection.Add(PrintBiddingTable(main.boards[0].bidding,positions.N,a));

            //document.LastSection.Add(AddCommentLines());


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

            Contract minimax = BridgeInfo.FindOptimalContract(analizaDF, game.boards[idx].vulnerability,
                game.boards[idx].rozklad.dealer);

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
            Table biddingtrening = PrintBiddingTable(game.boards[idx].bidding, game.boards[idx].rozklad.dealer,
                game.boards[idx].players);
            Paragraph p_tmp = new Paragraph();
            p_tmp.AddLineBreak();

         
            document.LastSection.Add(PrintConsolationBidding(idx));
            document.LastSection.Add(BreakLine.Clone());
            // linie z komentarzami
            document.LastSection.Add(AddCommentLines());
        }




    


    public Table PrintConsolationBidding(int idx)
    {
        Table biddingtrening = PrintBiddingTable(game.boards[idx].bidding, game.boards[idx].rozklad.dealer, game.boards[idx].players);
        Table biddingOpen = PrintBiddingTable(vugraph.boards[idx].bidding, vugraph.boards[idx].rozklad.dealer, vugraph.boards[idx].players);
        Table biddingClosed = PrintBiddingTable(vugraph.boards_closed[idx].bidding, vugraph.boards_closed[idx].rozklad.dealer, vugraph.boards_closed[idx].players);


        double szer = 6.0;
        double tfszer = 4.5;

        Paragraph p_tmp = new Paragraph(); p_tmp.AddLineBreak();

        TextFrame tf1 = new TextFrame();
        tf1.Width = Unit.FromCentimeter(tfszer);
        tf1.Height = Unit.FromCentimeter(biddingtrening.Rows.Count * 0.6 + 1.0);
        tf1.Add(biddingtrening);
        tf1.Add(p_tmp);
        Paragraph p = WriteContractLine(game.boards[idx]);
        tf1.Add(p);

        TextFrame tf2 = new TextFrame();
        //  tf2.Left = Unit.FromCentimeter(0);
        tf2.Width = Unit.FromCentimeter(tfszer);
        tf2.Height = Unit.FromCentimeter(biddingOpen.Rows.Count * 0.6 + 1.0);
        tf2.Add(biddingOpen);

        p_tmp = new Paragraph(); p_tmp.AddLineBreak();
        tf2.Add(p_tmp);
        p = WriteContractLine(vugraph.boards[idx]);
        tf2.Add(p);

        TextFrame tf3 = new TextFrame();
        // tf3.Left = Unit.FromCentimeter(0.08);
        tf3.Width = Unit.FromCentimeter(tfszer);
        tf3.Add(biddingClosed);
        tf3.Height = Unit.FromCentimeter(biddingClosed.Rows.Count * 0.6 + 1.0);

        p_tmp = new Paragraph(); p_tmp.AddLineBreak();
        p = WriteContractLine(vugraph.boards_closed[idx]);
        tf3.Add(p_tmp);
        tf3.Add(p);

        Table biddingTable = new Table();

        biddingTable.AddColumn(Unit.FromCentimeter(szer)); biddingTable.AddColumn(Unit.FromCentimeter(szer)); biddingTable.AddColumn(Unit.FromCentimeter(szer));
        Row row = biddingTable.AddRow();
        //biddingTable.Borders.Width = 1.0;
        //biddingTable.Borders.Color = Colors.Red;

        biddingTable[0, 0].Add(tf1);
        biddingTable[0, 1].Add(tf2);
        biddingTable[0, 2].Add(tf3);

        return biddingTable;

    }
}
}
