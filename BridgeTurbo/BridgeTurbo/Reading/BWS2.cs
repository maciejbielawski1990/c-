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
    public class BWS2
    {
        OleDbConnection connection;
        public List<BWSStruct> lista;

        public BWS2(string filename)
        {
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            connString += filename;
            connection = new OleDbConnection(connString);
            connection.Open();

            ReadData();
            ReadBidding();
            RepairBidding();
            connection.Close();
        }

        public BWS2(){ }

        void ReadData()
        {
            string query = "SELECT * FROM ReceivedData";
            OleDbCommand command = new OleDbCommand(query, connection);


            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data);

            lista = new List<BWSStruct>();

            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                BWSStruct node = new BWSStruct();
                int tables = int.Parse(data.Tables[0].Rows[i]["Table"].ToString());
                node.table = tables;
                node.ns = int.Parse(data.Tables[0].Rows[i]["PairNS"].ToString());
                node.ew = int.Parse(data.Tables[0].Rows[i]["PairEW"].ToString());
                node.round = int.Parse(data.Tables[0].Rows[i]["Round"].ToString());
                node.section = int.Parse(data.Tables[0].Rows[i]["Section"].ToString());
                node.boardNr = int.Parse(data.Tables[0].Rows[i]["Board"].ToString());
                //contract
                string contr_ = data.Tables[0].Rows[i]["Contract"].ToString().Replace(" ", "");
                contr_ = contr_.Replace("T", "");
                string declarer_ = data.Tables[0].Rows[i]["NS/EW"].ToString();
                try
                {
                    string contract_ = contr_.Insert(2, declarer_) + data.Tables[0].Rows[i]["Result"].ToString();

                    node.contract = new Contract(contract_);

                    lista.Add(node);
                }
                catch { }
            }
        }

        void ReadBidding()
        {
            string query = "SELECT * FROM BiddingData";
            OleDbCommand command = new OleDbCommand(query, connection);


            OleDbDataAdapter AdapterTabela = new OleDbDataAdapter(command);
            DataSet data = new DataSet();
            AdapterTabela.Fill(data);

            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                int tables = int.Parse(data.Tables[0].Rows[i]["Table"].ToString());
                int round = int.Parse(data.Tables[0].Rows[i]["Round"].ToString());
                int board = int.Parse(data.Tables[0].Rows[i]["Board"].ToString());

                string bid = data.Tables[0].Rows[i]["Bid"].ToString();
                if (bid == "PASS") bid = "p";
                if (bid == "X") bid = "d";
                if (bid == "XX") bid = "r";

                int result = lista.FindIndex(item => ((item.boardNr == board) && (item.table == tables) && (item.round == round))); 
                int idx = int.Parse(data.Tables[0].Rows[i]["Counter"].ToString());
                if (result >= 0)
                    lista[result].bidding[idx] = bid;
            }
        }

        void RepairBidding()
        {
            for (int i = 0; i < lista.Count(); i++)
            {
                lista[i].Bidding = new List<Bidding>();
                int idx = 1;
                do
                {
                    Bidding b = new Bidding();
                    b.odzywka = lista[i].bidding[idx++];
                    lista[i].Bidding.Add(b);
                } while (lista[i].bidding[idx] != null);

                lista[i].bidding = null;
                
            }
        }

    }
}
