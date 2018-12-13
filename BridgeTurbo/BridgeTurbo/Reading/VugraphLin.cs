using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using System.IO;
namespace Bridge.Reading
{
    public class VugraphLin : Lin
    {
        public List<Board> boards_closed;
        public Title title;
        private string[] surnames;

        public VugraphLin(string path)
        {
            StreamReader reader = new StreamReader(path);
            content = reader.ReadToEnd();
            DeleteComments();
        }

        public VugraphLin() { }

        /// <summary>
        /// Wczytuke date, liste kontraktow(aby pozniej przypisac kontrakty do poszczegolnych rozdan.
        /// Wyszukuje linie z rozdaniami w pliku, a nastepnie dla kazdej linii tworzy rozdanie.
        /// 
        /// W rezultacie wypelnione sa 2 listy - boards, boards_closed w klasie VugraphLin.
        /// </summary>
        public void ReadLin()
        {
            title = ReadTitleLine();
            List<Contract> contracts = ConvertContractLine();
            surnames = GetNicks();

            List<string> rozdanka = FindLines(Settings.board_line);
            boards = new List<Board>();
            boards_closed = new List<Board>();

            for (int i = 0; i < rozdanka.Count; i++)
            {
                ReadBoard(rozdanka[i], contracts[i]);
            }
        }

        /// <summary>
        /// Uzupelnia zmienne skladowe klasy board. W ten sposob wiemy wszystko o rozdaniu, co mozna zobaczyc w linie.
        /// </summary>
        /// <param name="boardline">Linia z rozdaniem</param>
        /// <param name="contract_">kontrakt do uzupelnienia</param>
        /// <returns></returns>
        public Board ReadBoard(string boardline, Contract contract_)
        {
            Board board = new Board();
            board.nr = GetNumberQx(boardline);
            board.lead = GetLead(boardline);
            board.rozklad = GetRozklad(boardline);


            board.contract = contract_;
            board.bidding = GetBidding(boardline);
           

            bool partia = GetVulnerability(board.nr, board.contract.declarer);
            board.vulnerability = SetVulnerability(boardline);
            board.score = board.CalculateScore(partia);


            if (IfOpenRoom(boardline))
            {
                board.players = SurnamesToBidding(surnames,1);
                boards.Add(board);

            }
            else
            {
                board.players = SurnamesToBidding(surnames,2);
                boards_closed.Add(board);
            }

            return board;
        }


        // funkcja obsluguje, gdy 1 rozdanie w wielu liniach!!!!!
        private List<string> FindLines(string code, int delay = 3)
        {
            int idx = 0;

            List<string> output = new List<string>();
            int dlug = content.Length;
            while ((idx = content.IndexOf(code, idx)) >= 0)
            {
                int idx2 = content.IndexOf(code, idx + 1);
                int length = idx2 -idx;
                if (length > 0)
                {
                    string board = content.Substring(idx, length);

                    output.Add(board);
                }
                else
                {
                    string board = content.Substring(idx, dlug - idx);
                    output.Add(board);
                }
                idx++;
                //if (output.Count == 30)
                //{
                //    idx++;
                //}
            }

            return output;
        }

        private void DeleteComments()
        {
            int idx = 0;

            while ((idx = content.IndexOf("nt|", idx)) >= 0)
            {
                int idx_k = content.IndexOf("pg", idx);
                int length = idx_k - idx + 4;

                string tmp = content.Substring(idx, length);

                content = content.Remove(idx, length);
                idx = 0;
            }
        }
    }
}
