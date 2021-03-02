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
            try
            { 
                /*
                 * ADD|000|2
                 * ADD|000|10|Mercedes|ModelloX
                 * 
                 * WITHDRAW|000|2
                 */

                string connectionString;
                SqlConnection cnn;
                connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
                cnn = new SqlConnection(connectionString);
                SqlDataReader lol;

                SqlCommand command;
                String sql;
                SqlDataAdapter adapter = new SqlDataAdapter();

                while (true)
                {
                    Console.Write("Inserire il comando-> ");
                    string ComandoDaClient = Console.ReadLine();
                    
                    //string ComandoDaClient = "ADD|00000000;
                    //string ComandoDaClient = "WITHDRAW|00000000;
                    //string ComandoDaClient = "SHOW";
                    //string ComandoDaClient = "ADD|00000000|7";
                    //string ComandoDaClient = "ADD|00000001|5|'Mercedes'|'C50'";

                    string[] Elementi = ComandoDaClient.Split('|');
                    cnn.Open();

                    if (Elementi[0] == "ADD" && Elementi.Length==2)
                    {
                        sql = "UPDATE dbo.Pezzo SET Quantità=Quantità+1 " + "WHERE Codice_Prodotto=" + Elementi[1];
                        command = new SqlCommand(sql, cnn);
                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else if(Elementi[0] == "ADD" && Elementi.Length == 3)
                    {
                        sql = "UPDATE dbo.Pezzo SET Quantità=Quantità+"+Elementi[2]+" WHERE Codice_Prodotto=" + Elementi[1];
                        command = new SqlCommand(sql, cnn);
                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else if(Elementi[0] == "ADDNEW" && Elementi.Length == 5)
                    {
                        sql = "INSERT INTO dbo.Pezzo(Codice_Prodotto, Quantità, Marca, Modello) VALUES(" + Elementi[1] + "," + Elementi[2] + "," + Elementi[3] + "," + Elementi[4]+")";
                        command = new SqlCommand(sql, cnn);
                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else if (Elementi[0] == "WITHDRAW")
                    {
                        sql = "SELECT Quantità FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];
                        command = new SqlCommand(sql, cnn);
                        int s = ((int)(command.ExecuteScalar()));

                        if (s - 1 < 0)
                        {
                            throw new InvalidExpressionException("...");
                        }
                        else
                        {
                            sql = "UPDATE dbo.Pezzo SET Quantità=Quantità-1 " + "WHERE Codice_Prodotto=" + Elementi[1];
                            command = new SqlCommand(sql, cnn);
                            adapter.InsertCommand = new SqlCommand(sql, cnn);
                            adapter.InsertCommand.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                    else if(Elementi[0] == "SHOW")
                    {

                        sql = "SELECT * FROM dbo.Pezzo";
                        command = new SqlCommand(sql, cnn);
                        lol = command.ExecuteReader();
                        Console.WriteLine(lol.GetName(0) + "   " + lol.GetName(1) + "      " + lol.GetName(2) + "                         " + lol.GetName(3));
                        while (lol.Read())
                        {
                            Console.WriteLine(lol[0] + "                 " + lol[1] + "           " + lol[2] + " " + lol[3]);
                        }
                        lol.Close();
                    }
                    cnn.Close();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("lool " + ex.Message);
                Console.Read();
            }
        }
    }
}