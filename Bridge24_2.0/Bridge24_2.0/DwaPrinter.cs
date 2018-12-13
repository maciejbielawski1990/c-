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

namespace Bridge24_2._0
{
    public class DwaPrinter
    {
        
            private VugraphLin vugraph1,vugraph2;
            private BBOGameReader game;
            Document document;
            const double rozklad_szerokosckolumny = 5.4;
            const double borders_szerkoscramyrozdania = 1.5;
            const double bidding_szerokosckolumny = 1.5;
            const double szerokosc_krawedzi = 0.1;
            Color kolor_tla_licytacja = Colors.LightGray;
            int[] imps1,imps2; 
            Color kolor_ramyrozdania = Colors.AntiqueWhite;
            string[] zalozenie = { "obie przed partią", "NS po partii", "EW po partii", "obie po partii" };
            Font[] znaki_kart;
            Font font_tytuly;
            Font font_normal;
            Font font_red;
            char[] suits;
            enum kolory { C, D, H, S };
            string mail;
            double[] wyniki_szerokosc_kolum = { 0.6, 1.7, 0.8, 1, 0.9, 1.5, 1.7, 0.8, 1, 0.9, 1.5, 1.54, 1.54, 1.5 };
            int[, ,] lewy;
            public DwaPrinter(VugraphLin vugraph_, VugraphLin vu2_)
            {
                vugraph1 = vugraph_;
                vugraph2 = vu2_;
                document = new Document();
                SetProperties();

            }

            public Document CreateTreningDOC()
            {

                Section section = document.AddSection();

                PageSettings(section);
                if (Ustawienia.deepfin)
                    lewy = InfoBridge.wylicz_DF(vugraph1.rozklady);
                imps1 = new int[Ustawienia.ilosc_rozdan];
                imps2 = new int[Ustawienia.ilosc_rozdan];
                imps1= wyliczimpy(vugraph1.Vu_ContractList_Open, vugraph1.Vu_ContractList_Closed);
                imps2 = wyliczimpy(vugraph2.Vu_ContractList_Open, vugraph2.Vu_ContractList_Closed);
                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    if (i != 0) document.AddSection();
                   
                    CreateBoard(i, vugraph1.rozklady[i]);
                    if (Ustawienia.deepfin)
                    {

                        CreateDF(i);
                    }

                    //pierwsza tabela licytacji
                    CreateBidding(i,vugraph1); 
                    WriteLineKontrakt(vugraph1.Vu_ContractList_Open[i], vugraph1.Vu_ContractList_Closed[i]);
                    if (imps1[i] != 0)
                    {
                        Paragraph p = new Paragraph();
                        p.AddLineBreak();
                        p.AddFormattedText("Wynik : ");
                        p.AddFormattedText(imps1[i].ToString());
                        if (imps1[i] > 0)
                        {
                            p.AddFormattedText(imps1[i].ToString());
                            p.AddFormattedText(" imp dla " + vugraph1.team1Name);
                        }
                        if (imps1[i] < 0)
                        {
                            p.AddFormattedText((-imps1[i]).ToString());
                            p.AddFormattedText(" imp dla " + vugraph1.team2Name);
                        }
                        p.AddLineBreak();
                        document.LastSection.Add(p);
                    }
                    // druga tabela licytacji
                    CreateBidding(i, vugraph2);
                    WriteLineKontrakt(vugraph2.Vu_ContractList_Open[i], vugraph2.Vu_ContractList_Closed[i]);
                    if (imps2[i] != 0)
                    {
                        Paragraph p = new Paragraph();
                        p.AddLineBreak();
                        p.AddFormattedText("Wynik : ");
                        p.AddFormattedText(imps2[i].ToString());
                        if (imps2[i] > 0)
                        {
                            p.AddFormattedText(imps2[i].ToString());
                            p.AddFormattedText(" imp dla " + vugraph2.team1Name);
                        }
                        if (imps2[i] < 0)
                        {
                            p.AddFormattedText((-imps2[i]).ToString());
                            p.AddFormattedText(" imp dla " + vugraph2.team2Name);
                        }
                        p.AddLineBreak();
                        document.LastSection.Add(p);

                    }
                }
                MakeTable33(vugraph1);
                MakeTable33(vugraph2);


                MigraDoc.DocumentObjectModel.Shapes.Image reklama = new MigraDoc.DocumentObjectModel.Shapes.Image("images\\reklama.png");

                reklama.Height = Unit.FromCentimeter(6.5);
                reklama.Width = Unit.FromCentimeter(12.0);

                reklama.WrapFormat.DistanceTop = Unit.FromCentimeter(1.0);
                reklama.WrapFormat.DistanceLeft = Unit.FromCentimeter(2.0);

                document.LastSection.Add(reklama);
                document.UseCmykColor = true;


                string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);


                // RtfDocumentRenderer renderer = new RtfDocumentRenderer();
                // renderer.Render(document, "Test.doc", null);

                // Process.Start("Test.doc");

                return document;
            }


            void CreateDF(int nr)
            {
                Paragraph px = new Paragraph();
                px.AddLineBreak();
                px.AddLineBreak();
                document.LastSection.Add(px);
                Table table = document.LastSection.AddTable();
                //  int[, ,] lewy = InfoBridge.wylicz_DF(vugraph.rozklady);

                for (int i = 0; i < 13; i++)
                {
                    table.AddColumn(Unit.FromCentimeter(1.29));
                }

                for (int i = 0; i < 3; i++)
                {
                    table.AddRow();
                }


                table[1, 0].AddParagraph("N");
                table[2, 0].AddParagraph("S");
                table[1, 7].AddParagraph("E");
                table[2, 7].AddParagraph("W");

                for (int i = 2; i < 6; i++)
                {
                    Paragraph p = new Paragraph();
                    p.AddFormattedText(suits[6 - i].ToString(), znaki_kart[6 - i]);

                    table[0, i].Add(p);
                    table[1, i].AddParagraph(lewy[nr, 0, 5 - i].ToString());
                    table[2, i].AddParagraph(lewy[nr, 1, 5 - i].ToString());
                }
                table[0, 1].AddParagraph("NT");
                table[1, 1].AddParagraph(lewy[nr, 0, 4].ToString());
                table[2, 1].AddParagraph(lewy[nr, 1, 4].ToString());

                for (int i = 9; i < 13; i++)
                {
                    Paragraph p = new Paragraph();
                    p.AddFormattedText(suits[13 - i].ToString(), znaki_kart[13 - i]);
                    table[0, i].Add(p);
                    table[1, i].AddParagraph(lewy[nr, 2, 12 - i].ToString());
                    table[2, i].AddParagraph(lewy[nr, 3, 12 - i].ToString());
                }
                table[0, 8].AddParagraph("NT");
                table[1, 8].AddParagraph(lewy[nr, 2, 4].ToString());
                table[2, 8].AddParagraph(lewy[nr, 3, 4].ToString());
                Row row = table.Rows[0];
                row.Borders.Bottom.Width = 1.0;

                Column col = table.Columns[0];

                col.Borders.Right.Width = 1.0;
                row.Borders.Right.Width = 0;
                table[0, 0].Borders.Right.Width = 1.0;
                table[0, 6].Borders.Bottom.Width = 0;


                col = table.Columns[7];

                col.Borders.Right.Width = 1.0;
                table[0, 7].Borders.Right.Width = 1.0;

                table.Format.Alignment = ParagraphAlignment.Center;
                table.Rows.VerticalAlignment = VerticalAlignment.Center;

                table.Rows.Height = Unit.FromCentimeter(0.7);
            }
            private void SetProperties()
            {
                mail = "maciej.bielawski@bridge24.pl";


                znaki_kart = new Font[5];
                for (int i = 0; i < 5; i++)
                {
                    znaki_kart[i] = new Font();
                }
                //trefl
                znaki_kart[1].Color = Colors.Green;
                //karo
                znaki_kart[2].Color = Colors.Orange;
                znaki_kart[3].Color = Colors.Red;
                znaki_kart[4].Color = Colors.Black;

                int piczek = 9824;
                int kierek = 9829;
                int karko = 9830;
                int trefelek = 9827;

                suits = new char[5];

                suits[1] = (char)trefelek;
                suits[2] = (char)karko;
                suits[3] = (char)kierek;
                suits[4] = (char)piczek;

                font_tytuly = new Font();

                font_tytuly.Name = "Cambria";
                font_tytuly.Size = 10;
                font_tytuly.Underline = Underline.Single;
                font_tytuly.Color = Colors.Blue;
                font_tytuly.Bold = true;

                font_normal = new Font();
                font_normal.Name = "Cambria";
                font_normal.Size = 8;

                font_red = new Font();
                font_red.Name = "Cambria";
                font_red.Size = 8;
                font_red.Color = Colors.Red;
            }

            private void PageSettings(Section section)
            {


                Paragraph naglowek = section.Headers.Primary.AddParagraph();
                naglowek.Format.Font.Underline = Underline.Single;

                naglowek.AddFormattedText(vugraph1.tytul);
                string tekst = vugraph1.tytul;

                if (tekst.Count() > 40)
                    tekst = tekst.Substring(0, 40);

                naglowek.AddSpace(40 - vugraph1.tytul.Count());
                naglowek.AddImage("images\\mini_b24dwa.jpg");
                naglowek.AddSpace(15);
                naglowek.AddFormattedText(DateTime.Now.ToShortDateString());


                Paragraph stopka = section.Footers.Primary.AddParagraph();
                stopka.Format.Borders.Top.Width = Unit.FromCentimeter(0.1);
                stopka.AddFormattedText(mail);
                stopka.AddTab(); stopka.AddTab(); stopka.AddTab(); stopka.AddTab(); stopka.AddTab(); stopka.AddTab();
                stopka.AddSpace(4);
                stopka.Format.Alignment = ParagraphAlignment.Justify;
                stopka.AddFormattedText("Strona ");
                stopka.AddPageField();
                stopka.AddFormattedText(" z " + (Ustawienia.ilosc_rozdan + 1).ToString());
            }


            #region prezentacja rozkladu

            private void CreateBoard(int nr, RozkladKart rozklad)
            {
                Section section = document.LastSection;

                Table table = new Table();
                table.Borders.Width = 0;

                Column column1 = table.AddColumn(Unit.FromCentimeter(rozklad_szerokosckolumny));
                Column column2 = table.AddColumn(Unit.FromCentimeter(rozklad_szerokosckolumny));
                Column column3 = table.AddColumn(Unit.FromCentimeter(rozklad_szerokosckolumny));

                table.Format.Alignment = ParagraphAlignment.Justify;

                /////////////////////////////////N//////////////////////////////
                Row row = table.AddRow();
                Paragraph p = row.Cells[1].AddParagraph();
                zrob_rozklad(rozklad.N, p);
                p.AddLineBreak();

                /////////////////////////////////W//////////////////////////////

                Row row2 = table.AddRow();
                p = row2[0].AddParagraph();
                zrob_rozklad(rozklad.W, p);

                /////////////////////////////////E//////////////////////////////
                p = row2[2].AddParagraph();
                zrob_rozklad(rozklad.E, p);


                /////////////////////////////////////S//////////////////////////////////////////////
                Row row3 = table.AddRow();
                p = row3[1].AddParagraph();
                zrob_rozklad(rozklad.S, p);

                Cell cell = new Cell();
                cell = row2.Cells[1];
                cell.Borders.Width = borders_szerkoscramyrozdania;
                cell.Shading.Color = kolor_ramyrozdania;

                cell = zrob_ramke(cell, (nr + 1));



                table.SetEdge(0, 0, 3, 1, Edge.Box, BorderStyle.None, 0.0, Colors.White);
                document.LastSection.Add(table);
            }

            private void zrob_rozklad(Karty karty, Paragraph p)
            {
                p = zrob_kolor(4, p, karty.piki);

                p = zrob_kolor(3, p, karty.kiery);
                p = zrob_kolor(2, p, karty.kara);

                p = zrob_kolor(1, p, karty.trefle);

                p.AddLineBreak();
            }

            private Paragraph zrob_kolor(int idx, Paragraph p, string karty)
            {
                p.AddLineBreak();
                p.AddTab(); p.AddSpace(5);

                p.AddFormattedText(suits[idx].ToString(), znaki_kart[idx]);


                p.AddText(" " + karty);

                return p;
            }

            private Cell zrob_ramke(Cell cell, int nr)
            {


                Paragraph naglowek1 = new Paragraph();
                naglowek1 = cell.AddParagraph();
                naglowek1.Format.Alignment = ParagraphAlignment.Center;
                string rozdanie_string = "Rozdanie nr " + nr;
                naglowek1.Format.Font.Bold = true;
                naglowek1.Format.Font.Name = "Cambria";
                naglowek1.Format.Font.Size = 11;
                naglowek1.AddText(rozdanie_string);

                naglowek1.AddLineBreak(); naglowek1.AddLineBreak();


                Paragraph naglowek2 = new Paragraph();
                naglowek2 = cell.AddParagraph();
                naglowek2.Format.Font.Bold = true;
                naglowek2.Format.Font.Name = "Cambria";
                naglowek2.Format.Alignment = ParagraphAlignment.Center;
                naglowek2.Format.Font.Size = 6;
                string zal_ = "";
                if (InfoBridge.NS_partia[nr] == 0 && InfoBridge.WE_partia[nr] == 0) zal_ = zalozenie[0];
                if (InfoBridge.NS_partia[nr] == 1 && InfoBridge.WE_partia[nr] == 0) zal_ = zalozenie[1];
                if (InfoBridge.NS_partia[nr] == 0 && InfoBridge.WE_partia[nr] == 1) zal_ = zalozenie[2];
                if (InfoBridge.NS_partia[nr] == 1 && InfoBridge.WE_partia[nr] == 1) zal_ = zalozenie[3];
                naglowek2.AddText(zal_);

                cell.VerticalAlignment = VerticalAlignment.Center;


                return cell;
            }

            #endregion


            #region prezentacja licytacji

            private Row CreateBidding_Surname(Row row,VugraphLin vugraph)
            {
                row.Format.Font.Size = 7;

                //wydruk nazwisk pokoj otwarty

                row.Cells[0].AddParagraph(vugraph.nazwiska_[0]);
                row.Cells[1].AddParagraph(vugraph.nazwiska_[1]);
                row.Cells[2].AddParagraph(vugraph.nazwiska_[2]);
                row.Cells[3].AddParagraph(vugraph.nazwiska_[3]);

                //wydrik nazwisk pokoj zamkniety
                row.Cells[6].AddParagraph(vugraph.nazwiska_[4].ToString());
                row.Cells[7].AddParagraph(vugraph.nazwiska_[5].ToString());
                row.Cells[8].AddParagraph(vugraph.nazwiska_[6].ToString());
                row.Cells[9].AddParagraph(vugraph.nazwiska_[7].ToString());

                return row;
            }

            private Row CreateBidding_FirstRow(Row row)
            {


                //open room
                row.Cells[0].AddParagraph("N");
                row.Cells[1].AddParagraph("E");
                row.Cells[2].AddParagraph("S");
                row.Cells[3].AddParagraph("W");

                //closed room
                row.Cells[6].AddParagraph("N");
                row.Cells[7].AddParagraph("E");
                row.Cells[8].AddParagraph("S");
                row.Cells[9].AddParagraph("W");


                row.Shading.Color = kolor_tla_licytacja;

                return row;
            }

            private Paragraph WriteOdzywka(Paragraph p, string odzywka)
            {

                if (odzywka.Count() > 1)
                {
                    p.AddText(odzywka[0].ToString());

                    char suit = odzywka[1];
                    if (suit == 'C') p.AddFormattedText(suits[1].ToString(), znaki_kart[1]);
                    if (suit == 'D') p.AddFormattedText(suits[2].ToString(), znaki_kart[2]);
                    if (suit == 'H') p.AddFormattedText(suits[3].ToString(), znaki_kart[3]);
                    if (suit == 'S') p.AddFormattedText(suits[4].ToString(), znaki_kart[4]);
                    if (suit.ToString() == "N") p.AddText("BA");
                }
                else
                {
                    if (odzywka.ToUpper() == "D")
                    {
                        p.AddText("ktr");
                    }
                    if (odzywka.ToUpper() == "R")
                    {
                        p.AddText("rktr");
                    }
                    if (odzywka.ToUpper() == "P")
                    {
                        p.AddText("pas");
                    }
                }
                return p;
            }

            private void CreateBidding(int nr,VugraphLin vugraph)
            {
                Section section = document.LastSection;
                Paragraph tp = section.AddParagraph();
                tp.AddLineBreak();
                tp.AddLineBreak();
                string naglowek = "Licytacja z meczu " + vugraph.team1Name + " przeciwko " + vugraph.team2Name;

                tp.AddFormattedText(naglowek, font_tytuly);
                tp.AddLineBreak();
                tp.AddLineBreak();

                Table table = new Table();
                table.Format.Font.Name = "Cambria";
                table.Format.Font.Size = 8;
                table.Format.Alignment = ParagraphAlignment.Center;


                for (int i = 0; i < 10; i++)
                {
                    Column col = table.AddColumn(Unit.FromCentimeter(bidding_szerokosckolumny));
                    col.Borders.Width = szerokosc_krawedzi;
                    if ((i == 4 || i == 5))
                    {
                        col.Borders.Width = 0;
                    }
                }
                Row row = table.AddRow();
                row = CreateBidding_FirstRow(row);
                row = table.AddRow();
                row = CreateBidding_Surname(row,vugraph);

                row = table.AddRow();

                int idx = 1;
                while (vugraph.licytacja_open[nr][idx] != null)
                {
                    if ((idx - 1 + nr) % 4 == 0)
                    {
                        if (idx != 1) row = table.AddRow();
                        Paragraph p = row.Cells[(idx - 1 + nr) % 4].AddParagraph();
                        p = WriteOdzywka(p, vugraph.licytacja_open[nr][idx]);
                    }

                    else
                    {
                        Paragraph p = row.Cells[(idx - 1 + nr) % 4].AddParagraph();

                        p = WriteOdzywka(p, vugraph.licytacja_open[nr][idx]);

                    }
                    idx++;
                }

                //licytacja zamkniety
                idx = 1;
                int wiersz;
                wiersz = 1;
                while (vugraph.licytacja_closed[nr][idx] != null)
                {
                    int tmp = (idx - 1 + nr) / 4 + 3;
                    if ((idx - 1 + nr) % 4 == 0 || idx == 1)
                        wiersz++;
                    if (wiersz >= table.Rows.Count)
                    {
                        row = table.AddRow();
                    }

                    row = table.Rows[wiersz];
                    Paragraph p = row[((idx - 1 + nr) % 4) + 6].AddParagraph();
                    p = WriteOdzywka(p, vugraph.licytacja_closed[nr][idx]);

                    idx++;
                }

                table.Rows.Height = Unit.FromCentimeter(0.6);
                table.Rows.VerticalAlignment = VerticalAlignment.Center;

                table.SetEdge(0, 0, 10, 1, Edge.Box, BorderStyle.Single, 1.0, Colors.Black);

                table[0, 4].Borders.Bottom.Width = 0;
                table[0, 5].Borders.Bottom.Width = 0;
                table[0, 4].Borders.Top.Width = 0;
                table[0, 5].Borders.Top.Width = 0;
                table[0, 4].Borders.Right.Width = 0;
                table[0, 4].Shading.Color = Colors.White;
                table[0, 5].Shading.Color = Colors.White;

                document.LastSection.Add(table);

            }

            #endregion

            #region komentarze

            private void WriteLineKontrakt(InfoBoard board, InfoBoard board2)
            {
                Section section = document.LastSection;
                Paragraph p = section.AddParagraph();
                p.Format.Font = font_normal.Clone();
                p.AddLineBreak();
                p.AddFormattedText("Kontrakt  ");
                p = WriteContract(p, board);
                p.AddFormattedText(" ");
                p = WriteTricks(p, board.lew);
                p.AddFormattedText(", ");
                p = WriteLead(p, board);
                p.AddSpace(6);

                p = WriteScore(p, board.score);
                p.AddSpace(29);
                p.AddFormattedText("Kontrakt  ");
                p = WriteContract(p, board2);
                p.AddFormattedText(" ");
                p = WriteTricks(p, board2.lew);
                p.AddFormattedText(", ");
                p = WriteLead(p, board2);
                p.AddSpace(6);
                p = WriteScore(p, board2.score);

            }
            private void WriteCommentTitle()
            {
                Section section = document.LastSection;
                Paragraph komentarze = section.AddParagraph();
                komentarze.Format.Font = font_tytuly.Clone();
                komentarze.AddLineBreak();
                komentarze.AddLineBreak();
                komentarze.AddLineBreak();
                komentarze.AddFormattedText("WYJAŚNIENIE ODZYWEK KONWENCYJNYCH :");
                komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddFormattedText("KOMENTARZ DO LICYTACJI :");
                komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddFormattedText("KOMENTARZ DO ROZGRYWKI :");
                komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddFormattedText("KOMENTARZ DO WISTU :");
                komentarze.AddLineBreak(); komentarze.AddLineBreak(); komentarze.AddLineBreak();
                komentarze.AddLineBreak(); komentarze.AddLineBreak();
            }

            #endregion

            #region tabele z wynikami


            Paragraph WriteTableTitle(string team1, string team2, int nr)
            {
                Paragraph p = new Paragraph();
                p.Format.Font.Color = new Color(192, 80, 72);
                p.Format.Font.Bold = true;
                p.Format.Font.Name = "Cambria";
                p.Format.Font.Size = 8;
                p.AddLineBreak();
                p.AddLineBreak();
                p.AddFormattedText("Mecz nr " + nr.ToString() + " :  ");
                p.AddFormattedText(team1);
                p.AddSpace(5);
                p.AddFormattedText("przeciwko");
                p.AddSpace(5);
                p.AddFormattedText(team2);
                p.AddLineBreak();
                p.AddLineBreak();
                p.AddLineBreak();


                return p;
            }

            void TableProperties(ref Table table)
            {

                table.Format.Font.Name = "Cambria";
                table.Format.Font.Size = 8;
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

         /*   void MakeTable22()
            {
                Table table = new Table();
                for (int i = 0; i < 14; i++)
                {
                    table.AddColumn(Unit.FromCentimeter(wyniki_szerokosc_kolum[i]));
                }

                int[] imp = wyliczimpy(game.ContractList, vugraph.Vu_ContractList_Closed);


                string s1 = "NS : " + game.nazwiska_[2] + " - " + game.nazwiska_[0];
                string s2 = "EW : " + game.nazwiska_[3] + " - " + game.nazwiska_[1];
                string s3 = "NS : " + vugraph.nazwiska_[6] + " - " + vugraph.nazwiska_[4];
                string s4 = "EW : " + vugraph.nazwiska_[7] + " - " + vugraph.nazwiska_[5];
                string s5 = "Mecz treningowy";
                string napis1 = "Pokój zamknięty";

                MakeTableRow1(table, s1, s2, s3, s4);

                string inicjaly1 = game.nazwiska_[2][0] + "/" + game.nazwiska_[0][0] + " + ";
                inicjaly1 += vugraph.nazwiska_[7][0] + "/" + vugraph.nazwiska_[5][0];

                string inicjaly2 = game.nazwiska_[3][0] + "/" + game.nazwiska_[1][0] + " + ";
                inicjaly2 += vugraph.nazwiska_[6][0] + "/" + vugraph.nazwiska_[4][0];

                MakeTableRow2(table, napis1, inicjaly1, inicjaly2, s5);

                MakeTableRow3(table);

                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    table = MakeScoresGame(table, i, game.ContractList[i], vugraph.Vu_ContractList_Closed[i]);
                    AddImp(ref table, imp, i);
                }
                TableProperties(ref table);
                table.SetEdge(0, 2, 14, 17, Edge.Box, BorderStyle.Single, 1.0, Colors.Black);

                string team1 = game.nazwiska_[0] + " - " + game.nazwiska_[2] + " - " + vugraph.nazwiska_[5] + " - " + vugraph.nazwiska_[7];
                string team2 = game.nazwiska_[1] + " - " + game.nazwiska_[3] + " - " + vugraph.nazwiska_[4] + " - " + vugraph.nazwiska_[6];
                int nr_meczu = 2;

                WstawOstatniaLinie(table, imp);

                document.AddSection();
                document.LastSection.Add(WriteTableTitle(team1, team2, nr_meczu));
                document.LastSection.Add(table);
            }

            void WstawOstatniaLinie(Table table, int[] imp)
            {
                Row row2 = table.AddRow();
                int impsns = 0;
                int impsew = 0;
                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    if (imp[i] > 0)
                        impsns += imp[i];
                    else
                        impsew -= imp[i];
                }
                row2.Borders.Left.Width = 0;
                row2.Borders.Right.Width = 0;
                row2.Borders.Bottom.Width = 0;
                row2[11].Borders.Left.Width = 1.0;
                row2[11].Borders.Right.Width = 1.0;
                row2[11].Borders.Bottom.Width = 1.0;
                row2[12].Borders = row2[11].Borders.Clone();
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Cells[11].AddParagraph(impsns.ToString());
                row2.Cells[12].AddParagraph(impsew.ToString());
            }

            void MakeTable11()
            {
                Table table = new Table();
                for (int i = 0; i < 14; i++)
                {
                    table.AddColumn(Unit.FromCentimeter(wyniki_szerokosc_kolum[i]));
                }

                int[] imp = wyliczimpy(game.ContractList, vugraph.Vu_ContractList_Open);


                string s1 = "NS : " + game.nazwiska_[2] + " - " + game.nazwiska_[0];
                string s2 = "EW : " + game.nazwiska_[3] + " - " + game.nazwiska_[1];
                string s3 = "NS : " + vugraph.nazwiska_[2] + " - " + vugraph.nazwiska_[0];
                string s4 = "EW : " + vugraph.nazwiska_[3] + " - " + vugraph.nazwiska_[1];
                string napis1 = "Pokój otwarty";


                MakeTableRow1(table, s1, s2, s3, s4);

                string inicjaly1 = game.nazwiska_[2][0] + "/" + game.nazwiska_[0][0] + " + ";
                inicjaly1 += vugraph.nazwiska_[3][0] + "/" + vugraph.nazwiska_[1][0];

                string inicjaly2 = game.nazwiska_[3][0] + "/" + game.nazwiska_[1][0] + " + ";
                inicjaly2 += vugraph.nazwiska_[2][0] + "/" + vugraph.nazwiska_[0][0];

                MakeTableRow2(table, napis1, inicjaly1, inicjaly2, "Mecz treningowy");

                MakeTableRow3(table);

                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    table = MakeScoresGame(table, i, game.ContractList[i], vugraph.Vu_ContractList_Closed[i]);
                    AddImp(ref table, imp, i);
                }
                TableProperties(ref table);
                table.SetEdge(0, 2, 14, 17, Edge.Box, BorderStyle.Single, 1.0, Colors.Black);

                string team1 = game.nazwiska_[0] + " - " + game.nazwiska_[2] + " - " + vugraph.nazwiska_[1] + " - " + vugraph.nazwiska_[3];
                string team2 = game.nazwiska_[1] + " - " + game.nazwiska_[3] + " - " + vugraph.nazwiska_[0] + " - " + vugraph.nazwiska_[2];
                int nr_meczu = 1;

                WstawOstatniaLinie(table, imp);

                document.AddSection();

                document.LastSection.Add(WriteTableTitle(team1, team2, nr_meczu));
                document.LastSection.Add(table);
            }
        */
            void MakeTable33(VugraphLin vu)
            {
                Table table = new Table();
                for (int i = 0; i < 14; i++)
                {
                    table.AddColumn(Unit.FromCentimeter(wyniki_szerokosc_kolum[i]));
                }

                int[] imp = wyliczimpy(vu.Vu_ContractList_Open, vu.Vu_ContractList_Closed);


                string s1 = "NS : " + vu.nazwiska_[2] + " - " + vu.nazwiska_[0];
                string s2 = "EW : " + vu.nazwiska_[3] + " - " + vu.nazwiska_[1];
                string s3 = "NS : " + vu.nazwiska_[6] + " - " + vu.nazwiska_[4];
                string s4 = "EW : " + vu.nazwiska_[7] + " - " + vu.nazwiska_[5];
                string napis1 = "Pokój otwarty";


                MakeTableRow1(table, s1, s2, s3, s4);

                string inicjaly1 = vu.nazwiska_[2][0] + "/" + vu.nazwiska_[0][0] + " + ";
                inicjaly1 += vu.nazwiska_[7][0] + "/" + vu.nazwiska_[5][0];

                string inicjaly2 = vu.nazwiska_[3][0] + "/" + vu.nazwiska_[1][0] + " + ";
                inicjaly2 += vu.nazwiska_[6][0] + "/" + vu.nazwiska_[4][0];

                MakeTableRow2(table, napis1, inicjaly1, inicjaly2, "Mecz treningowy");

                MakeTableRow3(table);

                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    table = MakeScoresGame(table, i, vu.Vu_ContractList_Open[i], vu.Vu_ContractList_Closed[i]);
                    AddImp(ref table, imp, i);
                }
                TableProperties(ref table);
                table.SetEdge(0, 0, 14, 2, Edge.Box, BorderStyle.Single, 1.0, Colors.Black);

                string team1 = vu.nazwiska_[0] + " - " + vu.nazwiska_[2] + " - " + vu.nazwiska_[5] + " - " + vu.nazwiska_[7];
                string team2 = vu.nazwiska_[1] + " - " + vu.nazwiska_[3] + " - " + vu.nazwiska_[4] + " - " + vu.nazwiska_[6];
                int nr_meczu = 1;

                //WstawOstatniaLinie(table, imp);
                document.AddSection();
                document.LastSection.Add(WriteTableTitle(team1, team2, nr_meczu));
                document.LastSection.Add(table);
            }



            void AddImp(ref Table table, int[] imp, int i)
            {
                if (imp[i] > 0)
                    table.Rows[i + 3].Cells[11].AddParagraph(imp[i].ToString());
                else
                {
                    if (imp[i] != 0)
                        table.Rows[i + 3].Cells[12].AddParagraph((-imp[i]).ToString());
                    else
                    {
                        table.Rows[i + 3].Cells[11].AddParagraph("-");
                        table.Rows[i + 3].Cells[12].AddParagraph("-");
                    }
                }
            }

            Table MakeTableRow1(Table table, string s1, string s2, string s3, string s4)
            {
                Row row = table.AddRow();
                Paragraph p = row.Cells[1].AddParagraph();
                Font f = new Font();
                f.Bold = true;
                f.Color = new Color(79, 129, 189);

                p.AddFormattedText(s1, f);
                p.AddLineBreak();
                p.AddFormattedText(s2, f);

                row.Cells[1].MergeRight = 4;

                p = row.Cells[6].AddParagraph();

                p.AddFormattedText(s3, f);
                p.AddLineBreak();
                p.AddFormattedText(s4, f);


                row.Cells[6].MergeRight = 4;

                row.Cells[11].Format.Font = f.Clone();
                row.Cells[11].AddParagraph("Team 1");
                row.Cells[12].Format.Font = f.Clone();
                row.Cells[12].AddParagraph("Team 2");

                return table;
            }

            Table MakeTableRow2(Table table, string s2, string s3, string s4, string s5)
            {
                Row row = table.AddRow();

                row.Format.Font.Bold = true;

                row.Cells[1].AddParagraph(s5);
                row.Cells[1].MergeRight = 4;

                row.Cells[6].AddParagraph(s2);
                row.Cells[6].MergeRight = 4;

                row.Cells[11].AddParagraph(s3);
                row.Cells[12].AddParagraph(s4);

                return table;
            }

            Table MakeTableRow3(Table table)
            {
                Row row = table.AddRow();
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

                return table;
            }

            private int[] wyliczimpy(List<InfoBoard> table1, List<InfoBoard> table2)
            {
                int[] mecz = new int[Ustawienia.ilosc_rozdan + 1];

                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    int saldo = table1[i].score - table2[i].score;
                    mecz[i] = InfoBridge.wylicz_impy(Math.Abs(saldo));
                    if (saldo < 0)
                        mecz[i] *= -1;
                }

                return mecz;
            }

            Table MakeScoresGame(Table table, int i, InfoBoard table1, InfoBoard table2)
            {
                Row row = table.AddRow();

                Paragraph p = row.Cells[0].AddParagraph();
                p.AddFormattedText((i + 1).ToString());

                p = row.Cells[1].AddParagraph();
                p = WriteContract(p, table1);

                row.Cells[2].AddParagraph(table1.declarer);
                p = row.Cells[3].AddParagraph();
                WriteLead(p, table1);

                p = row.Cells[4].AddParagraph();
                if (table1.lew != null)
                    WriteTricks(p, table1.lew);

                Paragraph score = row.Cells[5].AddParagraph();
                WriteScore(score, table1.score);

                p = row.Cells[6].AddParagraph();
                WriteContract(p, table2);

                row.Cells[7].AddParagraph(table2.declarer);
                p = row.Cells[8].AddParagraph();
                WriteLead(p, table1);

                p = row.Cells[9].AddParagraph();
                if (table2.lew != null)
                    WriteTricks(p, table2.lew);

                score = row.Cells[10].AddParagraph();
                WriteScore(score, table2.score);




                return table;
            }



            #endregion

            #region WrittingShort

            Paragraph WriteLead(Paragraph p, InfoBoard board)
            {
                if (board.wist != null)
                {
                    p.AddFormattedText(board.wist[1].ToString());

                    WriteSuit(p, board.wist[0].ToString().ToUpper());


                }


                return p;
            }

            Paragraph WriteContract(Paragraph p, InfoBoard board)
            {
                if (board.level > 0)
                {
                    p.AddFormattedText(board.level.ToString());

                    WriteSuit(p, board.suit);

                    if (board.kontra) p.AddFormattedText("x");
                    if (board.rekontra) p.AddFormattedText("x");
                }
                else
                {
                    p.AddFormattedText("4 pasy");
                }

                return p;
            }

            Paragraph WriteSuit(Paragraph p, string suit)
            {

                if (suit == "C") p.AddFormattedText(suits[1].ToString(), znaki_kart[1]);
                if (suit == "D") p.AddFormattedText(suits[2].ToString(), znaki_kart[2]);
                if (suit == "H") p.AddFormattedText(suits[3].ToString(), znaki_kart[3]);
                if (suit == "S") p.AddFormattedText(suits[4].ToString(), znaki_kart[4]);
                if (suit == "N") p.AddFormattedText("BA");

                return p;
            }

            Paragraph WriteTricks(Paragraph p, string lew)
            {
                if (lew != "=")
                {
                    if ((int.Parse(lew)) > 0)
                    {
                        p.AddFormattedText("+" + lew);
                    }
                    else
                    {
                        p.AddFormattedText(lew);
                    }
                }
                else
                {
                    p.AddFormattedText("=");
                }

                return p;
            }

            Paragraph WriteScore(Paragraph p, int score)
            {
                if (score < 0)
                {
                    p.AddFormattedText(score.ToString(), font_red);
                }
                else
                {
                    p.AddFormattedText(score.ToString());
                }

                return p;
            }

            #endregion


        }


}

