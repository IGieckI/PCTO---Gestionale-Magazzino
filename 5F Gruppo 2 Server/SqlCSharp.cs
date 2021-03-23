using System;
using System.Data;
using System.Data.SqlClient;

namespace _5F_Gruppo_2_Server
{
    class SqlCSharp
    {
        static string _nomePCDB = "DESKTOP-CDHTOA2";
        //Dario Bekic 5F Parte C# SQL PCTO
        static void ModificaSuPezzo(string comando, out SqlCommand command, SqlConnection cnn, SqlDataAdapter adapter)
        {
            command = new SqlCommand(comando, cnn);
            adapter.InsertCommand = new SqlCommand(comando, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
        }
        static string Operation(string Comando)
        {
            string connectionString;
            SqlConnection cnn;
            //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
            connectionString = $@"Data Source={_nomePCDB};Initial Catalog=Magazzino;Integrated Security=SSPI;";
            cnn = new SqlConnection(connectionString);
            SqlDataReader OutPutSelectAll;
            SqlCommand command;
            String sql;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string ComandoDaClient = Comando;

            string[] Elementi = ComandoDaClient.Split('|');
            cnn.Open();

            if (IsThereError000(Elementi[0], Elementi.Length))
                return "000|" + Elementi[1];

            if (Elementi[0] == "add")
            {
                sql = "SELECT Codice_Prodotto FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];
                command = new SqlCommand(sql, cnn);

                OutPutSelectAll = command.ExecuteReader();
                if (OutPutSelectAll.FieldCount == 1)
                    ModificaSuPezzo("UPDATE dbo.Pezzo SET Quantità=Quantità+" + Elementi[2] + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                else
                    return "008|" + Elementi[1];
            }
            else if (Elementi[0] == "new")
            {
                sql = "SELECT Codice_Prodotto FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];
                command = new SqlCommand(sql, cnn);

                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                {
                    if (!OutPutSelectAll.IsDBNull(0))
                        return "007|" + Elementi[1];
                }
                OutPutSelectAll.Close();
                sql = "SELECT Nome_Prodotto FROM dbo.Pezzo WHERE Nome_Prodotto='" + Elementi[3] + "'";

                command = new SqlCommand(sql, cnn);

                OutPutSelectAll = command.ExecuteReader();

                if (OutPutSelectAll.HasRows)
                    return "006|" + Elementi[1];

                ModificaSuPezzo("INSERT INTO dbo.Pezzo(Codice_Prodotto, Quantità, Nome_Prodotto) VALUES(" + Elementi[1] + "," + Elementi[2] + ",'" + Elementi[3] + "')", out command, cnn, adapter);
            }
            else if (Elementi[0] == "remove")
            {
                sql = "SELECT Quantità FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];

                command = new SqlCommand(sql, cnn);

                int OutPutSelect = ((int)(command.ExecuteScalar())), QuantitàDaSottrarre = 1;

                QuantitàDaSottrarre = int.Parse(Elementi[2]);

                if (OutPutSelect - QuantitàDaSottrarre < 0)
                    return "009|" + Elementi[1];
                else
                    ModificaSuPezzo("UPDATE dbo.Pezzo SET Quantità=Quantità-" + QuantitàDaSottrarre + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
            }
            else if (Elementi[0] == "select")
            {
                sql = "SELECT * FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                    Console.WriteLine(OutPutSelectAll[0] + "|" + OutPutSelectAll[1] + "|" + OutPutSelectAll[2]);
                OutPutSelectAll.Close();
            }
            else if (Elementi[0] == "database")
            {
                sql = "SELECT * FROM dbo.Pezzo";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read()) //Console.WriteLine(OutPutSelectAll.GetName(0) + "|" + OutPutSelectAll.GetName(1) + "|" + OutPutSelectAll.GetName(2));
                    Console.WriteLine(OutPutSelectAll[0] + "|" + OutPutSelectAll[1] + "|" + OutPutSelectAll[2]);
                OutPutSelectAll.Close();
            }
            else
                return "001";

            cnn.Close();
            return "002";

        }
        static bool IsThereError000(string ActionCode, int ParameterNumber)
        {
            if (ActionCode == "add" && ParameterNumber != 3)
                return true;
            else if (ActionCode == "new" && ParameterNumber != 4)
                return true;
            else if (ActionCode == "remove" && ParameterNumber != 3)
                return true;
            else if (ActionCode == "select" && ParameterNumber != 2)
                return true;
            else if (ActionCode == "database" && ParameterNumber != 1)
                return true;
            else
                return false;
        }
    }
}
