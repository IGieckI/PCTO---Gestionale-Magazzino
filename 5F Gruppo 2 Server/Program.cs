using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading;
using System.Data.SqlClient;

namespace _5F_Gruppo_2_Server
{
    class Program
    {
        static string _nomePCDB = "DESKTOP-CDHTOA2";
        static void Main(string[] args)
        {
            Console.WriteLine("Programma Server\n\n");

            //ottengo l'ip del destinatario
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[1];
            //IPAddress iPAddress = IPAddress.Parse("10.12.0.28");
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000); //creo un endpoint con il mio ip e la porta di comunicazione
            Console.WriteLine("IP: " + iPAddress.ToString());
            List<Thread> threads = new List<Thread>();

            Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
            try
            {
                listener.Bind(localEndPoint); //bind della socket
                listener.Listen(10); //numero massimo client
                bool end = false;
                while (!end)
                {
                    Console.WriteLine("Waiting for connection. . .");
                    Socket handler = null; //dichiara la socket
                    handler = listener.Accept(); //accetta un client, è bloccante

                    threads.Add(new Thread(() => Connection(handler)));
                    threads[threads.Count - 1].Start();

                    while (Console.KeyAvailable) //controlla se ci sono tasti premuti
                    {
                        if (Console.ReadKey(true).Key == ConsoleKey.Q) //se q è stata premuta almeno 1 volta
                            end = true; //chiude il server
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); //mostra l'errore
            }
        }
        static void Connection(Socket h)
        {
            byte[] bytes = new byte[1024]; //buffer dei dati
            bool k = false;
            while (!k)
            {
                string data = null; //messaggio ricevuto
                int bytesRec = h.Receive(bytes); //riceve i bytes
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec); //trasforma in stringa
                byte[] bytearr = new byte[1024];
                //bytearr = Encoding.ASCII.GetBytes(s);
                //h.Send(bytearr); //invia al client un messaggio

                string connectionString;
                SqlConnection cnn;
                //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
                connectionString = $@"Data Source={_nomePCDB};Initial Catalog=Magazzino;Integrated Security=SSPI;";
                cnn = new SqlConnection(connectionString);
                SqlDataReader OutPutSelectAll;
                SqlCommand command;
                String sql;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string[] Elementi = data.Split('|');
                cnn.Open();

                sql = "SELECT * FROM dbo.Utenti";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                bool found = false;
                while (OutPutSelectAll.Read() && found == false)
                {
                    if (Elementi[0] == OutPutSelectAll[0].ToString() && Elementi[1] == OutPutSelectAll[1].ToString())
                        found = true;
                }

                OutPutSelectAll.Close();
                try
                {
                    if (Elementi[2] == "check")
                    {
                        if (found)
                            h.Send(Encoding.ASCII.GetBytes("004"));
                        else
                            h.Send(Encoding.ASCII.GetBytes("003"));
                    }
                    else
                    {
                        if (found == true)
                        {
                            string datas = "";
                            for (int i = 2; i < Elementi.Length; i++)
                            {
                                datas += Elementi[i];
                                if (i < Elementi.Length - 1)
                                    datas += "|";
                            }
                            SqlCSharp sqlCSharp = new SqlCSharp();
                            h.Send(Encoding.ASCII.GetBytes(sqlCSharp.Operation(datas)));
                        }
                        else
                        {
                            bytearr = Encoding.ASCII.GetBytes("003");
                            h.Send(bytearr); //invia al client un messaggio
                        }
                    }
                }
                catch(IndexOutOfRangeException ex)
                {
                    h.Send(Encoding.ASCII.GetBytes("000"));
                }                
                
                k = true; //se c'è si offende e chiude la connesione
                h.Shutdown(SocketShutdown.Both); //aspetta che la socket finisca di fare ciò che sta facendo e chiude la socket
                h.Close();
            }
        }
        static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
