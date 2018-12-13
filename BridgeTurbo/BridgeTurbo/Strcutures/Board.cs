using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    /// <summary>
    /// \brief Struktura reprezentujaca rozdanie w brydzu sportowym
    /// </summary>
    public class Board
    {
        public Contract contract { get; set; }
        public string lead { get; set; }
        public int score { get; set; }
        public vulnerabilties vulnerability;
        public RozkladKart rozklad;
        public int nr { get; set; }
        public List<Bidding> bidding;

        public int[,] deepfinesse;
        public string[] players;
        public int impNS { get; set; }

        public int CalculateScore(bool partia)
        {
            int score = 0;
            int d = (int)contract.declarer;
            score = contract.CalculateScore(partia);
            if (d % 2 == 0)
                score = -score;


            return score;
        }
    }
}
