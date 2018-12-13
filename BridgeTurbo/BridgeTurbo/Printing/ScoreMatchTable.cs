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
        public double[] wyniki_szerokosc_kolum = { 0.6, 1.7, 0.8, 1, 1, 1.5, 1.7, 0.8, 1, 1, 1.5, 1.68, 1.68, 1.54 }; // suma 17.5

        protected string napisTeam1 = "Team 1";
        protected string napisTeam2 = "Team 2";
        protected string napisOpisStolu1 = "Mecz treningowy";
        protected string napisOpisStolu2 = "Pokój otwarty";
        protected string napisOpisStolu3 = "Pokój zamknięty";
        protected string napisPrzeciwko = "  przeciwko  ";

        void SetScoreMarchTableProperties(double[] szerkosc_kolumn)
        {
            wyniki_szerokosc_kolum = szerkosc_kolumn;
        }


        /// <summary>
        /// Tworzy table MatchScoreTable
        /// </summary>
        /// <param name="PO">Lista rozdan z pokoju jednego</param>
        /// <param name="PZ">Lista rozdan z pokoju drugiego</param>
        /// <returns>Cała wypełniona tabela</returns>
        protected virtual Table MakeMatchScoreTable(List<Board> PO, List<Board> PZ)
        {
            Table table = new Table(); 
            

            foreach (double d in wyniki_szerokosc_kolum)
                table.AddColumn(Unit.FromCentimeter(d));
      
            MakeTableRow1(ref table, PO[0].players, PZ[0].players);
            MakeTableRow2(ref table, napisOpisStolu1, napisOpisStolu2, PO[0].players, PZ[0].players);
            MakeTableRow3(ref table);

            for (int i = 0; i < PO.Count; i++)
            {
                MakeTableRowBoard(ref table, PO[i], PZ[i], i + 1);
            }
            int[] imps = BridgeInfo.wylicz_mecz(ref PO, ref PZ);
            JoinLineIMP(ref table, imps[0], imps[1]);
            double vpNS = BridgeInfo.wylicz_vp((imps[0] - imps[1]), PO.Count);
            JoinLineVP(ref table, vpNS);

            
            ScoreMatchTableProperties(ref table);
            return table;
        }

        /// <summary>
        /// Tworzy pierwszy wiersz z nazwiskami i napisami team1 team2
        /// </summary>
        /// <param name="table">Tabela do ktorej dodajemy wiersz 1</param>
        /// <param name="surnames1">Nazwiska z pierwszego stolu</param>
        /// <param name="surnames2">Nazwiska z drugiego stolu</param>
        protected virtual void MakeTableRow1(ref Table table, string[] surnames1, string[] surnames2)
        {
            // w [0] - ns , w [1] we
            Row row = table.AddRow();
            row.Format.Font = Czcionki.font_surnames;
            Paragraph p = row.Cells[1].AddParagraph();


            p.AddFormattedText(PrepareSurnames(surnames1)[0], Czcionki.font_surnames);
            p.AddLineBreak();
            p.AddFormattedText(PrepareSurnames(surnames1)[1], Czcionki.font_surnames);

            row.Cells[1].MergeRight = 4;

            p = row.Cells[6].AddParagraph();

            p.AddFormattedText(PrepareSurnames(surnames2)[0], Czcionki.font_surnames);
            p.AddLineBreak();
            p.AddFormattedText(PrepareSurnames(surnames2)[1], Czcionki.font_surnames);


            row.Cells[6].MergeRight = 4;

            row.Cells[11].Format.Font = Czcionki.font_surnames.Clone();
            row.Cells[11].AddParagraph(napisTeam1);
            row.Cells[12].Format.Font = Czcionki.font_surnames.Clone();
            row.Cells[12].AddParagraph(napisTeam2);

        }

        /// <summary>
        /// Przygotowuje wiersz drugi tabeli. Napis i inicjaly
        /// </summary>
        /// <param name="table">Tabela do ktorej dodajemy wiersz</param>
        /// <param name="tabletitle1 i 2 "> Naglowki tabeli, podajemy ogólną nazwę stolu np. "Mecz treningowy", "Pokój otwarty"</param>
        /// <param name="surnames1 i 2"> Nazwiska z poszczególnych stołów</param>    
        protected virtual void MakeTableRow2(ref Table table, string tabletitle1, string tabletitle2, string[] surnames1, string[] surnames2)
        {
            Row row = table.AddRow();

            row.Format.Font = Czcionki.font_normalBold;

            row.Cells[1].AddParagraph(tabletitle1); //"Mecz treningowy"
            row.Cells[1].MergeRight = 4;

            row.Cells[6].AddParagraph(tabletitle2);
            row.Cells[6].MergeRight = 4;
            
            string[] table1 = PrepareInicjaly(surnames1);
            string[] table2 = PrepareInicjaly(surnames2);

            string inicjaly1 = table1[0] + " + " + table2[1];
            string inicjaly2 = table1[1] + " + " + table2[0];

            row.Cells[11].AddParagraph(inicjaly1);
            row.Cells[12].AddParagraph(inicjaly2);
        }

        /// <summary>
        /// Wstawie wiersz tabeli z tytułami nagłówków (Nr,kontrakt, wist, wynik lew, itd) dla dwóch stołów z pokoju otwartego i zamkniętego
        /// </summary>
        /// <param name="table">Tabela do której chcemy dodac wiersz</param>
        protected virtual void MakeTableRow3(ref Table table)
        {
            Row row = table.AddRow();

            row.Format.Font = Czcionki.font_normal;

            row.Cells[0].AddParagraph("Nr");
            row.Cells[1].AddParagraph("Kontrakt");
            row.Cells[2].AddParagraph("By");
            row.Cells[3].AddParagraph("Wist");
            row.Cells[4].AddParagraph("Lew");
            row.Cells[5].AddParagraph("Wynik");

            row.Cells[6].AddParagraph("Kontrakt");
            row.Cells[7].AddParagraph("By");
            row.Cells[8].AddParagraph("Wist");
            row.Cells[9].AddParagraph("Lew");
            row.Cells[10].AddParagraph("Wynik");

            row.Cells[11].AddParagraph("IMP");
            row.Cells[12].AddParagraph("IMP");
            row.Cells[13].AddParagraph("Uwagi");

            // return table;
        }

        /// <summary>
        /// Dodaje wiersz do tabeli z zadanym rozdaniem zarówno z PO jak i PZ. 
        /// </summary>
        /// <param name="table">Tabela do ktorej dodajemy wiersz</param>
        /// <param name="BO">Rozdanie z PO </param>
        /// <param name="BC">Rozdanie z PZ</param>
        /// <param name="nr">Nr rozdania do wpisania jak lp.</param>
        protected virtual void MakeTableRowBoard(ref Table table, Board BO, Board BC, int nr)
        {
            Row row = table.AddRow();
            row[0].AddParagraph(nr.ToString());
            row[1].Add(WriteContract(BO.contract));
            row[2].AddParagraph(BO.contract.declarer.ToString());
            row[3].Add(WriteLead(BO.lead));
            row[4].Add(WriteTricks(BO.contract.tricks));
            row[5].AddParagraph(BO.score.ToString());
            if (BO.score < 0)
                row[5].Format.Font.Color = Colors.Red;

            row[6].Add(WriteContract(BC.contract));
            row[7].AddParagraph(BC.contract.declarer.ToString());
            row[8].Add(WriteLead(BC.lead));
            row[9].Add(WriteTricks(BC.contract.tricks));
            row[10].AddParagraph(BC.score.ToString());
            if (BC.score < 0)
                row[10].Format.Font.Color = Colors.Red;

            int imp = BridgeInfo.wylicz_impy(BO.score - BC.score);
            if (imp == 0)
            {
                row[11].AddParagraph("-");
                row[12].AddParagraph("-");
            }
            else
            {
                if (imp > 0)
                    row[11].AddParagraph(imp.ToString());
                else
                    row[12].AddParagraph((-imp).ToString());
            }

            row.Format.Font = Czcionki.font_normal;
            
        }

        /// <summary>
        /// Wstawia wiersz z sumą impów. Ustawia całe formatowanie dla tego wiersza. 
        /// </summary>
        /// <param name="table">Tabela do ktorej wstawiamy ilość impów</param>
        /// <param name="impNS">Suma impow dla NS</param>
        /// <param name="impWE">Suma impow dla WE</param>
        private void JoinLineIMP(ref Table table, int impNS, int impWE)
        {
            Row row2 = table.AddRow();

            row2.Borders.Left.Width = 0;
            row2.Borders.Right.Width = 0;
            row2.Borders.Bottom.Width = 0;
            row2[11].Borders.Left.Width = 0.1;
            row2[11].Borders.Right.Width = 0.1;
            row2[11].Borders.Bottom.Width = 0.1;
            row2[12].Borders = row2[11].Borders.Clone();
            row2.Format.Alignment = ParagraphAlignment.Center;
            row2.Cells[11].AddParagraph(impNS.ToString());
            row2.Cells[12].AddParagraph(impWE.ToString());
            row2.Format.Font = Czcionki.font_normal;
        }

        /// <summary>
        /// Dodaje w ostatnia linie tabeli i wpisuje tam wynik w vpach i ustawia cale obramowanie tego wiersza dla ScoreMatchTable!
        /// </summary>
        /// <param name="table">Tabela do ktorej dodajemy wiersz</param>
        /// <param name="vpNS">ilosc vp dla pary NS</param>
        private void JoinLineVP(ref Table table, double vpNS)
        {
            Row row2 = table.AddRow();
            double vpWE = 20 - vpNS;

            row2.Format.Font = Czcionki.font_normal;
            row2.Format.Font.Bold = true;

            row2.Borders.Left.Width = 0;
            row2.Borders.Right.Width = 0;
            row2.Borders.Bottom.Width = 0;
            row2.Borders.Top.Width = 0;
            row2[11].Borders.Left.Width = 0.1;
            row2[11].Borders.Right.Width = 0.1;
            row2[11].Borders.Bottom.Width = 0.1;
            row2[11].Borders.Top.Width = 0.1;
            row2[12].Borders = row2[11].Borders.Clone();
            row2.Format.Alignment = ParagraphAlignment.Center;

            row2.Cells[11].AddParagraph(vpNS.ToString());
            row2.Cells[12].AddParagraph(vpWE.ToString());
        }

        /// <summary>
        /// Ustawia właściwości tabeli, wysokosc, szerokość, obramowanie itp dla tabeli z prezentacja gry meczowej. Tak jak w treningach b24.
        /// </summary>
        /// <param name="table">Referencja do tabeli dla ktorej to sie ustawia</param>
        protected void ScoreMatchTableProperties(ref Table table)
        {
            // table.Format.Font.Name = "Cambria";
            //   table.Format.Font.Size = 8;

            table.Borders.Width = 0.1;
            table.Rows.Height = Unit.FromCentimeter(0.66);
            table.Rows.VerticalAlignment = VerticalAlignment.Center;
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Rows[0].Height = Unit.FromCentimeter(1.38);
            table.Rows[1].Height = Unit.FromCentimeter(1.08);
            table.Rows[2].Height = Unit.FromCentimeter(1.08);

            table[0, 0].Borders.Width = 0;
            table[1, 0].Borders.Width = 0;
        }

        /// <summary>
        /// Przygotowuje napis w postaci nazwisk dwoch teamów.
        /// </summary>
        /// <param name="players1">Zawodnicy z pokoju otwartego</param>
        /// <param name="players2">Zawodnicy z pokoju zamkniętego</param>
        /// <returns>String z odpowiednim tekstem</returns>
        protected string PrepareTableTitle(string[] players1, string[] players2)
        {
            string output;

            output = players1[(int)positions.N] + " - " + players1[(int)positions.S] + " - ";
            output += players2[(int)positions.E] + " - " + players2[(int)positions.W];
            output += napisPrzeciwko;
            output += players1[(int)positions.E] + " - " + players1[(int)positions.W] + " - ";
            output += players2[(int)positions.N] + " - " + players2[(int)positions.S];

            return output;
        }

        /// <summary>
        /// Przygotowuje prezentacje par grajacych na NS i WE w formacie NS : nazwiska. Np. NS : Abacki - Babacki
        /// </summary>
        /// <param name="table1">Nazwiska w tablicy wg schematu zgodnego z enumem positions, tak jak zmienna players w Board</param>
        /// <returns>Dwuelementową tablicę, w 0 - o NS, w 1 - o EW</returns>
        private string[] PrepareSurnames(string[] players)
        {
            string[] output = new string[4];

            output[0] = "NS : " + players[(int)positions.N] + " - " + players[(int)positions.S];
            output[1] = "EW : " + players[(int)positions.E] + " - " + players[(int)positions.W];


            return output;
        }

        /// <summary>
        /// Przygotowuje inicjaly par grajacych na NS i WE w formacie np. B/N.
        /// </summary>
        /// <param name="table1">Nazwiska w tablicy wg schematu zgodnego z enumem positions, tak jak zmienna players w Board</param>
        /// <returns>Dwuelementową tablicę, w 0 - o NS, w 1 - o EW</returns>
        private string[] PrepareInicjaly(string[] players)
        {
            string[] output = new string[2];

            output[0] = players[(int)positions.N][0] + "/" + players[(int)positions.S][0];
            output[1] = players[(int)positions.E][0] + "/" + players[(int)positions.W][0];

            return output;
        }
    }
}
