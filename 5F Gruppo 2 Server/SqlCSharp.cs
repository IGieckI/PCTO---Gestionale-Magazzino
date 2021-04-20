using System;
using System.Data;
using System.Data.SqlClient;

namespace _5F_Gruppo_2_Server
{
    class SqlCSharp
    {
        //Dario Bekic 5F Parte C# SQL PCTO
        string _nomePCDB = "PC1229"; //Nome del server

        public SqlCSharp()//Mantiene le impostazioni di default
        {}
        public SqlCSharp(string nomePCDB)//PC e nome DB personalizzati
        {
            _nomePCDB = nomePCDB;
        }
        /// <summary>
        /// Comando per eseguire il comando non query che cambierà il database;
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="command"></param>
        /// <param name="cnn"></param>
        /// <param name="adapter"></param>
        void ModificaSuPezzo(string comando, out SqlCommand command, SqlConnection cnn, SqlDataAdapter adapter)
        {
            command = new SqlCommand(comando, cnn); //Crea il comando
            adapter.InsertCommand = new SqlCommand(comando, cnn); 
            adapter.InsertCommand.ExecuteNonQuery(); //Esegue il comando
            command.Dispose();
        }
        /// <summary>
        /// Metodo per eseguire un operazione sulla tabella dei pezzi del magazzino;
        /// </summary>
        /// <param name="Comando"></param>
        /// <returns></returns>
        public string Operation(string Comando)
        {
            string connectionString; //Stringa che conterrà i parametri per connettersi ad SQLServer
            SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020"); //Un oggetto di tipo sqlconnection
            //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
            SqlDataReader OutPutSelectAll; //Oggetto che conterrà l'output dei comandi sql che producono un output
            SqlCommand command; //Oggetto che conterrà il comando che di volta in volta cambierà a seconda delle richieste dell'utente
            String sql; //Stringa che conterrà la sintassi del comando;
            SqlDataAdapter adapter = new SqlDataAdapter(); 
            string ComandoDaClient = Comando; 

            string[] Elementi = ComandoDaClient.Split('|'); //Splitta il comando in una serie di stringhe così da facilitare l'analisi del comando.
            cnn.Open();//Apre la connessione

            if (IsThereError000(Elementi[0], Elementi.Length)) //L'errore 000 sta a significare un incorretto numero di parametri per quel determinato comando;
            {
                cnn.Close(); //Chiude la connessione se incappa in un errore;
                return "000|" + Elementi[1]; //Ritorna il codice d'errore e il comando che agiva sol
            }

            else if (Elementi[0] == "add")
            {
                sql = $"SELECT * FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader(); //Esegue il comando
                int count = 0; //Intero per vedere se esiste un outpuit
                while (OutPutSelectAll.Read())  //Se esiste allora aumenta count
                {
                    count++;
                }

                if (count>0)//Se count è aumentato allora significa che esiste il prodotto e quindi va aumentato
                {
                    OutPutSelectAll.Close();
                    ModificaSuPezzo("UPDATE dbo.Prodotti SET Quantità=Quantità+" + Elementi[2] + " WHERE Codice_Prodotto=" + Elementi[1], out command, cnn, adapter);
                }                    
                else
                {
                    cnn.Close(); //Chiude la conn in caso di errore
                    return "008|" + Elementi[1]; 
                }
            }
            else if (Elementi[0] == "new")
            {
                sql = $"SELECT Codice_Prodotto FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader(); //Esegue il comando
                while (OutPutSelectAll.Read()) //Legge e qui entra solo quindi se l'output c'è ergo....si sta cercando d'inserire un prodotto con un codice già esistente(qui controlla il codice)!!! 
                {
                    cnn.Close(); //Chiude la conn in caso di errore
                    return "007|" + Elementi[3];
                }
                OutPutSelectAll.Close(); //Chiude il datareader per riaprirlo più avanti in caso di non errore

                sql = $"SELECT Nome_Prodotto FROM dbo.Prodotti WHERE Nome_Prodotto='{Elementi[3]}'";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();// Esegue il comando
                while (OutPutSelectAll.Read())//Legge e qui entra solo quindi se l'outpu c'è ergo...si sta cercando d'inserire un prodotto con un nome già esistente!                                               
                {
                    cnn.Close();
                    return "006|" + Elementi[3];
                }
                OutPutSelectAll.Close();//Chiude il datareader

                ModificaSuPezzo($"INSERT INTO dbo.Prodotti(Codice_Prodotto, Quantità, Nome_Prodotto) VALUES('{Elementi[1]}',{Elementi[2]},'{Elementi[3]}')", out command, cnn, adapter);
            }
            else if (Elementi[0] == "remove") //Rimuove
            {
                try
                {
                    sql = $"SELECT Quantità FROM dbo.Prodotti WHERE Codice_Prodotto='{Elementi[1]}'";
                    command = new SqlCommand(sql, cnn);
                    int OutPutSelect = ((int)(command.ExecuteScalar())), QuantitàDaSottrarre = 1; //Imposta la quantità sottratta di default ad 1
                    QuantitàDaSottrarre = int.Parse(Elementi[2]);
                    if (OutPutSelect - QuantitàDaSottrarre < 0)//Se si sfora lo 0 produce errore
                    {
                        cnn.Close();//Chiude la conn in caso di errore
                        return "009|" + Elementi[1];
                    }
                    else
                        ModificaSuPezzo($"UPDATE dbo.Prodotti SET Quantità=Quantità-{QuantitàDaSottrarre} WHERE Codice_Prodotto= {Elementi[1]}", out command, cnn, adapter);
                }
                catch(Exception ex)
                {
                    return "005|" + Elementi[1];
                }
            }
            else if (Elementi[0] == "select") //Select di una riga
            {
                sql = "SELECT * FROM dbo.Prodotti WHERE Codice_Prodotto=" + Elementi[1];
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader(); //Esegue il comando del select
                string risposta = "";
                if (OutPutSelectAll.Read()) //Se esiste un output
                    risposta += $"002|{OutPutSelectAll[0]}|{OutPutSelectAll[1]}|{OutPutSelectAll[2]}"; //Risponde con tale riga
                else
                {
                    OutPutSelectAll.Close();
                    cnn.Close(); //Chiude la conn in caso di errore
                    return $"005|{Elementi[1]}";
                }
                OutPutSelectAll.Close();
                cnn.Close();
                return risposta; //Ritorna la risposta(Cioè la riga)
            }
            else if (Elementi[0] == "database") //Comando per ricevere la tabella dei pezzi per intero
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
                return risposta2; //Restituisce la tabella
            }
            else
                return "001";//Non esiste il codice d'azione inserito

            cnn.Close();
            return $"002|{Elementi[1]}";//Il codice 002 significa che non da errori;

        }
        /// <summary>
        /// Controlla per ogni comando possibile se il numero dei parametri è errato; 
        /// In caso sia errato restituisce true
        /// </summary>
        /// <param name="ActionCode"></param>
        /// <param name="ParameterNumber"></param>
        /// <returns></returns>
        bool IsThereError000(string ActionCode, int ParameterNumber)
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
