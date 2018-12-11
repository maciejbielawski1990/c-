using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butler
{
    class BaseEditor
    {
        public BaseEditor() { }
        


        public static void ObslugaBazy(ref List<ButlerPlayer> baza, List<TablesButlerData> tableData)
        {
            foreach (TablesButlerData tdata in tableData)
            {
                for (int p = 0; p < 8; p++)
                {
                    string player_surn = tdata.players[p];
                    ButlerPlayer player_ = baza.Find(item => item.name == player_surn);

                    if (player_ != null)
                        AddMatchToPlayer(ref player_, tdata, p);
                    else
                    {
                       player_ =  AddNewPlayer(tdata,p);
                       baza.Add(player_);
                    }
                }
            }



        }

        private static void AddMatchToPlayer(ref ButlerPlayer player, TablesButlerData tData, int pos)
        {
            if (NS(pos) && openRoom(pos)) //NS open
            {
                player.imps.Add(tData.impsOR);
                player.opponentsTeam.Add(tData.TeamAway);
                player.opponentsPlayer.Add(tData.players[5] + " " + tData.players[6]);
              

                player.impyzrozdaniami.Add(tData.impyOR);

            }

            if (NS(pos) && !openRoom(pos)) //NS closed
            {
                player.imps.Add(tData.impsCR);
                player.opponentsTeam.Add(tData.TeamHome);
                player.opponentsPlayer.Add(tData.players[1] + " " + tData.players[2]);

             
             
                    player.impyzrozdaniami.Add(tData.impyCR);

            }

            if (!NS(pos) && openRoom(pos)) //EW Open
            {
                player.imps.Add(-tData.impsOR);
                player.opponentsTeam.Add(tData.TeamHome);
                player.opponentsPlayer.Add(tData.players[0] +" " + tData.players[3]);
                
                
                int[] tmp = tData.impyOR;
                for (int i = 0; i < tData.impyOR.Count(); i++)
                {
                    tmp[i] = -tmp[i];
                }

                player.impyzrozdaniami.Add(tmp);
            }

            if (!NS(pos) && !openRoom(pos)) //EW Closed
            {
                player.imps.Add(-tData.impsCR);
                player.opponentsTeam.Add(tData.TeamAway);
                player.opponentsPlayer.Add(tData.players[4] + " " + tData.players[7]);

          
                int[] tmp = tData.impyCR;
                for (int i = 0; i < tData.impyCR.Count(); i++)
                {
                    tmp[i] = -tmp[i];
                }

                player.impyzrozdaniami.Add(tmp);
            }

        }

        private static ButlerPlayer AddNewPlayer(TablesButlerData tData, int pos)
        {
            ButlerPlayer player = new ButlerPlayer();
            player.opponentsPlayer = new List<string>();
            player.opponentsTeam = new List<string>();
            player.imps = new List<int>();
            player.name = tData.players[pos];
            player.impyzrozdaniami = new List<int[]>();

            if (NS(pos) && openRoom(pos)) //NS open
            {
                player.imps.Add(tData.impsOR);
                player.opponentsTeam.Add(tData.TeamAway);
                player.opponentsPlayer.Add(tData.players[5] + " " + tData.players[6]);
                player.team = tData.TeamHome;
                
                player.impyzrozdaniami.Add(tData.impyOR);
      
            }

            if (NS(pos) && !openRoom(pos)) //NS closed
            {
                player.imps.Add(tData.impsCR);
                player.opponentsTeam.Add(tData.TeamHome);
                player.opponentsPlayer.Add(tData.players[1] + " " + tData.players[2]);
                player.team = tData.TeamAway;

                player.impyzrozdaniami.Add(tData.impyCR);
            }

            if (!NS(pos) && openRoom(pos)) //EW Open
            {
                player.imps.Add(-tData.impsOR);
                player.opponentsTeam.Add(tData.TeamHome);
                player.opponentsPlayer.Add(tData.players[0] + " " + tData.players[3]);
                player.team = tData.TeamAway;

                int[] tmp = tData.impyOR;
                for (int i = 0; i < tData.impyOR.Count(); i++)
                {
                    tmp[i] = -tmp[i];
                }

                player.impyzrozdaniami.Add(tmp);
                
            }

            if (!NS(pos) && !openRoom(pos)) //EW Closed
            {
                player.imps.Add(-tData.impsCR);
                player.opponentsTeam.Add(tData.TeamAway);
                player.opponentsPlayer.Add(tData.players[4] + " " + tData.players[7]);
                player.team = tData.TeamHome;
                
                int[] tmp = tData.impyCR;
                for (int i = 0; i < tData.impyCR.Count(); i++)
                {
                    tmp[i] = -tmp[i];
                }

                player.impyzrozdaniami.Add(tmp);
            }

            return player;
        }


        private static bool NS(int pos)
        {
            if ((pos == 0) || (pos == 3) || (pos == 4) || (pos == 7))
                return true;
            else
                return false;
        }

        private static bool openRoom(int pos)
        {
            if ((pos == 0) || (pos == 3) || (pos == 5) || (pos == 6))
                return true;
            else
                return false;
        }

    }
}
