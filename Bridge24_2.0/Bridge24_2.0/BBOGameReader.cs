using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge24_2._0
{
    public class BBOGameReader
    {

        public List<InfoBoard> ContractList;
        public string[] nazwiska_nicki;
        public string[] nazwiska_;
        public List<string[]> licytacja_;
        public DateTime data;
        public RozkladKart[] rozklady;
        private string[] boardstring;
        private string[] scores_;
        public bool cztery_rece = false;
        public string[] wisty;
        string[] tablica;
        public BBOGameReader ReadLinBBO(string calosc)
        {
            string[] tablica_ = calosc.Split('|');
            tablica = new string[tablica_.Count()];
            tablica = tablica_;

            int idx_ = 0;
           
                idx_ = ReadScores();

            ReadSurnames(idx_);
            SetSurnames();
            ReadBidding();
            boardstring = new string[Ustawienia.ilosc_rozdan + 3];
            FindBoards();
            ReadBoards();

            if (MainWindow.gra)
                MakeBoards();
            
            if (MainWindow.gra)
            {
                ReadLeads();
                int j = 0;
                for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
                {
                    if (ContractList[i].level != 0)
                    {
                        ContractList[i].wist = wisty[j];
                        j++;
                    }
                    
                }
            }
 
            return this;
        }
        private int ReadScores()
        {
          //  scores_ = new string[Ustawienia.ilosc_rozdan + 1];
            int idx = 0;
            
            while (true)
            {
                if (tablica[idx] == Ustawienia.wyniki)
                {
                    char[] del = {','};
                    scores_ = tablica[idx + 1].Split(del, StringSplitOptions.RemoveEmptyEntries);
                    break;
                }
                idx++;
            }
            return idx;
        }

        private void ReadSurnames(int idx)
        {
            nazwiska_nicki = new string[4];

            while (true)
            {
                if (tablica[idx] == Ustawienia.nazwiska)
                {
                    nazwiska_nicki = tablica[idx + 1].Split(',');
                    break;
                }
                idx++;
            }
        }
        private void SetSurnames()
        {
            nazwiska_ = new string[5];

            int stop = 0;
            for (int i = 0; i < 4; i++)
            {
                string tmp = nazwiska_nicki[i];

                int j;
                int k = InfoBoard.nicki.Count();
                for (j = 0; j < InfoBoard.nicki.Count(); j++)
                {
                    if (InfoBoard.nicki[j] == tmp)
                    {
                        stop = j;
                        nazwiska_[i] = InfoBoard.nazwiska[stop];
                        break;
                    }
                    if (InfoBoard.nicki[j] == null)
                    {
                        nazwiska_[i] = nazwiska_nicki[i];
                        break;
                    }

                }
            }
        }
        private void ReadBidding()
        {
            int nr_rozdania = 0;
            int odzywka = 0;
            int idx = 0;
            bool test = false;
            string[] tmp = new string[40];
            licytacja_ = new List<string[]>();
            while (idx < tablica.Count())
            {


                if (tablica[idx] == Ustawienia.rozdanie)
                {
                    odzywka = 0;
                    if (test)
                        licytacja_.Add(tmp);
                    test = true;
                    tmp = new string[40];
                    nr_rozdania++;
                }

                if (tablica[idx] == Ustawienia.licytacja)
                {
                    idx++;
                    odzywka++;
                    string odzywka_ = tablica[idx];
                    if (odzywka_.Contains('!'))
                        odzywka_ = odzywka_.Remove(odzywka_.IndexOf('!')).ToString();

                    tmp[odzywka] = odzywka_;
                    
                }
                idx++;
            }
            if (test) licytacja_.Add(tmp);
        }

        private void ReadLeads()
        {
            wisty = new string[Ustawienia.ilosc_rozdan * 2 + 3];
            
            int idx = 0;
            int iter = 0;
            for (; idx < tablica.Count(); idx++)
            {
                if (tablica[idx] == Ustawienia.rozdanie)
                {
                 
                    while (tablica[idx] != Ustawienia.wisty)
                    {
                        idx++;
                    }
                 //   ContractList[iter].wist = tablica[idx + 1];
                    wisty[iter++] = tablica[idx + 1];
                }
            }
        }

        private void FindBoards()
        {
            int idx = 0;
            int nr_rozdania = 0;
            while (idx < tablica.Count())
            {
                if (tablica[idx] == Ustawienia.rozklad)
                {
                    nr_rozdania++;
                    idx++;
                    boardstring[nr_rozdania] = tablica[idx];
                }
                idx++;
            }
        }

        private RozkladKart ReadBoard(int nr)
        {
            RozkladKart rozklad_ = new RozkladKart();
            string piki = "AKQJT98765432";
            string kiery = "AKQJT98765432";
            string kara = "AKQJT98765432";
            string trefle = "AKQJT98765432";
            bool cztery_rece = false;

            //reka N
            string tmp = boardstring[nr].Split(',')[0];

            rozklad_.S = new Karty();
            rozklad_.S.piki = "";
            int j = 2;


            while ((tmp[j].ToString() != "H"))
            {
                rozklad_.S.piki += tmp[j].ToString();
                piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while ((tmp[j].ToString() != "D") )
            {
                rozklad_.S.kiery += tmp[j].ToString();
                kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "C")
            {
                rozklad_.S.kara += tmp[j].ToString();
                kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (j < tmp.Count())
            {
                rozklad_.S.trefle += tmp[j].ToString();
                trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                j++;
            }


            // druga reka

            tmp = boardstring[nr].Split(',')[1];
            j = 1;
            if (tmp != "")
            {
                rozklad_.W = new Karty();
                cztery_rece = true;
                while ((tmp[j].ToString() != "H") )
                {
                    rozklad_.W.piki += tmp[j].ToString();
                    piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                    j++;
                }
                j++;
                while ((tmp[j].ToString() != "D") )
                {
                    rozklad_.W.kiery += tmp[j].ToString();
                    kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                    j++;
                }
                j++;
                while (tmp[j].ToString() != "C")
                {
                    rozklad_.W.kara += tmp[j].ToString();
                    kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                    j++;
                }
                j++;
                while (j < tmp.Count())
                {
                    rozklad_.W.trefle += tmp[j].ToString();
                    trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                    j++;
                }
            }
            // trzecia reka

            tmp = boardstring[nr].Split(',')[2];
            j = 1;

            rozklad_.N = new Karty();

            while ((tmp[j].ToString() != "H"))
            {
                rozklad_.N.piki += tmp[j].ToString();
                piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while ((tmp[j].ToString() != "D") )
            {
                rozklad_.N.kiery += tmp[j].ToString();
                kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "C")
            {
                rozklad_.N.kara += tmp[j].ToString();
                kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (j < tmp.Count())
            {
                rozklad_.N.trefle += tmp[j].ToString();
                trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                j++;
            }
            if (cztery_rece)
            {
                rozklad_.E = new Karty();
                rozklad_.E.piki = piki;
                rozklad_.E.kiery = kiery;
                rozklad_.E.kara = kara;
                rozklad_.E.trefle = trefle;
            }

            return rozklad_;
        }

        private void ReadBoards()
        {
            rozklady = new RozkladKart[Ustawienia.ilosc_rozdan + 2];
            for (int i = 1; i < Ustawienia.ilosc_rozdan+1; i++)
            {
                rozklady[i] = ReadBoard(i);
            }
        }

        private void MakeBoard(string ciagwynikow, int j)
        {
            InfoBoard node = new InfoBoard();
            int i = 0;
            node.nr = j / 2 + 1;
            node.level = int.Parse(ciagwynikow[i].ToString());
            i++;
            node.suit = ciagwynikow[i].ToString();
            i++;
            node.declarer = ciagwynikow[i].ToString();
            i++;
            string elem = ciagwynikow[i].ToString();
            if (elem == "x")
            {
                node.kontra = true;
                i++;
                if (ciagwynikow[i].ToString() == "x")
                {
                    node.rekontra = true;
                    i++;
                }
            }
            if (ciagwynikow[i].ToString() != "=")
            {
                node.nadrobek = int.Parse(ciagwynikow[i].ToString() + ciagwynikow[i + 1].ToString());
                i += 3;
                node.lew = node.nadrobek.ToString();

            }
            else
            {
                i += 2;
                node.lew = "=";
            }

            if (node.nadrobek < 0) node.realizacja = false;

            node.score = InfoBridge.oblicz_zapis(node);


            ContractList.Add(node);

              
        }
        

        private void MakeBoards()
        {
            ContractList = new List<InfoBoard>();
         

            for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
            {
                MakeBoard(scores_[i], i);
            }

        }
    }
}
