using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butler {
  
    class Calculator
    {
        static int[] tabela_imp = {19,40,80,120,160,210,260,310,360,420,490,590,740,890,
                               1090,1290,1490,1740,1990,2240,2490,2990,3490,3990};
        List<ButlerPlayer> playersList;
        public Calculator()
        {
            playersList = new List<ButlerPlayer>();
        }

      


        /// <summary>
        /// Funkcja wylicza butlera dla par NS na podstawie podanych danych. Generalnie dla wszystkich stolow. 
        /// Uzupelnia strukture TablesButlerData o wyniki butlera ze stolu dla par ns w pokoju otwartym i zamknietym
        /// </summary>
        /// <param name="scores">Lista ze wszystkimi zapisami</param>
        public static void CalculateImps(ref List<TablesButlerData> data, int boards_permatch)
        {
            List<int> means = GetAllMeans(ref data, boards_permatch);
            

            for (int t = 0; t < data.Count; t++)
            {
                int sumaOR = 0;
                data[t].impyOR = new int[data[t].scoresOR.Count];
                data[t].impyCR = new int[data[t].scoresCR.Count];

                //open room
                for (int b = 0; b < boards_permatch; b++)
                {
                    int saldo = data[t].scoresOR[b] - means[b];
                    sumaOR += wylicz_impy(saldo);
                    data[t].impyOR[b] = wylicz_impy(saldo);
                }
     
                //closed room
                int sumaCR = 0;
                for (int b = 0; b < boards_permatch; b++)
                {
                    int saldo = data[t].scoresCR[b] - means[b];
                    sumaCR += wylicz_impy(saldo);
                    data[t].impyCR[b] = wylicz_impy(saldo);
                }

                data[t].impsOR = sumaOR;
                data[t].impsCR = sumaCR;
            }
        }

        /// <summary>
        /// Funkcja wylicza srednie wszystkich rozdan na podstawie podanych wynikow wszystkich stolow wszystkich rozdan
        /// </summary>
        /// <param name="allscores">Lista wyników ze stołów ze wszystkich rozdań allscores[stol][rozdanie*wsp]</param>
        /// <param name="boards">Ilosc rozdan</param>
        /// <returns>Lista ze srednimi z poszczegolnych rozdan</returns>
        private static List<int> GetAllMeans(ref List<TablesButlerData> data, int boards)
        {
            List<int> results = new List<int>();

            for (int b = 0; b < boards; b++)
            {
                List<int> boardsresult = new List<int>();
                for (int t = 0; t < data.Count; t++)
                {
                    boardsresult.Add(data[t].scoresOR[b]); //pokoj otwarty
                    boardsresult.Add(data[t].scoresCR[b]); //pokoj zamkniety
                }

                int mean = ObliczSrednia(boardsresult);
                results.Add(mean);
            }

            return results;
        }

        /// <summary>
        /// Oblicza srednia na podstawie zadanych parametrow i tablicy z wynikami
        /// </summary>
        /// <param name="scores">Lista z wynikami z danego rozdania</param>
        /// <param name="ile_obciac_zapisow">Ilosc zapisow do obciecia - domyslnie 0</param>
        /// <returns>Srednia z rozdania dla NS</returns>
        private static int ObliczSrednia(List<int> scores, int ile_obciac_zapisow = 0)
        {
            int count = scores.Count;
            int suma = 0;

            if (ile_obciac_zapisow > 0)
            {
                // Array.Sort<int>(scores);
                scores.Sort();
            }

            for (int j = ile_obciac_zapisow; j < (count - ile_obciac_zapisow); j++)
            {
                suma += scores[j];
            }

            int mean = (int)((suma + 0.5) / (count - ile_obciac_zapisow * 2));

            return mean;
        }

        private static int wylicz_impy(int saldo)
        {
            int imp = 0;
            for (; imp < 24; imp++)
            {
                if (Math.Abs(saldo) <= tabela_imp[imp])
                    break;
            }

            if (saldo < 0)
                return -imp;
            else
                return imp;
        }

  

    }

 
}
