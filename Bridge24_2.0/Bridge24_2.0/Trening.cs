using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge24_2._0
{
    [Serializable]
    public class Trening
    {
        public List<InfoBoard> ContractList;
        public List<InfoBoard> Vu_ContractList_Open;
        public List<InfoBoard> Vu_ContractList_Closed;

        public string[] nazwiska_nasze;
        public int nr_first_board;
        public DateTime data_treningu;
        public string[,] licytacja;
        public RozkladKart[] rozklady;
        public string title;
        public string[] nazwiska_vu;

        public string[,] licytacja_open;
        public string[,] licytacja_closed;
        public string[] wist_open;
        public string[] wist_closed;


        public int[] mecz1, mecz2;
        public string nr;
        public AnalizaTreningu[] komentarze;
    }
}
