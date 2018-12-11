// Decompiled with JetBrains decompiler
// Type: Zarys.Faktura
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.RtfRendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZarysManagment2018
{
  public class Faktura
  {
    private Document document;
    private DocFV FV;
    private Zamowienie zamowienie;
    private string filename;
    private string company_shortname;
    private string nr_fv_filename;

    public Faktura(Zamowienie zam, int nr_fv)
    {
      FV = new DocFV();
      FV.data_sprzedaży = zam.data_sprzedazy.Day.ToString("D2") + "." + zam.data_sprzedazy.Month.ToString("D2") + "." + zam.data_sprzedazy.Year.ToString();
      company_shortname = zam.nabywca.skrot;
      FV.data_wystawienia_faktury = FV.data_sprzedaży;
      FV.nabywca = zam.nabywca.dane_do_fv;
      int month = zam.data_sprzedazy.Month;
      string str = month.ToString().ToCharArray().Count() != 1 ? month.ToString() : "0" + month.ToString();
      FV.nr = nr_fv.ToString("D2") + "/" + str + "/18";
      FV.termin_zaplaty = zam.termin_zaplaty;
      if (zam.termin_zaplaty != "przedpłata")
        FV.termin_zaplaty += " od daty wystawienia faktury";
      FV.sposob_zaplaty = zam.sposob_zaplaty;
      zamowienie = zam;
      double num1 = 0.0;
      int length = zam.towary.Count<Towar>();
      FV.lista_brutto = new string[length];
      FV.lista_netto = new string[length];
      FV.lista_vat = new string[length];
      for (int index = 0; index < length; ++index)
      {
        double num2 = double.Parse(zam.towary[index].cena_jednostkowa) * double.Parse(zam.towary[index].ilosc);
        num1 += num2;
        FV.lista_netto[index] = string.Format("{0:N}", (object) num2);
        FV.lista_vat[index] = string.Format("{0:N}", (object) (num2 * 0.23));
        FV.lista_brutto[index] = string.Format("{0:N}", (object) (num2 * 1.23));
        FV.suma_netto = string.Format("{0:N}", (object) num1);
        FV.suma_vat = string.Format("{0:N}", (object) (num1 * 0.23));
        FV.suma_brutto = string.Format("{0:N}", (object) (num1 * 1.23));
      }
    }

    public void Print()
    {
      document = new Document();
      document.AddSection();
      PageSetings();
      DodajNaglowek();
      DodajTabele1();
      DodajSprzedawce();
      DodajNabywce();
      DodajTowary();
      DodajNapisSlownie(FV.suma_brutto);
      DodajNapisyDolne();
      string[] strArray = this.FV.nr.Split('/');
      filename = Settings.FV_adresfolderu + Settings.FV_filename_przedrostek + strArray[0] + "-" + strArray[1] + "-" + this.company_shortname + ".doc";
      new RtfDocumentRenderer().Render(document, filename,"");
    //  Process.Start(this.filename);
    }

    private void PageSetings()
    {
      this.document.LastSection.PageSetup.LeftMargin = Unit.FromCentimeter(Settings.FV_margin_left);
      this.document.LastSection.PageSetup.RightMargin = Unit.FromCentimeter(Settings.FV_margin_right);
      this.document.LastSection.PageSetup.TopMargin = Unit.FromCentimeter(Settings.FV_margin_top);
      this.document.LastSection.PageSetup.FooterDistance = Unit.FromCentimeter(Settings.FV_footer_location);
    }

    private Font SetFont(string[] input)
    {
      Font font = new Font(input[0]);
      font.Size = Unit.FromPoint(double.Parse(input[1]));
      if (input[2] == "1")
        font.Bold = true;
      if (input[3] == "1")
        font.Italic = true;
      return font;
    }

    private void DodajNaglowek()
    {

        Paragraph paragraph1 = new Paragraph();
      paragraph1.AddFormattedText("FAKTURA NR ");
      paragraph1.AddFormattedText(this.FV.nr);
      paragraph1.Format.Font = this.SetFont(Settings.FV_napis_faktura_font);
      paragraph1.Format.Alignment = ParagraphAlignment.Center;
      document.LastSection.Add(paragraph1);

        Paragraph paragraph2 = new Paragraph();
      paragraph2.AddFormattedText("Oryginał/Kopia", this.SetFont(Settings.FV_napis_oryginal_font));
      paragraph2.Format.Alignment = ParagraphAlignment.Center;
      paragraph2.Format.SpaceAfter = Unit.FromCentimeter(Settings.FV_napis_faktura_spaceafter);
      document.LastSection.Add(paragraph2);

        Paragraph paragraph3 = new Paragraph();
      paragraph3.AddFormattedText("pieczęć sprzedawcy", this.SetFont(Settings.FV_napis_pieczec_font));
      paragraph3.Format.LeftIndent = Unit.FromCentimeter(Settings.FV_napis_pieczec_wciecie);
      paragraph3.Format.SpaceAfter = Unit.FromCentimeter(Settings.FV_napis_pieczec_spaceafter);
      document.LastSection.Add(paragraph3);
    }

    private void DodajTabele1()
    {
      Table table = new Table();
      table.AddColumn(Unit.FromCentimeter(Settings.FV_platnosci_szerkoscpierwszejkolumny));
      table.AddColumn(Unit.FromCentimeter(Settings.FV_platnosc_szerokoscdrugiejkolumny));
      table.AddRow();
      table.AddRow();
      table.AddRow();
      table.AddRow();
      table[0, 0].AddParagraph("Data sprzedaży");
      table[1, 0].AddParagraph("Data wystawienie faktury");
      table[2, 0].AddParagraph("Termin zapłaty");
      table[3, 0].AddParagraph("Sposób zapłaty");
      table.Columns[0].Format.Font = SetFont(Settings.FV_platnosc_pierwszakolumna_font);
      table[0, 1].AddParagraph("-- " + FV.data_sprzedaży + " r.");
      table[1, 1].AddParagraph("-- " + FV.data_wystawienia_faktury + " r.");
      table[2, 1].AddParagraph("-- " + FV.termin_zaplaty);
      table[3, 1].AddParagraph("-- " + FV.sposob_zaplaty);
      table.Columns[1].Format.Font = SetFont(Settings.FV_platnosc_drugakolumna_font);
      document.LastSection.Add(table);
    }

    private void DodajSprzedawce()
    {
      Paragraph paragraph = new Paragraph();
      paragraph.Format.SpaceBefore = Unit.FromCentimeter(Settings.FV_sprzedawca_space_before_napissprzedawca);
      paragraph.AddFormattedText("SPRZEDAWCA:", SetFont(Settings.FV_napis_sprzedawca_font));
      paragraph.AddLineBreak();
      paragraph.AddLineBreak();
      paragraph.AddFormattedText("\"ZARYS\" ", SetFont(Settings.FV_napis_ZARYS_font));
      paragraph.AddFormattedText("Sp. z o.o.", SetFont(Settings.FV_napis_dane_o_firmie_font));
      paragraph.AddLineBreak();
      paragraph.AddFormattedText("ul. Walczaka 25, 66-407 Gorzów Wlkp.", SetFont(Settings.FV_napis_dane_o_firmie_font));
      paragraph.AddLineBreak();
      paragraph.AddFormattedText("NIP: 955-19-18-939", SetFont(Settings.FV_napis_dane_o_firmie_font));
      paragraph.AddLineBreak();
      paragraph.AddFormattedText("tel./fax 95 731 11 06 kom. 603 088 741", SetFont(Settings.FV_napis_dane_o_firmie_font));
      paragraph.AddLineBreak();
      paragraph.AddFormattedText("Konto: Pekao S.A. V oddz. Szczecin 40 1240 3969 1111 0000 4242 1355", SetFont(Settings.FV_napis_dane_o_firmie_font));
      paragraph.Format.SpaceAfter = Unit.FromCentimeter(Settings.FV_sprzedwca_space_after);
      this.document.LastSection.Add(paragraph);
    }

    private void DodajNabywce()
    {
      Paragraph paragraph = new Paragraph();
      paragraph.AddFormattedText("NABYWCA:", SetFont(Settings.FV_napis_nabywca_font));
      paragraph.AddLineBreak();
      paragraph.AddLineBreak();
      string[] strArray = FV.nabywca.Split(new string[1]
      {
        "\n"
      }, StringSplitOptions.RemoveEmptyEntries);
      paragraph.AddFormattedText(strArray[0] + "\n", SetFont(Settings.FV_napis_dane_klienta_firstLine_font));
      for (int index = 1; index < strArray.Count(); ++index)
        paragraph.AddFormattedText(strArray[index] + "\n", SetFont(Settings.FV_napis_dane_klienta_resztaLinii_font));
      paragraph.Format.SpaceAfter = Unit.FromCentimeter(Settings.FV_tabelaTowary_spacebefore);
      this.document.LastSection.Add(paragraph);
    }

    private void DodajTowary()
    {
      Table table = new Table();
      for (int index = 0; index < 7; ++index)
        table.AddColumn(Unit.FromCentimeter(Settings.FV_tabelaTowary_szerkosckolumn[index]));
      table.AddRow();
      table.AddRow();
      table.AddRow();
      table[0, 0].AddParagraph("Nazwa towaru lub usługi");
      table[0, 1].AddParagraph("Ilość");
      table[0, 2].AddParagraph("Cena za jedn.\n[zł.]");
      table[0, 3].AddParagraph("Wartość sprzedaży netto\n[zł.]");
      table[0, 4].AddParagraph("Stawka podatku \nVAT\n[%]");
      table[0, 5].AddParagraph("Kwota podatku \nVAT\n [zł.]");
      table[0, 6].AddParagraph("Wartość sprzedaży brutto\n[zł.]");
      table.Rows[0].Format.Alignment = ParagraphAlignment.Center;
      table.Rows[0].VerticalAlignment = VerticalAlignment.Center;
      for (int index = 0; index < 7; ++index)
        table[1, index].AddParagraph("");
      foreach (Towar towar in this.zamowienie.towary)
      {
        if (towar.typ_produktu == "Zyłka PA6")
          towar.typ_produktu = "Żyłka PA6";
        table[1, 0].AddParagraph(towar.typ_produktu + " " + towar.nazwa_towaru);
        if (towar.typ_produktu == "Usługa transportowa")
          table[1, 1].AddParagraph(string.Format("{0:N}", double.Parse(towar.ilosc)) + " szt.");
        else
          table[1, 1].AddParagraph(string.Format("{0:N}", double.Parse(towar.ilosc)) + " kg");
        table[1, 2].AddParagraph(string.Format("{0:N}", double.Parse(towar.cena_jednostkowa)));
      }
      for (int index = 0; index < (FV.lista_brutto).Count(); ++index)
      {
        table[1, 3].AddParagraph(FV.lista_netto[index]);
        table[1, 4].AddParagraph("23");
        table[1, 5].AddParagraph(FV.lista_vat[index]);
        table[1, 6].AddParagraph(FV.lista_brutto[index]);
      }
      for (int index = 0; index < 7; ++index)
        table[1, index].AddParagraph("");
      table[1, 0].Format.Alignment = ParagraphAlignment.Center;
      table[1, 1].Format.Alignment = ParagraphAlignment.Right;
      table[1, 2].Format.Alignment = ParagraphAlignment.Right;
      table[1, 3].Format.Alignment = ParagraphAlignment.Right;
      table[1, 4].Format.Alignment = ParagraphAlignment.Center;
      table[1, 5].Format.Alignment = ParagraphAlignment.Right;
      table[1, 6].Format.Alignment = ParagraphAlignment.Right;
      table.Format.Font = SetFont(Settings.FV_towaryWierszDrugi_font);
      table.Rows[1].Format.RightIndent = Unit.FromMillimeter(2.0);
      Paragraph paragraph1 = new Paragraph();
      paragraph1.AddFormattedText("Razem    ", SetFont(Settings.FV_napis_Razem_font));
      table[2, 2].Add(paragraph1);
      Paragraph paragraph2 = new Paragraph();
      paragraph2.AddFormattedText(FV.suma_netto);
      table[2, 3].Add(paragraph2);
      Paragraph paragraph3 = new Paragraph();
      paragraph3.AddFormattedText("23");
      table[2, 4].Add(paragraph3);
      Paragraph paragraph4 = new Paragraph();
      paragraph4.AddFormattedText(FV.suma_vat);
      table[2, 5].Add(paragraph4);
      Paragraph paragraph5 = new Paragraph();
      paragraph5.AddFormattedText(FV.suma_brutto);
      table[2, 6].Add(paragraph5);
      table.Rows[2].Format.Alignment = ParagraphAlignment.Center;
      table.Rows[2].Format.Alignment = ParagraphAlignment.Right;
      table.Rows[2].Format.RightIndent = Unit.FromMillimeter(2.0);
      table.Rows[2].Format.Font = SetFont(Settings.FV_towaryWierszTrzeci_font);
      table[2, 4].Format.Alignment = ParagraphAlignment.Center;
      table.Rows[0].Borders.Width = (Unit) 1.0;
      table.Rows[1].Borders.Width = (Unit) 1.0;
      table[2, 0].Borders.Width = (Unit) 0.0;
      table[2, 1].Borders.Width = (Unit) 0.0;
      table[2, 2].Borders.Width = (Unit) 0.0;
      table[2, 3].Borders.Width = (Unit) 1.5;
      table[2, 4].Borders.Width = (Unit) 1.5;
      table[2, 5].Borders.Width = (Unit) 1.5;
      table[2, 6].Borders.Width = (Unit) 1.5;
      this.document.LastSection.Add(table);
    }

    private static string WykonajNapisSlownie(string pln)
    {
      double num1 = double.Parse(pln);
      string result = "";
      string[] strArray1 = new string[10]
      {
        "tysięcy",
        "tysięcy",
        "tysiące",
        "tysiące",
        "tysięce",
        "tysięcy",
        "tysięcy",
        "tysięcy",
        "tysięcy",
        "tysięcy"
      };
      string[] strArray2 = new string[10]
      {
        "złotych",
        "złotych",
        "złote",
        "złote",
        "złote",
        "złotych",
        "złotych",
        "złotych",
        "złotych",
        "złotych"
      };
      if (num1 / 1000.0 >= 1.0)
      {
        int num2 = (int) num1 / 1000;
        int index = Faktura.WykonajTrojkeSłownie((int) num1 / 1000, ref result);
        result = num2 != 1 ? result + strArray1[index] + " " : result + "tysiąc ";
        num1 -= (double) (num2 * 1000);
      }
      int index1 = Faktura.WykonajTrojkeSłownie((int) num1, ref result);
      result += strArray2[index1];
      double num3 = (double) (int) num1;
      int num4 = (int) ((num1 - num3) * 100.0 + 0.01);
      result = result + " " + num4.ToString() + "/100";
      return result;
    }

    private static int WykonajTrojkeSłownie(int input, ref string result)
    {
      string[] strArray1 = new string[10]
      {
        "",
        "sto ",
        "dwieście ",
        "trzysta ",
        "czterysta ",
        "pięćset ",
        "sześćset ",
        "siedemset ",
        "osiemset ",
        "dziewięćset "
      };
      string[] strArray2 = new string[10]
      {
        "dziesięć ",
        "jedenaście ",
        "dwanaście ",
        "trzynaście ",
        "czternaście ",
        "piętnaście ",
        "szesnaście ",
        "siedemnaście ",
        "osiemnaście ",
        "dziewiętnaście "
      };
      string[] strArray3 = new string[10]
      {
        "",
        "",
        "dwadzieścia ",
        "trzydzieści ",
        "czterdzieści ",
        "pięćdziesiąt ",
        "sześćdziesiąt ",
        "siedemdziesiąt ",
        "osiemdziesiąt ",
        "dziewięćdziesiąt "
      };
      string[] strArray4 = new string[10]
      {
        "",
        "jeden ",
        "dwa ",
        "trzy ",
        "cztery ",
        "pięć ",
        "sześć ",
        "siedem ",
        "osiem ",
        "dziewięć "
      };
      int index1 = input / 100;
      result += strArray1[index1];
      input -= index1 * 100;
      int index2 = input / 10;
      if (index2 == 1)
      {
        result += strArray2[input - index2 * 10];
        return 0;
      }
      result += strArray3[index2];
      int index3 = input - index2 * 10;
      result += strArray4[index3];
      return index3;
    }

    private void DodajNapisSlownie(string pln)
    {
      char[] charArray = Faktura.WykonajNapisSlownie(pln).ToCharArray();
      charArray[0] = (char) ((uint) charArray[0] - 32U);
      string text = new string(charArray);
      this.document.LastSection.Add(new Paragraph()
      {
        Format = {
          SpaceAfter = Unit.FromCentimeter(Settings.FV_tabelaSlownie_spacebefore)
        }
      });
      Table table = this.document.LastSection.AddTable();
      table.AddColumn(Unit.FromCentimeter(15.0));
      table.AddRow();
      Paragraph paragraph = new Paragraph();
      paragraph.AddFormattedText("Słownie złotych: ", new Font("Arial", (Unit) 9)
      {
        Italic = true
      });
      paragraph.AddFormattedText(text, new Font("Arial", (Unit) 9)
      {
        Bold = false,
        Italic = true
      });
      table[0, 0].Add(paragraph);
      table.Borders.Width = (Unit) 1.0;
    }

    private void DodajPodpis()
    {
    }

    private void DodajNapisyDolne()
    {
      Table table = new Table();
      table.AddColumn(Unit.FromCentimeter(Settings.FV_napisdolny_szerokosc_kolumny));
      table.AddColumn(Unit.FromCentimeter(Settings.FV_napisdolny_szerokosc_kolumny));
      table.AddColumn(Unit.FromCentimeter(Settings.FV_napisdolny_szerokosc_kolumny));
      table.AddRow();
      Paragraph paragraph1 = new Paragraph();
      paragraph1.AddFormattedText(Settings.FV_napisdolny_lewy, SetFont(Settings.FV_napisdolny_font));
      table[0, 0].Add(paragraph1);
      Paragraph paragraph2 = new Paragraph();
      paragraph2.AddFormattedText(Settings.FV_napisdolny_prawy, SetFont(Settings.FV_napisdolny_font));
      table[0, 2].Add(paragraph2);
      table[0, 2].Format.Alignment = ParagraphAlignment.Center;
      table[0, 0].Format.Alignment = ParagraphAlignment.Center;
      table.AddRow();
      table.AddRow();
      document.LastSection.Footers.Primary.Add(table);
    }
  }
}
