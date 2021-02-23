using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PCTO_DB
{
    class Program
    {
        //Dario Bekic 5F Parte C# SQL PCTO
        static void Main(string[] args)
        {
            /*
             * ADD|000|2
             * ADD|000|1|Mercedes|ModelloX
             * ADDNEW|000|1|Marca|Modello
             * WITHDRAW|000|2|
             */

            string connectionString;
            SqlConnection cnn;
            connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
            cnn = new SqlConnection(connectionString);
            SqlDataReader lol;
            cnn.Open();

            SqlCommand command;
            String sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            //string ComandoDaClient = "ADD|00000000|3";
            //string ComandoDaClient = "ADD|00000001|2|Mercedes|X50";
            string ComandoDaClient = "WITHDRAW|00000001|1";
            string[] Elementi = ComandoDaClient.Split('|');

            if (Elementi[0] == "ADD" && Elementi.Length == 3)
            {
                sql = $"UPDATE dbo.Pezzo SET Quantità=Quantità+" + Elementi[2] + "WHERE Codice_Prodotto="+Elementi[1];
                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();
            }
            else if (Elementi[0] == "ADD" && Elementi.Length > 3)
            {
                sql = $"INSERT INTO dbo.Pezzo(Codice_Prodotto, Quantità,Marca,Modello) VALUES(" + Elementi[1] + "," + Elementi[2] + ",'" + Elementi[3] + "','" + Elementi[4] + "')";
                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();
            }
            else if(Elementi[0] == "WITHDRAW")
            {
                sql = "SELECT Quantità FROM dbo.Pezzo WHERE Codice_Prodotto="+Elementi[1];
                command = new SqlCommand(sql, cnn);
                int s = (int) command.ExecuteScalar();
                if(s-Convert.ToInt32(Elementi[2]) == 0)
                {
                    sql = "DELETE FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[0];
                    command = new SqlCommand(sql, cnn);
                }
                else if(s-Convert.ToInt32(Elementi[2]) < 0)
                {
                    //Ecezzione
                }
                else
                {
                    sql = "UPDATE dbo.Pezzo SET Quantità=Quantità-" + Elementi[2];
                    command = new SqlCommand(sql, cnn);
                    command.ExecuteNonQuery();
                } 
            }
            Console.Read();

        }
    }
}
