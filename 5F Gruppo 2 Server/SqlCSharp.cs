using System;
using System.Data;
using System.Data.SqlClient;

namespace _5F_Gruppo_2_Server
{
    class SqlCSharp
    {
        string _nomePCDB = "DESKTOP-CDHTOA2";
        string _nomeDB = "Magazzino";

        public SqlCSharp()//Mantiene le impostazioni di default
        {

        }

        public SqlCSharp(string nomePCDB)//PC e nome DB personalizzati
        {
            _nomePCDB = nomePCDB;
        }

        //Dario Bekic 5F Parte C# SQL PCTO
        void ModificaSuPezzo(string comando, out SqlCommand command, SqlConnection cnn, SqlDataAdapter adapter)
        {
            command = new SqlCommand(comando, cnn);
            adapter.InsertCommand = new SqlCommand(comando, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
        }
        public string Operation(string Comando)
        {
            string connectionString;
            SqlConnection cnn;
            //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
            connectionString = $@"Data Source={_nomePCDB};Initial Catalog={_nomeDB};Integrated Security=SSPI;";
            cnn = new SqlConnection(connectionString);
            SqlDataReader OutPutSelectAll;
            SqlCommand command;
            String sql;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string ComandoDaClient = Comando;

            string[] Elementi = ComandoDaClient.Split('|');
            cnn.Open();

            if (IsThereError000(Elementi[0], Elementi.Length))
            {
                cnn.Close();
                return "000|" + Elementi[1];
            }

            else if (Elementi[0] == "add")
            {
                sql = $"SELECT * FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                int count = 0;
                while (OutPutSelectAll.Read())
                {
                    count++;
                }

                if (count>0)
                {
                    OutPutSelectAll.Close();
                    ModificaSuPezzo("UPDATE dbo.Prodotti SET Quantità=Quantità+" + Elementi[2] + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                }                    
                else
                {
                    cnn.Close();
                    return "008|" + Elementi[1];
                }
            }
            else if (Elementi[0] == "new")
            {
                sql = $"SELECT Codice_Prodotto FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                {
                    cnn.Close();
                    return "007|" + Elementi[3];
                }
                OutPutSelectAll.Close();

                sql = $"SELECT Nome_Prodotto FROM dbo.Prodotti WHERE Nome_Prodotto='{Elementi[3]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                {
                    cnn.Close();
                    return "006|" + Elementi[3];
                }
                OutPutSelectAll.Close();

                ModificaSuPezzo($"INSERT INTO dbo.Prodotti(Codice_Prodotto, Quantità, Nome_Prodotto) VALUES('{Elementi[1]}',{Elementi[2]},'{Elementi[3]}')", out command, cnn, adapter);
            }
            else if (Elementi[0] == "remove")
            {
                sql = $"SELECT Quantità FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                command = new SqlCommand(sql, cnn);
                int OutPutSelect = ((int)(command.ExecuteScalar())), QuantitàDaSottrarre = 1;
                QuantitàDaSottrarre = int.Parse(Elementi[2]);
                if (OutPutSelect - QuantitàDaSottrarre < 0)
                {
                    cnn.Close();
                    return "009|" + Elementi[3];
                }
                else
                    ModificaSuPezzo($"UPDATE dbo.Prodotti SET Quantità=Quantità-{QuantitàDaSottrarre} WHERE Codice_Prodotto= {Elementi[1]}", out command, cnn, adapter);
            }
            else if (Elementi[0] == "select")
            {
                sql = "SELECT * FROM dbo.Prodotti WHERE Codice_Prodotto=" + Elementi[1];
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                string risposta = "";
                if (OutPutSelectAll.Read())
                    risposta += $"002|{OutPutSelectAll[0]}|{OutPutSelectAll[1]}|{OutPutSelectAll[2]}";
                else
                {
                    OutPutSelectAll.Close();
                    cnn.Close();
                    return $"005|{Elementi[1]}";
                }
                OutPutSelectAll.Close();
                cnn.Close();
                return risposta;
            }
            else if (Elementi[0] == "database")
            {
                sql = "SELECT * FROM dbo.Prodotti";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                string risposta = "database|";
                while (OutPutSelectAll.Read()) //Console.WriteLine(OutPutSelectAll.GetName(0) + "|" + OutPutSelectAll.GetName(1) + "|" + OutPutSelectAll.GetName(2));
                {
                    risposta += $"{OutPutSelectAll[0]}│{OutPutSelectAll[1]}│{OutPutSelectAll[2]}|";                    
                }
                OutPutSelectAll.Close();
                cnn.Close();
                string risposta2 = "";
                for(int i=0;i<risposta.Length-1;i++)
                    risposta2 += risposta[i];
                return risposta2;
            }
            else
                return "001";

            cnn.Close();
            return $"002|{Elementi[1]}";

        }
        bool IsThereError000(string ActionCode, int ParameterNumber)
        {
            if (ActionCode == "add" && ParameterNumber == 3)
                return false;
            else if (ActionCode == "new" && ParameterNumber == 4)
                return false;
            else if (ActionCode == "remove" && ParameterNumber == 3)
                return false;
            else if (ActionCode == "select" && ParameterNumber == 2)
                return false;
            else if (ActionCode == "database" && ParameterNumber == 1)
                return false;
            else
                return true;
        }
    }
}
