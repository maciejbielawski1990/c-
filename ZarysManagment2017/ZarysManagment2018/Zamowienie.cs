// Decompiled with JetBrains decompiler
// Type: Zarys.Zamowienie
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System;
using System.Collections.Generic;

namespace ZarysManagment2018
{
  public class Zamowienie
  {
    public Wysylka wysylka;
    public List<Towar> towary;
    public DateTime czas_ostatniej_edycji;
    public DateTime data_sprzedazy;
    public string sposob_zaplaty;
    public string termin_zaplaty;
    public int idx;

    public Klient nabywca { get; set; }

    public DateTime czas_utworzenia { get; set; }

    public override string ToString()
    {
      return this.nabywca.skrot;
    }
  }
}
