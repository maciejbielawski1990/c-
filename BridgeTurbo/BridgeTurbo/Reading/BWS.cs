using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace Bridge.Reading
{
    public class BWS:Lin
    {
        OleDbConnection connection;
        int round;
        BWSStruct[] dane;
        public List<BWSStruct> lista;
   

        public BWS(string filename, int round_)
        {
            round = round_;
            dane = new BWSStruct[100];
            for (int i = 0; i < 100; i++)
                dane[i] = new BWSStruct();
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=r4.bws";
            connection = new OleDbConnection(connString);
            connection.Open();
            ReadNumbers();
            ReadBidding();
            connection.Close();
            lista = new List<BWSStruct>();
            for (int i = 0; i < 100; i++)
            {
                if (dane[i].ns != 0)
                    lista.Add(dane[i]);
            }

        }


        private void ReadNumbers()
        {
            string query = "SELECT * FROM ReceivedData";
            OleDbCommand command = new OleDbCommand(query, connection);


            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data);

        

            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                int tables = int.Parse(data.Tables[0].Rows[i]["Table"].ToString());
                dane[tables].table = tables;
                dane[tables].ns = int.Parse(data.Tables[0].Rows[i]["PairNS"].ToString());
                dane[tables].ew = int.Parse(data.Tables[0].Rows[i]["PairEW"].ToString());

                Board b = new Board();
                b.nr = int.Parse(data.Tables[0].Rows[i]["Board"].ToString());
                string contr_ = data.Tables[0].Rows[i]["Contract"].ToString().Replace(" ","");
                contr_ = contr_.Replace("T", "");
                string dealer_ = data.Tables[0].Rows[i]["NS/EW"].ToString();
                
                string contract_ = contr_.Insert(2, dealer_) + data.Tables[0].Rows[i]["Result"].ToString();

                b.contract = new Contract(contract_);
                b.lead = data.Tables[0].Rows[i]["LeadCard"].ToString();
                bool partia = GetVulnerability(b.nr, b.contract.declarer);
                b.score = b.CalculateScore(partia);
                b.bidding = new List<Bidding>();
                for (int x = 0; x < 30; x++)
                {
                    b.bidding.Add(new Bidding());
                }
                dane[tables].boards.Add(b);
            }
        }


        private void ReadBidding()
        {
            string query = "SELECT * FROM BiddingData";
            OleDbCommand command = new OleDbCommand(query, connection);


            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data);

            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                int tables = int.Parse(data.Tables[0].Rows[i]["Table"].ToString());
                int nr_ = int.Parse(data.Tables[0].Rows[i]["Board"].ToString());
                int counter = int.Parse(data.Tables[0].Rows[i]["Counter"].ToString());
                string bid = data.Tables[0].Rows[i]["Bid"].ToString();

                if (bid == "PASS") bid = "p";
                if (bid == "X") bid = "d";
                if (bid == "XX") bid = "r";

                int result = dane[tables].boards.FindIndex(b => b.nr == nr_);
            
               dane[tables].boards[result].bidding[counter - 1].odzywka = bid;
            }
        }

        
    
    }
}
