using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    public class BWSStruct
    {
        public int ns;
        public int ew;
        public int table;
        public int round;
        public Contract contract;
        public string[] bidding;
        public int section;
        public int boardNr;
        public List<Bidding> Bidding;
        public List<Board> boards;
        public BWSStruct()
        {
            boards = new List<Board>();
            bidding = new string[50];
        }

    }

    public class PAIR
    {
        public int nr;
        public string player1;
        public string player2;

        public PAIR(string[] node)
        {
            nr = int.Parse(node[0]);
            player1 = node[1];
            player2 = node[2];
        }
    }
}
