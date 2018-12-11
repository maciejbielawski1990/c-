// Decompiled with JetBrains decompiler
// Type: Zarys.ClientFromDocFV
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarysManagment2018
{
  public class ClientFromDocFV
  {
    public static Klient KlientParserFromString(string nabywca, string termin, string sposob, string filename)
    {
      Klient klient1 = new Klient();
      string[] nabywca1 = nabywca.Split(new char[2]
      {
        '\n',
        '\r'
      }, StringSplitOptions.RemoveEmptyEntries);
      klient1.NIP = ClientFromDocFV.GetNIP(nabywca1);
      klient1.nr_telefonu = ClientFromDocFV.GetPhoneNumber(nabywca1);
      klient1.skrot = ClientFromDocFV.GetShortName(filename);
      klient1.dane_do_fv = "";
      foreach (string str in nabywca1)
      {
        Klient klient2 = klient1;
        klient2.dane_do_fv = klient2.dane_do_fv + str + "\r";
      }
      klient1.nazwa_firmy = nabywca1[0];
      return klient1;
    }

    private static string GetNIP(string[] nabywca)
    {
      string str = "";
      foreach (string source in nabywca)
      {
        if (source.ToUpper().Contains("NIP"))
          str = new string(((IEnumerable<char>) source.ToArray<char>()).Where<char>((Func<char, bool>) (c =>
          {
            if (!char.IsDigit(c))
              return char.IsSeparator(c);
            return true;
          })).ToArray<char>());
      }
      return str.Trim();
    }

    private static string GetPhoneNumber(string[] nabywca)
    {
      string str = "";
      foreach (string source in nabywca)
      {
          if (source.ToUpper().Contains("TEL"))
              str = new string(source.ToArray().Where(c =>
              {
                  if (!char.IsDigit(c))
                      return char.IsSeparator(c);
                  return true;
              }).ToArray<char>());
      }
      return str;
    }

    private static string GetShortName(string filename)
    {
      string str1 = "";
      string[] strArray = filename.Split('.').First<string>().Split(new char[1] {'-'}, StringSplitOptions.RemoveEmptyEntries);
      string str2 = str1 + strArray[3];
      for (int index = 4; index < strArray.Count(); ++index)
        str2 = str2 + "-" + strArray[index];
      return str2;
    }

    private static string GetAdres(string[] nabywca)
    {
      string str = "";
      for (int index = 1; index < nabywca.Length && !nabywca[index].ToUpper().Contains("ul."); ++index)
        str = str + nabywca[index] + "  ";
      return str;
    }
  }
}
