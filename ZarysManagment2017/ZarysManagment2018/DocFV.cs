// Decompiled with JetBrains decompiler
// Type: Zarys.DocFV
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System.Linq;

namespace ZarysManagment2018
{
  public class DocFV
  {
    public string nr;
    public string data_sprzedaży;
    public string data_wystawienia_faktury;
    public string termin_zaplaty;
    public string sposob_zaplaty;
    public string zamowienia;
    public string nabywca;
    public string filename;
    public string[] lista_produktow;
    public string[] lista_ilosci;
    public string[] lista_cen;
    public string[] lista_netto;
    public string[] lista_vat;
    public string[] lista_brutto;
    public string suma_netto;
    public string suma_vat;
    public string suma_brutto;

    public DocFV()
    {
    }

    public DocFV(Zamowienie zamowienie)
    {
      data_sprzedaży = zamowienie.data_sprzedazy.ToShortDateString();
      data_wystawienia_faktury = zamowienie.data_sprzedazy.ToShortDateString();
      termin_zaplaty = zamowienie.termin_zaplaty;
      sposob_zaplaty = zamowienie.sposob_zaplaty;
      nabywca = zamowienie.nabywca.dane_do_fv;
      int length = zamowienie.towary.Count<Towar>();
      lista_produktow = new string[length];
      lista_ilosci = new string[length];
      lista_cen = new string[length];
      lista_netto = new string[length];
      lista_vat = new string[length];
      lista_brutto = new string[length];
      double num1 = 0.0;
      double num2;
      for (int index1 = 0; index1 < length; ++index1)
      {
        lista_produktow[index1] = zamowienie.towary[index1].typ_produktu + zamowienie.towary[index1].nazwa_towaru;
        lista_ilosci[index1] = zamowienie.towary[index1].ilosc;
        lista_cen[index1] = zamowienie.towary[index1].cena_jednostkowa;
        double num3 = double.Parse(lista_ilosci[index1]) * double.Parse(lista_cen[index1]);
        num1 += num3;
        lista_netto[index1] = num3.ToString();
        string[] listaVat = lista_vat;
        int index2 = index1;
        num2 = num3 * 0.23;
        string str1 = num2.ToString();
        listaVat[index2] = str1;
        string[] listaBrutto = lista_brutto;
        int index3 = index1;
        num2 = num3 * 1.23;
        string str2 = num2.ToString();
        listaBrutto[index3] = str2;
      }
      suma_netto = num1.ToString();
      num2 = 0.23 * num1;
      suma_vat = num2.ToString();
      num2 = 1.23 * num1;
      suma_brutto = num2.ToString();
    }
  }
}
