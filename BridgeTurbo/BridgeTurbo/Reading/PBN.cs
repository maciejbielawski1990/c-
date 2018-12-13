using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using System.IO;
namespace Bridge.Reading
{
    class PBN
    {
        public struct PBNStruct
        {
            public int nr;
            public positions dealer;
            public vulnerabilties vul;
            public RozkladKart rozklad;
            public int[,] df;
            public string miniMax;
        };
    
        StreamReader reader;
        public List<PBNStruct> lista;

        public PBN(string url)
        {
            reader = new StreamReader(url);
            lista = new List<PBNStruct>();
            ReadPBN2();
        }

        public void ReadPBN2()
        {
            string content = reader.ReadToEnd();
            content = content.Replace("\r","");
            string[] sep={"\n"};
            string[] tablica = content.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            int boards = (tablica.Count() - 2) / 7;

            for (int idx = 3; idx < tablica.Count(); idx++)
            {
                PBNStruct node = new PBNStruct();
                node.nr = int.Parse(tablica[idx].Split('\"')[1]); idx++;
                node.dealer = (positions)Enum.Parse(typeof(positions), tablica[idx][9].ToString()); idx++;
                string vu = tablica[idx].Split('\"')[1].ToLower();
                try
                {
                    node.vul = (vulnerabilties)Enum.Parse(typeof(vulnerabilties), vu);
                }
                catch
                {
                    node.vul = vulnerabilties.both;
                }
                idx++;
                int d = (int)node.dealer;
                string r = d.ToString() + ConvertRozkladToLinString(tablica[idx]); idx++;
                node.rozklad = new RozkladKart(r);

                node.df = ReadDeepF(tablica[idx]); idx++;
                node.miniMax = tablica[idx].Split('\"')[1]; idx++;

                lista.Add(node);
            }
        }

        public List<RozkladKart> ReadPBN()
        {
            List<RozkladKart> rozklady = new List<RozkladKart>();
            string rozk_="";
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                if (s.StartsWith("[Board"))
                {

                }
                if (s.StartsWith("[Dealer"))
                {
                    rozk_ += ConvertDealerToString(s[9]);
                    
                }
                if (s.StartsWith("[Deal "))
                {
                    rozk_ += ConvertRozkladToLinString(s);
                    rozklady.Add(new RozkladKart(rozk_));
                    rozk_ = "";
                }
            }

            return rozklady;
        }

       private string ConvertRozkladToLinString(string input)
       {
           input = input.Split('\"')[1].Substring(2);
           string[] hands = input.Split(' ');
           string[] karts = new string[4];           
           string[] znaki = { "S", "H", "D", "C" };

           string rozklad = "";
           for (int i = 0; i < 4; i++)
           {
               karts = hands[(i + 2) % 4].Split('.');
               for (int k = 0; k < 4; k++)
               {
                   rozklad += znaki[k] + karts[k];
               }
               rozklad += ',';
           }
           rozklad = rozklad.Split('"')[0];

           return rozklad;
       }

       private string ConvertDealerToString(char input)
       {
           string output="";

           if (input == 'N') output = "3";
           if (input == 'S') output = "1";
           if (input == 'W') output = "2";
           if (input == 'E') output = "4";

           return output;
       }

       private int[,] ReadDeepF(string input)
       {
          string content = input.Split('\"')[1];
          string[] gracze = content.Split(' ');
          int[,] lewy = new int[4, 5];
        
          for (int i = 0; i < 4; i++)
          {
              for (int j = 0; j < 5; j++)
              {
                  lewy[i, j] = Convert.ToInt32(gracze[i][j + 2].ToString(),16);
              }
          }

          return lewy;
       }
    
    }
}
