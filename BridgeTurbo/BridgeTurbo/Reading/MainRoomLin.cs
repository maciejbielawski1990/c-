using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
namespace Bridge.Reading
{
    public class MainRoomLin : Lin
    {
        string path;
        int[] numbers;
        public MainRoomLin(string path_)
        {
            path = path_;
            StreamReader reader = new StreamReader(path);
            content = reader.ReadToEnd();
        }

        /// <summary>
        /// Wczytuke date, liste kontraktow(aby pozniej przypisac kontrakty do poszczegolnych rozdan.
        /// Wyszukuje linie z rozdaniami w pliku, a nastepnie dla kazdej linii tworzy rozdanie.
        /// 
        /// W rezultacie wypelniona jest data i lista board z rozdaniami
        /// </summary>
        public void ReadLin()
        {
            date = File.GetCreationTime(path).ToShortDateString();           
            List<Contract> contracts = ConvertContractLine();
          //  surnames = GetNicks();
           numbers = GetNumbersFromBNLines();

            List<string> rozdanka = FindLines(Settings.board_line);
            boards = new List<Board>();


            for (int i = 0; i < rozdanka.Count; i++)
            {
                ReadBoard(rozdanka[i], contracts[i]);
            }
        }


        // funkcja wczytuje poszczegolne rozdania, tworzy strukture board dla plikow z lina z MainRoomu
        // Numer rozdania na podstawie linii qx
        // partia korzystajac z numeru rozdania !! (do wyliczania wynikow)
        // vulnerability korzysta z linii lina !!

        /// <summary>
        /// Uzupelnia zmienne skladowe klasy board. W ten sposob wiemy wszystko o rozdaniu, co mozna zobaczyc w linie.
        /// </summary>
        /// <param name="boardline">Linia z rozdaniem</param>
        /// <param name="contract_">kontrakt do uzupelnienia</param>
        /// <returns></returns>
        public Board ReadBoard(string boardline, Contract contract_)
        {
            Board board = new Board();

            board.nr = numbers[GetNumberQx(boardline) - 1]; // moze byc raczej w zasadzie lp
            board.lead = GetLead(boardline);
            board.rozklad = GetRozklad(boardline);


            board.contract = contract_;
            board.bidding = GetBidding(boardline);
           
            bool partia = GetVulnerability(board.nr, board.contract.declarer);
            board.vulnerability = SetVulnerability(boardline);
            board.score = board.CalculateScore(partia);

            string[] nicks = GetNicks();
            nicks = SetSurnames(nicks);
            board.players = SurnamesToBidding(nicks,1);
            boards.Add(board);

            return board;
        }
    }
}
