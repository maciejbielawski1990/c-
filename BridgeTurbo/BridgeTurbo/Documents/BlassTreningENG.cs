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
    class BlassTreningENG:BlassTrening
    {
        public BlassTreningENG(VugraphLin vu, MainRoomLin m)
        {
            vugraph = vu;
            game = m;
            rezultatMeczuVu = "Result of the original match ";
            napisPorownania = "The comparision of the training match to the other two tables listed below:";
            napisLicytacja = "BIDDING";
            napisDF = "Number of tricks to take:";
            napisOpisStolu1 = "Training match";
            napisOpisStolu2 = "Open room";
            napisOpisStolu3 = "Closed room";
            napisMecz = "Match";
            napis_rozdanie = "Board ";
            napis_zalozenia[1] = "None";
            napis_zalozenia[2] = "NS vul";
            napis_zalozenia[3] = "EW vul";
            napis_zalozenia[4] = "Both";
            napisPrzeciwko = "  against  ";
            napis_strona = "Page ";
        }

        /// <summary>
        /// Wstawie wiersz tabeli z tytułami nagłówków (Nr,kontrakt, wist, wynik lew, itd) dla dwóch stołów z pokoju otwartego i zamkniętego
        /// </summary>
        /// <param name="table">Tabela do której chcemy dodac wiersz</param>
        protected override void MakeTableRow3(ref Table table)
        {
            Row row = table.AddRow();

            row.Format.Font = Czcionki.font_normal;

            row.Cells[0].AddParagraph("Nr");
            row.Cells[1].AddParagraph("Contract");
            row.Cells[2].AddParagraph("By");
            row.Cells[3].AddParagraph("Lead");
            row.Cells[4].AddParagraph("Trikcs");
            row.Cells[5].AddParagraph("Score");

            row.Cells[6].AddParagraph("Contract");
            row.Cells[7].AddParagraph("By");
            row.Cells[8].AddParagraph("Lead");
            row.Cells[9].AddParagraph("Tricks");
            row.Cells[10].AddParagraph("Score");

            row.Cells[11].AddParagraph("IMP");
            row.Cells[12].AddParagraph("IMP");
            row.Cells[13].AddParagraph("Comments");

            // return table;
        }

           /// <summary>
        /// Funkcja tworzy paragraph, z liniami do wpisania komentarzy. Ustawia czcionkę tekstu na Czcionki.font_header
        /// </summary>
        /// <returns>Utworzony paragraf</returns>
        protected override Paragraph AddCommentLines()
        {
            Paragraph p = new Paragraph();
            p.Format.Font = Czcionki.font_header;

            p.AddFormattedText("ARTIFICIAL BIDS EXPLANATION :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("BIDDING COMMENTS :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("DECLARER'S PLAY COMMENTS :"); p.AddLineBreak(); p.AddLineBreak();
            p.AddFormattedText("LEAD COMMENTS :"); p.AddLineBreak(); p.AddLineBreak();

            return p;
        }

    }
       
    
}
