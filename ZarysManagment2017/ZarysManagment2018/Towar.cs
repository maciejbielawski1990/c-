// Decompiled with JetBrains decompiler
// Type: Zarys.Towar
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System;

namespace ZarysManagment2018
{
  public class Towar
  {
    public string nazwa_towaru { get; set; }

    public string ilosc { get; set; }

    public string cena_jednostkowa { get; set; }

    public string typ_produktu { get; set; }

    public string fi { get; set; }

    public string kolor { get; set; }

    public string nawoj { get; set; }

    public Guid ID_PRODUKT_TYPE { get; set; }

    public int ID { get; set; }

    public Towar(string nazwa_towaru_, string ilosc_, string cena)
    {
      this.nazwa_towaru = nazwa_towaru_;
      this.ilosc = "0.00";
      this.cena_jednostkowa = "0.00";
    }

    public Towar()
    {
      this.nazwa_towaru = "";
      this.ilosc = "0,00";
      this.cena_jednostkowa = "0,00";
      this.typ_produktu = "Szczecina PA6";
    }

    public override string ToString()
    {
      return this.nazwa_towaru;
    }
  }
}
