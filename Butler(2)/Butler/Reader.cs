using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace Butler
{
    class Reader
    {
        private string serwer;
        private string[] rounds;

        private int tablesCount = 11;
        private int ile_obciac_zapisow = 0;

        List<TablesData> data;

        string ticker_HomeTeam = "//span[@class='SpanStyleHomeTeam']";
        string ticker_AwayTeam = "//span[@class='SpanStyleVisitingTeam']";

        public Reader()
        {
            //rounds = new string[setter.rounds_count];
           // czytaj_linki_rund(setter.LinksFilename);

            data = new List<TablesData>();
            string url = "http://www.worldbridge.org/repository/tourn/chennai.15/Microsite/Asp/BoardDetails.asp?qmatchid=29351";

            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            GetSurnames(html, 0);
            GetScores(html, 0);
        }



        private void GetSurnames(string html, int tableNumber)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Player[] node = new Player[8]; // N W E S
            string[] surnames = new string[8];
            HtmlNodeCollection team = doc.DocumentNode.SelectNodes("//a[@href]");

            int[] id = new int[100];
            string home = team[0].InnerText;
            string away = team[1].InnerText;
            //Home Team
            HtmlNodeCollection links = doc.DocumentNode.SelectNodes(ticker_HomeTeam);
            int idx = 0;
            foreach (var link in links)
            {
                surnames[idx++] = link.ChildNodes[1].InnerHtml.ToString().Split('&')[1].ToString().Substring(5);
            }

            links = doc.DocumentNode.SelectNodes(ticker_AwayTeam);

            foreach (var link in links)
            {
                surnames[idx++] = link.ChildNodes[1].InnerHtml.ToString().Split('&')[1].ToString().Substring(5);
            }

            TablesData data_ = new TablesData();
            data_.nr = tableNumber;
            data_.players = surnames;
            data_.TeamHome = home;
            data_.TeamAway = away;

            data.Add(data_);

            //for (int i = 0; i < 4; i++)
            //{
            //    Player tmp = new Player();
            //    tmp.nazwisko = surnames[i];
            //    tmp.team_your = home;
            //    tmp.team_against = away;
            //    node[i] = tmp;
            //}
            //for (int i = 4; i < 8; i++)
            //{
            //    Player tmp = new Player();
            //    tmp.nazwisko = surnames[i];
            //    tmp.team_your = away;
            //    tmp.team_against = home;
            //    node[i] = tmp;
            //}
            
         //   nazwiska.Add(node);

        }

        private void GetScores(string html, int tableNumber)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");

            HtmlNodeCollection rows = tables[7].SelectNodes(".//tr");
          
            for (int i = 1; i < rows.Count; ++i)
            {
                HtmlNodeCollection cols = rows[i].SelectNodes(".//td");
                int score;
                string[] s = cols[5].InnerText.Split('&');
                if (s[0] != "")
                {
                    score = int.Parse(s[0]);

                }
                else
                {
                    s = cols[6].InnerText.Split('&');
                    score = int.Parse(s[0]) * (-1);
                }
               // OR[i] = score;
                data[tableNumber].scores[2 * (i-1)] = score;

                s = cols[11].InnerText.Split('&');
                if (s[0] != "")
                {
                    score = int.Parse(s[0]);

                }
                else
                {
                    s = cols[12].InnerText.Split('&');
                    score = int.Parse(s[0]) * (-1);
                }
               // CR[i] = score;
                data[tableNumber].scores[2 * (i - 1) + 1] = score;
            }
        }

        private int ObliczSrednia(int boardnumber)
        {
            int[] scores = new int[2 * tablesCount];
            for (int j = 0; j < tablesCount; j++)
            {
                scores[2 * j] = data[j].scores[2 * boardnumber];
                scores[2 * j + 1] = data[j].scores[2 * boardnumber + 1]; 
            }

            if (ile_obciac_zapisow > 0)
            {
                Array.Sort<int>(scores);
            }

            int suma = 0;
            for (int j = ile_obciac_zapisow; j < (tablesCount * 2 - ile_obciac_zapisow); j++)
            {
                suma += scores[j];
            }

            int mean = (int)(suma / (tablesCount * 2 - ile_obciac_zapisow * 2));

            return mean;

        }



        //public static int wylicz_impy(int saldo)
        //{
        //    int imp = 0;
        //    for (; imp < 24; imp++)
        //    {
        //        if (saldo <= tabela_imp[imp])
        //            break;

        //    }
        //    return imp;
        //}
       
        private int czytaj_linki_rund(string filename)
        {
            FileStream plik = File.OpenRead("lista" + filename + ".txt");
            StreamReader reader = new StreamReader(plik);


            int idx = 0;
            while (!reader.EndOfStream)
            {
                rounds[idx++] = reader.ReadLine();
            }

            return idx;
        }
    }
}
