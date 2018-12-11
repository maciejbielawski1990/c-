// Decompiled with JetBrains decompiler
// Type: Zarys.ReaderDocSprzedaz
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
using System.Text;
using System.Threading.Tasks;

namespace ZarysManagment2018
{
  internal class ReaderDocSprzedaz
  {
    private List<List<string>> data;
    public List<Sprzedaz> listaSprzedazy;
    private List<string> errors;

    public ReaderDocSprzedaz()
    {
      listaSprzedazy = new List<Sprzedaz>();
    }

    public ReaderDocSprzedaz(List<Sprzedaz> baza)
    {
      listaSprzedazy = baza;
    }

      public void Make(string[] filenames)
      {
          Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
          Microsoft.Office.Interop.Word.Document doc;


          errors = new List<string>();

          foreach (string filename in filenames)
          {

              doc = winword.Documents.Open(filename);

              Table table = doc.Tables[1];
              try
              {
                  data = this.PrepareData(table);
              }
              catch
              {
                  errors.Add("Nieznany bład w funkcji PrepareData dla tabeli z pliku " + filename);
              }

              try
              {
                  Read();
              }
              catch
              {
                  errors.Add("Powyższy błąd czytania nastąpił w pliku: " + filename);
              }
          }


          winword.Quit();
          StreamWriter streamWriter = new StreamWriter("errors\\sprzedazy.txt");
          foreach (string error in this.errors)
              streamWriter.WriteLine(error);
          streamWriter.Close();
      }

      private int Read()
    {
      for (int index = 2; index < data.Count; ++index)
      {
        try
        {
          listaSprzedazy.Add(ReadNode(data[index]));
        }
        catch
        {
          errors.Add("Nieznany błąd dla czytania wiersza i tworzenia pozycji sprzedażowej dla wiersza nr " + index.ToString());
        }
      }
      return 0;
    }

    private Sprzedaz ReadNode(List<string> row)
    {
      Sprzedaz sprzedaz = new Sprzedaz();
      sprzedaz.date = row[1];
      sprzedaz.nr_fv = row[2];
      string[] strArray1 = row[2].Split('/');
     string klientSkrot = FindFVDoc("f-" + strArray1[0] + "-" + strArray1[1]);
        
      List<Klient> list = MainWindow.BKlienci.Baza.Where<Klient>((Func<Klient, bool>) (k => k.skrot.Contains(klientSkrot))).ToList<Klient>();
      try
      {
        sprzedaz.klient = list[0];
      }
      catch
      {
        errors.Add("Błąd klienta dla sprzedazy z faktury nr " + sprzedaz.nr_fv);
      }
      try
      {
        sprzedaz.wart_uslugi = double.Parse(row[8]);
      }
      catch
      {
        errors.Add("Błąd parsowanie usługi dla sprzedaży z faktury nr " + sprzedaz.nr_fv);
      }
      string[] strArray2 = row[4].Split('\r');
      string[] strArray3 = row[5].Split('\r');
      string[] strArray4 = row[6].Split('\r');
      sprzedaz.towary = new List<Towar>();
      for (int index = 0; index < strArray2.Length - 1; ++index)
      {
        try
        {
          sprzedaz.towary.Add(new Towar()
          {
            nazwa_towaru = strArray2[index],
            ilosc = strArray3[index],
            cena_jednostkowa = strArray4[index]
          });
        }
        catch
        {
          errors.Add("Błąd towaru");
        }
      }
      return sprzedaz;
    }

        private string FindFVDoc(string input)
        {
            string str1 = ((IEnumerable<string>)((IEnumerable<string>)Directory.GetFiles("D:\\Zarys\\FV 2017")).ToList<string>().Where<string>((Func<string, bool>)(f => f.Contains(input))).ToList<string>()[0].Split('\\')).Last<string>();

            string str2 = "";
            char[] chArray = new char[1] { '.' };
            string[] strArray = ((IEnumerable<string>)str1.Split(chArray)).First<string>().Split(new char[1]
            {
        '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            string str3 = str2 + strArray[3];
            for (int index = 4; index < ((IEnumerable<string>)strArray).Count<string>(); ++index)
                str3 = str3 + "-" + strArray[index];
            return str3;
        }

        private List<List<string>> PrepareData(Table tabela)
    {
      List<List<string>> stringListList = new List<List<string>>();
      foreach (Row row in tabela.Rows)
      {
        List<string> stringList = new List<string>();
        for (int index = 1; index <= row.Cells.Count; ++index)
        {
          if (index <= 4 || index >= 9)
            stringList.Add(Regex.Replace(row.Cells[index].Range.Text, "[\\a]|[\\r]", ""));
          else
            stringList.Add(Regex.Replace(row.Cells[index].Range.Text, "[\\a]", ""));
        }
        stringListList.Add(stringList);
      }
      return stringListList;
    }
  }
}
