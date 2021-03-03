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
                //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
                connectionString = @"Data Source=LAPTOP-HKOJICES;Initial Catalog=Magazzino;Integrated Security=SSPI;";
                cnn = new SqlConnection(connectionString);
                SqlDataReader OutPutSelectAll;
                SqlCommand command;
                String sql;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string ComandoDaClient="";
                while (ComandoDaClient!="Logout")
                {
                    Console.Write("Inserire il comando-> ");
                    ComandoDaClient = Console.ReadLine();

                    string[] Elementi = ComandoDaClient.Split('|');
                    cnn.Open();

                    if (Elementi[0] == "ADD" && Elementi.Length==2)
                    {
                        ModificaSuPezzo("UPDATE dbo.Pezzo SET Quantità=Quantità+1 " + "WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                        ModificaSuPezzo("INSERT INTO dbo.Movimento(QuantitàMossa, DataMovimento, IDProdottoCoinvolto, TipologiaMovimento) VALUES (1,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'," + Elementi[1] + ",'" + Elementi[0] + "')", out command, cnn, adapter);
                    }
                    else if(Elementi[0] == "ADD" && Elementi.Length == 3)
                    {
                        ModificaSuPezzo("UPDATE dbo.Pezzo SET Quantità=Quantità+" + Elementi[2] + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                        ModificaSuPezzo("INSERT INTO dbo.Movimento(QuantitàMossa, DataMovimento, IDProdottoCoinvolto, TipologiaMovimento) VALUES ("+Elementi[2] + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'," + Elementi[1] + ",'" + Elementi[0]+"')", out command, cnn, adapter);
                    }
                    else if(Elementi[0] == "ADDNEW" && Elementi.Length == 5)
                    {
                        ModificaSuPezzo("INSERT INTO dbo.Pezzo(Codice_Prodotto, Quantità, Marca, Modello) VALUES(" + Elementi[1] + "," + Elementi[2] + "," + Elementi[3] + "," + Elementi[4] +")", out command, cnn, adapter);
                        ModificaSuPezzo("INSERT INTO dbo.Movimento(QuantitàMossa, DataMovimento, IDProdottoCoinvolto, TipologiaMovimento) VALUES (" + Elementi[2] + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'," + Elementi[1] + ",'" + Elementi[0]+ "')", out command, cnn, adapter);
                    }
                    else if (Elementi[0] == "WITHDRAW")
                    {
                        sql = "SELECT Quantità FROM dbo.Pezzo WHERE Codice_Prodotto=" + Elementi[1];
                        command = new SqlCommand(sql, cnn);
                        int OutPutSelect = ((int)(command.ExecuteScalar())), QuantitàDaSottrarre = 1;
                        if (Elementi.Length == 3)
                            QuantitàDaSottrarre = int.Parse(Elementi[2]);

                        if (OutPutSelect - QuantitàDaSottrarre < 0)
                            throw new InvalidExpressionException("...");
                        else
                        {
                            ModificaSuPezzo("UPDATE dbo.Pezzo SET Quantità=Quantità-" + QuantitàDaSottrarre + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                            ModificaSuPezzo("INSERT INTO dbo.Movimento(QuantitàMossa, DataMovimento, IDProdottoCoinvolto, TipologiaMovimento) VALUES (" + Elementi[2] + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'," + Elementi[1] + ",'" + Elementi[0] +"')", out command, cnn, adapter);
                        }
                    }
                    else if(Elementi[0] == "SHOW")
                    {
                        if (Elementi[1] == "dbo.Pezzo")
                            sql = "SELECT * FROM dbo.Pezzo";
                        else
                            sql = "SELECT * FROM dbo.Movimento";
                        command = new SqlCommand(sql, cnn);
                        OutPutSelectAll = command.ExecuteReader();
                        if (Elementi[1] == "dbo.Pezzo")
                        {
                            Console.WriteLine(OutPutSelectAll.GetName(0) + "    " + OutPutSelectAll.GetName(1) + "      " + OutPutSelectAll.GetName(2) + "                         " + OutPutSelectAll.GetName(3));
                            while (OutPutSelectAll.Read())
                                Console.WriteLine(OutPutSelectAll[0] + "                 " + OutPutSelectAll[1] + "             " + OutPutSelectAll[2] + "" + OutPutSelectAll[3]);
                        }
                        else
                        {
                            Console.WriteLine(OutPutSelectAll.GetName(0) + "    " + OutPutSelectAll.GetName(1) + "      " + OutPutSelectAll.GetName(2) + "                 " + OutPutSelectAll.GetName(3) +"         "+ OutPutSelectAll.GetName(4));
                            while (OutPutSelectAll.Read())
                                Console.WriteLine(OutPutSelectAll[0] + "              " + OutPutSelectAll[1] + "                  " + OutPutSelectAll[2] + "            " + OutPutSelectAll[3]+"                           "+ OutPutSelectAll[4]);

                        }
                        OutPutSelectAll.Close();
                    }
                    cnn.Close();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                Console.Read();
            }
        }
        static void ModificaSuPezzo(string comando, out SqlCommand command, SqlConnection cnn, SqlDataAdapter adapter)
        {
            command = new SqlCommand(comando, cnn);
            adapter.InsertCommand = new SqlCommand(comando, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
        }
    }
}