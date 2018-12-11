using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Butler
{
    class WBFReader
    {

        public string[] html_protocols;
        public List<TablesButlerData> tablesData;
        string url, serwer;
        string ticker_HomeTeam = "//span[@class='SpanStyleHomeTeam']";
        string ticker_AwayTeam = "//span[@class='SpanStyleVisitingTeam']";
        int tournament_id;
        int round_id;



        public WBFReader(string url_, string serwer_, int tour_id, int r_id)
        {
           // url = "http://www.worldbridge.org/repository/tourn/chennai.15/Microsite/Asp/RoundTeams.asp?qtournid=1130&qroundno=1";
            url = url_;
            serwer = serwer_;
            tournament_id = tour_id;
            round_id = r_id;
        }

        /// <summary>
        /// Funkcja na podstawie zadanego url i serwera wczytuje wszystkie html z rundy i przetwarza dane do struktury TablesButlerData
        /// </summary>
        /// <returns>Liste danych 'TablesButlerData' z danymi ze stolow(nazwiska, teamy i wyniki(scores)</returns>
        public List<TablesButlerData> ReadWBFPage()
        {
            html_protocols = GetAllHtmlProtocols(GetTablesLinks(url, serwer));
            tablesData = new List<TablesButlerData>();
            for (int i = 0; i < html_protocols.Count(); i++)
            {
                tablesData.Add(ReadDataScoresWithTableInfo(html_protocols[i]));
            }

            return tablesData;
        }

        /// <summary>
        /// Funkcja wczytuje dane z naglowka oraz z tabeli z wynikami na podstawie html do struktury programu TablesButlerData
        /// </summary>
        /// <param name="html">Html z protokolem z meczu</param>
        /// <returns>Dane w strukturze programu TablesButleraData</returns>
        private TablesButlerData ReadDataScoresWithTableInfo(string html)
        {
            TableHeader tableheader = GetTableInfo(html);
            List<int>[] scores = GetScores(html);

            TablesButlerData tableData = new TablesButlerData();
            tableData.players = tableheader.surnames;
            tableData.TeamHome = tableheader.homeTeam;
            tableData.TeamAway = tableheader.awayTeam;

            tableData.scoresOR = scores[0];
            tableData.scoresCR = scores[1];

            tableData.id = (tournament_id * 10000 + round_id*100);

            return tableData;

        }

        /// <summary>
        /// Funkcja pobiera nazwy teamów i imiona z nazwiskami zawodnikow na podstawie naglowka z protokolu. 
        /// Algorytm: Nazwy teamow pochodza z linkow (pierwsze dwa linki to nazwy teamow), 
        /// Nazwiska poszczegolnych zawodnikow w teamie sa w "innym stylu" html. 
        /// </summary>
        /// <param name="html">Html ze stroną - tabela z kontrolka z obu stolow(WBFType)</param>
        /// <returns>Wypelniona struktura TableInfo, nazwiska w kolejnosci : NWES - najpierw pokoj otwarty, potem zamkniety</returns>
        private TableHeader GetTableInfo(string html)
        {
            string[] surnames = new string[8];
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection team = doc.DocumentNode.SelectNodes("//a[@href]");

            string home = team[0].InnerText;
            string away = team[1].InnerText;
            //Home Team
            HtmlNodeCollection links = doc.DocumentNode.SelectNodes(ticker_HomeTeam); // na podstawie tickera (raczej kolor)
            int idx = 0;
            foreach (var link in links)
            {
                surnames[idx++] = link.ChildNodes[1].InnerHtml.ToString().Split('&')[1].ToString().Substring(5);
            }

            links = doc.DocumentNode.SelectNodes(ticker_AwayTeam); // na podstawie tickera 

            foreach (var link in links)
            {
                surnames[idx++] = link.ChildNodes[1].InnerHtml.ToString().Split('&')[1].ToString().Substring(5);
            }

            return new TableHeader(surnames, home, away);
        }

        /// <summary>
        /// Funkcja odczytuje wyniki ze strony html na podstawie tabeli z kontrolka z obu stolow (WBFType). Zwraca 2-elementowa
        /// tablice list, odpowiednio z pokojem otwartym i zamknietym
        /// </summary>
        /// <param name="html">Html ze stroną - tabela z kontrolka z obu stolow(WBFType) </param>
        /// <returns>Tablica list - pierwszy element tablicy to wyniki z pokoju otwartego, drugi z pokoju zamknietego </returns>
        private List<int>[] GetScores(string html)
        {
            List<int>[] scores = new List<int>[2];
           
            scores[0] = new List<int>(); // open room
            scores[1] = new List<int>(); // closed room

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table");
            HtmlNodeCollection rows = tables[7].SelectNodes(".//tr"); // 8 tabela to tablica z wynikami

            for (int i = 1; i < rows.Count; ++i)
            {
                HtmlNodeCollection cols = rows[i].SelectNodes(".//td"); //pomijamy wiersz naglowkowy
                int score;
                string[] s = cols[5].InnerText.Split('&'); // w 6-tej kolumnie jest zapis z otwartego
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
                scores[0].Add(score);

                s = cols[11].InnerText.Split('&'); // w 12-tej kolumnie jest zapis z otwartego
                if (s[0] != "")
                {
                    score = int.Parse(s[0]);
                }
                else
                {
                    s = cols[12].InnerText.Split('&');
                    score = int.Parse(s[0]) * (-1);
                }
                // closed table
                scores[1].Add(score);
            }

            return scores;
        }


        /// <summary>
        /// Pobiera html z kontrolkami z meczów z danej rundy na podstawie podanych linkow
        /// </summary>
        /// <param name="links">Linki do meczów</param>
        /// <returns>Tablicę stringów zawierającą treść html z kontrolkami z meczów</returns>
        private string[] GetAllHtmlProtocols(List<string> links)
        {
            WebClient client = new WebClient();
            string[] protocols = new string[links.Count];

            for (int i = 0; i < links.Count; i++)
            {
                string html = client.DownloadString(links[i]);
                protocols[i] = html;
            }

            return protocols;
        }


        /// <summary>
        /// Funkcja pobiera linki do wszystkich meczy(stolow) z zadanego linka do strony. Powinna to byc strona z runda
        /// taka jak WBFu.
        /// Algorytm : Pobiera strone z adresu 'roundUrl' wyszukuje wszystkie linki zawarte na stronie i wybiera tylko te, 
        /// które zawieraja odpowiedni napis 'BoardDetails' i dodaje te linki do listy wynikowej
        /// </summary>
        /// <param name="roundUrl">adres strony z runda (wyniki wszystkich meczy w rundzie)</param>
        /// <param name="serwer">serwer strony - najbardziej zewnetrzny folder</param>
        /// <returns>Liste z linkami do poszczegolnych stron z kontrolkami z meczów</returns>
        private List<string> GetTablesLinks(string roundUrl, string serwer)
        {
            List<string> result = new List<string>();

            WebClient client = new WebClient();
            string html = client.DownloadString(roundUrl);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//a[@href]"); //pobieram wszystkie linki z tej strony

            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains("BoardDetails")) // jesli zawiera rozdanie, dodaje do listy
                {
                    string url = serwer + link.Attributes["href"].Value;
                    result.Add(url);
                }
            }

            return result;
        }



    }
}
