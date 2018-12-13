using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge24_2._0
{
    public class VugraphLin
    {
        string[] tablica;
        string[] scores_;
        private string[,] licytacja;
        private string[] boardstring;
        public string[] nazwiska_;
       
        public List<InfoBoard> Vu_ContractList_Closed, Vu_ContractList_Open;
        public RozkladKart[] rozklady;
        public List<string[]> licytacja_open, licytacja_closed;
        public string tytul;
        string[] wisty;
        public string team1Name;
        public string team2Name;
      

        public VugraphLin ReadLinVugraph(string calosc)
        {
            VugraphLin vugraph_ = new VugraphLin();
            string[] tab = calosc.Split('|');

            DeleteComment(tab);
            int idx = ReadScores();
            
            ReadSurnames(idx);
           ReadLeads();
            ReadTitle();
            licytacja = new string[2 * Ustawienia.ilosc_rozdan + 3, 40];
            FindBidding();
            licytacja_closed = new List<string[]>();
            licytacja_open = new List<string[]>();
            if (Ustawienia.linoc)
            {

                licytacja_open = ReadBiddingOpenRoom();
                licytacja_closed = ReadBiddingClosedRoom();
            }
            else
            {
                licytacja_closed = ReadBiddingOpenRoom();
                licytacja_open = ReadBiddingClosedRoom();
            }
            boardstring = new string[Ustawienia.ilosc_rozdan + 3];

            FindBoards();
            ReadBoards();

            MakeBoards();

     

            return vugraph_;
        }

        private void DeleteComment (string[] tab_)
        {
            string[] tablica2 = new string[tab_.Count()];

            int j = 0;
            for (int i = 0; i < tab_.Count(); i++)
            {
                if (tab_[i] != Ustawienia.komentarze)
                {
                    tablica2[j] = tab_[i];
                    j++;
                }
                else
                {
                    i++;
                }
                /*
                if (tab_[i] == Ustawienia.wisty)
                {
                    while (tab_[i] != Ustawienia.koniec)
                    {
                        i++;
                    }
                }*/
            }

            tablica = tablica2;

        }

        private void ReadLeads()
        {
            wisty = new string[Ustawienia.ilosc_rozdan * 2 + 3];
           
            int idx = 0;
            int iter=0;
            for (;idx < tablica.Count(); idx++)
            {
                if (tablica[idx] == Ustawienia.rozdanie)
                {
                    
                    while (tablica[idx]!=Ustawienia.wisty)
                    {
                        idx++;
                    }
                    if (scores_[iter] == "P")
                        iter += 2;
                    wisty[iter++] = tablica[idx + 1];
                }
            }
        }

        private void ReadTitle()
        {
            int idx = 0;
            while (true)
            {
                if (tablica[idx] == Ustawienia.tytul) 
                {
                    tytul = tablica[idx + 1].Split(',')[0];
                    team1Name = tablica[idx + 1].Split(',')[5];
                    team2Name = tablica[idx + 1].Split(',')[7];
                    break;
                }
                idx++;
            }
        }

        private int ReadScores()
        {
            scores_ = new string[Ustawienia.ilosc_rozdan * 2 + 4];
            int idx = 0;
            while (true)
            {
                if (tablica[idx] == Ustawienia.wyniki)
                {
                    scores_ = tablica[idx + 1].Split(',');
                    break;
                }
                idx++;
            }
            return idx;
        }

        private void ReadSurnames(int idx)
        {
            nazwiska_ = new string[8];

            while (true)
            {
                if (tablica[idx] == Ustawienia.nazwiska)
                {
                    nazwiska_ = tablica[idx + 1].Split(',');
                    break;
                }
                idx++;
            }

            for (int i = 0; i < 8; i++)
            {
                if (nazwiska_[i] == "")
                    nazwiska_[i] = " ";
            }
        }

        private void FindBidding()
        {
            int nr_rozdania = 0;
            int odzywka = 0;
            int idx = 0;

            while (idx < tablica.Count())
            {


                if (tablica[idx] == Ustawienia.rozdanie)
                {
                    odzywka = 0;
                    nr_rozdania++;
                }

                if (tablica[idx] == Ustawienia.licytacja)
                {
                    idx++; 
                    odzywka++;
                    string odzywka_ = tablica[idx];
                    if (odzywka_.Contains('!'))
                        odzywka_ = odzywka_.Remove(odzywka_.IndexOf('!')).ToString();

                    licytacja[nr_rozdania, odzywka] = odzywka_;
                }
                idx++;
            }
        }

        private List<string[]> ReadBiddingOpenRoom()
        {
            List<string[]> open_ = new List<string[]>();
  
            for (int i = 1; i < (Ustawienia.ilosc_rozdan * 2 + 1); i+=2)
            {
                string[] tmp = new string[40];
                for (int j = 0; j < 40; j++)
                {
                   tmp[j] = licytacja[i, j];
                }
                
                open_.Add(tmp);
                
            }

            return open_;
        }

        private List<string[]> ReadBiddingClosedRoom()
        {

            List<string[]> closed_ = new List<string[]>();
            for (int i = 2; i < (Ustawienia.ilosc_rozdan * 2 + 1); i += 2)
            {
                string[] tmp = new string[40];
                for (int j = 0; j < 40; j++)
                {
                    tmp[j] = licytacja[i, j];
                }

               closed_.Add(tmp);

            }

            return closed_;
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
                    boardstring[nr_rozdania/2] = tablica[idx];
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


            //reka N
            string tmp = boardstring[nr].Split(',')[0];

            rozklad_.S = new Karty();
            rozklad_.S.piki = "";
            int j = 2;

            while (tmp[j].ToString() != "H")
            {
                rozklad_.S.piki +=tmp[j].ToString();
                piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                j++;
            }
            rozklad_.S.piki = reverse(rozklad_.S.piki);
            j++;
            while (tmp[j].ToString() != "D")
            {
                rozklad_.S.kiery += reverse(tmp[j].ToString());
                kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                j++;
            }
            rozklad_.S.kiery = reverse(rozklad_.S.kiery);
            j++;
            while (tmp[j].ToString() != "C")
            {
                rozklad_.S.kara += reverse(tmp[j].ToString());
                kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                j++;
            }
            rozklad_.S.kara = reverse(rozklad_.S.kara);
            j++;
            while (j < tmp.Count())
            {
                rozklad_.S.trefle += reverse (tmp[j].ToString());
                trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                j++;
            }
            rozklad_.S.trefle = reverse(rozklad_.S.trefle);

            // druga reka

            tmp = boardstring[nr].Split(',')[1];
            j = 1;

            rozklad_.W = new Karty();

            while (tmp[j].ToString() != "H")
            {
                rozklad_.W.piki += reverse (tmp[j].ToString());
                piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "D")
            {
                rozklad_.W.kiery += reverse( tmp[j].ToString());
                kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "C")
            {
                rozklad_.W.kara +=  reverse (tmp[j].ToString());
                kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (j < tmp.Count())
            {
                rozklad_.W.trefle +=  reverse(tmp[j].ToString());
                trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                j++;
            }
            rozklad_.W.piki = reverse(rozklad_.W.piki);
            rozklad_.W.kiery = reverse(rozklad_.W.kiery);
            rozklad_.W.kara = reverse(rozklad_.W.kara);
            rozklad_.W.trefle = reverse(rozklad_.W.trefle);
            // trzecia reka

            tmp = boardstring[nr].Split(',')[2];
            j = 1;

            rozklad_.N = new Karty();

            while (tmp[j].ToString() != "H")
            {
                rozklad_.N.piki += reverse( tmp[j].ToString() );
                piki = piki.Remove(piki.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "D")
            {
                rozklad_.N.kiery += reverse (tmp[j].ToString());
                kiery = kiery.Remove(kiery.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (tmp[j].ToString() != "C")
            {
                rozklad_.N.kara += reverse( tmp[j].ToString());
                kara = kara.Remove(kara.LastIndexOf(tmp[j]), 1);
                j++;
            }
            j++;
            while (j < tmp.Count())
            {
                rozklad_.N.trefle += reverse( tmp[j].ToString() );
                trefle = trefle.Remove(trefle.LastIndexOf(tmp[j]), 1);
                j++;
            }

            rozklad_.N.piki = reverse(rozklad_.N.piki);
            rozklad_.N.kiery = reverse(rozklad_.N.kiery);
            rozklad_.N.kara = reverse(rozklad_.N.kara);
            rozklad_.N.trefle = reverse(rozklad_.N.trefle);

            rozklad_.E = new Karty();
            rozklad_.E.piki = piki;
            rozklad_.E.kiery = kiery;
            rozklad_.E.kara = kara;
            rozklad_.E.trefle = trefle;


            return rozklad_;
        }

        private void ReadBoards()
        {
            rozklady = new RozkladKart[Ustawienia.ilosc_rozdan + 2];
            for (int i = 0; i < Ustawienia.ilosc_rozdan; i++)
            {
                rozklady[i] = ReadBoard(i);
            }
        }

        public void WczytajVuKontrakty(string[] input)
        {
            Vu_ContractList_Closed = new List<InfoBoard>();
            Vu_ContractList_Open = new List<InfoBoard>();

            for (int j = 0; j < input.Count(); j++)
            {
                InfoBoard node = new InfoBoard();
                int i = 0;
                string ciagwynikow = input[j];
                if (ciagwynikow == "P")
					{
                    node.declarer = " ";
                    node.level = 0;
                    node.lew = "-";
                    node.score = 0;
                }
                else
                {
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

                    if (j % 2 == 0)
                    {
                        //kontrolka_vu2.Add(node);
                        Vu_ContractList_Open.Add(node);
                        // vu2_zapisy[(j - 1) / 2 + 1] = node.score;
                    }
                    else
                    {
                        //kontrola_vu1.Add(node);
                        Vu_ContractList_Closed.Add(node);
                        //vu1_zapisy[(j - 1) / 2 + 1] = node.score;
                    }
                }
            }
        }

        private void MakeBoard(string ciagwynikow,int j)
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

            node.wist = wisty[j];
            node.score = InfoBridge.oblicz_zapis(node);

            if (j % 2 == 0)
            {
                //kontrolka_vu2.Add(node);
            
                    Vu_ContractList_Open.Add(node);
              
                // vu2_zapisy[(j - 1) / 2 + 1] = node.score;
            }
            else
            {
                //kontrola_vu1.Add(node);
   
                    Vu_ContractList_Closed.Add(node);
               
                //vu1_zapisy[(j - 1) / 2 + 1] = node.score;
            }
        }
        string reverse(string str)
        {
            char[] charArray = str.ToCharArray(); 
	        Array.Reverse(charArray);
	        return new string(charArray);
        }

  
        private void MakeBoards()
        {
            Vu_ContractList_Closed = new List<InfoBoard>();
            Vu_ContractList_Open = new List<InfoBoard>();

            for (int i = 0; i < Ustawienia.ilosc_rozdan*2; i++)
            {
                if (scores_[i] != "P")
                    MakeBoard(scores_[i], i);
                else
                {
                    if (i % 2 == 0)
                    {
                        InfoBoard pasy = new InfoBoard();
                        pasy.level = 0;
                        pasy.suit = " ";
                        pasy.declarer = " ";
                        pasy.score = 0;
                        Vu_ContractList_Open.Add(pasy);
                    }
                    else
                    {
                        InfoBoard pasy = new InfoBoard();
                        pasy.level = 0;
                        pasy.suit = " ";
                        pasy.declarer = " ";
                        pasy.score = 0;
                        Vu_ContractList_Closed.Add(pasy);
                    }
                }
            }

        }
    }

    
}
