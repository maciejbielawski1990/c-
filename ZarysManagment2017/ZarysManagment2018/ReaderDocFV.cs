// Decompiled with JetBrains decompiler
// Type: Zarys.ReaderDocFV
// Assembly: Zarys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBBB23A0-D780-4F7D-9295-037A3957A614
// Assembly location: F:\Program do faktur\Zarys2018ver2.exe

using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ZarysManagment2018
{
  public static class ReaderDocFV
  {
    public static List<DocFV> Make(string[] files)
    {
      List<DocFV> docFvList = new List<DocFV>();
        Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word.Document doc;
            List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
        int num = files.Count();
      for (int index = 0; index < num; ++index)
      {
        try
        {
          doc = winword.Documents.Open(files[index]);

            List<string> list = ((IEnumerable<string>)doc.Content.Text.Replace('\a', ' ').Split('\r')).ToList<string>();
                    int count = doc.Tables.Count;
          string nr = ReaderDocFV.FindFVNumber(ref list).Trim();
          string nabywca1 = ReaderDocFV.FindNabywca(ref list);
          string[] strArray = ReaderDocFV.ReadFirstTable(doc.Tables[1]);
          List<List<string>> stringListList = ReaderDocFV.ReadSecondTable(doc.Tables[2]);
          string nabywca2 = nabywca1;
          string[] dane = strArray;
          List<List<string>> data = stringListList;
          DocFV docFv = ReaderDocFV.ReadDocFV(nr, nabywca2, dane, data);
          docFv.filename = files[index];
          docFvList.Add(docFv);
     
          doc.Close();
          stringList2.Add("Przeczytano plik " + files[index]);
        }
        catch
        {
          stringList1.Add("Błąd czytania pliku (ReaderDocFV.Make) " + files[index]);
        }
      }
    
      winword.Quit();
      StreamWriter streamWriter1 = new StreamWriter("errors\\WordErrors.txt");
      foreach (string str in stringList1)
        streamWriter1.WriteLine(str);
      streamWriter1.Close();
      StreamWriter streamWriter2 = new StreamWriter("errors\\WordRaport.txt");
      foreach (string str in stringList2)
        streamWriter2.WriteLine(str);
      streamWriter2.Close();
      return docFvList;
    }

    private static DocFV ReadDocFV(string nr, string nabywca, string[] dane, List<List<string>> data)
    {
      DocFV docFv = new DocFV();
      docFv.nr = nr;
      docFv.nabywca = nabywca;
      docFv.data_sprzedaży = dane[0].Trim();
      docFv.data_wystawienia_faktury = dane[0].Trim();
      docFv.termin_zaplaty = dane[2];
      docFv.sposob_zaplaty = dane[3];
      char[] separator = new char[1]{ '\r' };
      docFv.lista_produktow = data[1][0].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.lista_ilosci = data[1][1].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.lista_cen = data[1][2].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.lista_netto = data[1][3].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.lista_vat = data[1][5].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.lista_brutto = data[1][6].Split(separator, StringSplitOptions.RemoveEmptyEntries);
      docFv.suma_netto = data[2][1].Trim();
      docFv.suma_vat = data[2][3].Trim();
      docFv.suma_brutto = data[2][4].Trim();
      return docFv;
    }

    private static string FindFVNumber(ref List<string> dane)
    {
      int num1 = 0;
      do
            { }
      while (!dane[num1++].ToUpper().Contains("FAKTURA"));
      List<string> list = ((IEnumerable<string>) dane[num1 - 1].Split(' ')).ToList<string>();
      int num2 = 0;
      int num3 = 0;
      while (num3 < 3)
      {
        if (list[num2++] != "")
          ++num3;
      }
      return list[num2 - 1];
    }

    private static string FindNabywca(ref List<string> dane)
    {
      string str = "";
      int num1 = 0;
        do
        {
        }
      while (!dane[num1++].ToUpper().Contains("NABYWCA"));
      int num2 = num1;
        do
        {
        }
      while (!dane[num1++].ToUpper().Contains("NAZWA"));
      int num3 = num1 - 1;
      for (int index = num2; index < num3; ++index)
        str = str + dane[index].ToString() + "\n";
      return str.Replace('\v', '\r');
    }

    private static string[] ReadFirstTable(Table table)
    {
      string[] strArray1 = new string[4];
      char[] chArray = new char[3]{ '-', '\a', '\r' };
      for (int index = 0; index < 4; ++index)
      {
        string str = table.Rows[index + 1].Cells[2].ToString();
        strArray1[index] = str.Trim(chArray);
      }
      strArray1[0] = strArray1[0].Replace("r.", "");
      strArray1[1] = strArray1[1].Replace("r.", "");
      string[] strArray2 = strArray1[2].Split(' ');
      int index1 = 0;
      while (strArray2[index1] == "")
        ++index1;
      strArray1[2] = strArray2[index1];
      strArray1[3] = strArray1[3].Trim();
      return strArray1;
    }

    private static List<List<string>> ReadSecondTable(Table table)
    {
      List<List<string>> stringListList = new List<List<string>>();
      foreach (Row row in table.Rows)
      {
        List<string> stringList = new List<string>();
        for (int index = 1; index <= row.Cells.Count; ++index)
        {
          if (index <= 4 || index >= 9)
            stringList.Add(Regex.Replace(row.Cells[index].Range.Text, "[\\a]", ""));
          else
            stringList.Add(Regex.Replace(row.Cells[index].Range.Text, "[\\a]", ""));
        }
        stringListList.Add(stringList);
      }
      return stringListList;
    }

    private static string[] ReadFiles(string dir)
    {
      List<string> stringList = new List<string>();
      return Directory.GetFiles(dir);
    }
  }
}
