// Decompiled with JetBrains decompiler
// Type: Zarys.Sprzedaz
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System.Collections.Generic;

namespace ZarysManagment2018
{
  public class Sprzedaz
  {
    public Klient klient { get; set; }

    public string date { get; set; }

    public string nr_fv { get; set; }

    public List<Towar> towary { get; set; }

    public List<double> wart_wyrobu { get; set; }

    public double wart_uslugi { get; set; }
  }
}
