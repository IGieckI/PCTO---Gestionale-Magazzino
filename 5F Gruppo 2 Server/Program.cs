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
        static void Main(string[] args)
        {
            Console.WriteLine("Programma Server\n\n");

            string data = null;
            //ottengo l'ip del destinatario
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[2];
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

                    threads.Add(new Thread(() => Connection(listener)));
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
            while(!k)
            {
                string data = null; //messaggio ricevuto
                bool ok = false;
                while (!ok)
                {
                    int bytesRec = h.Receive(bytes); //riceve i bytes
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec); //trasforma in stringa
                    byte[] bytearr = new byte[1024];
                    //bytearr = Encoding.ASCII.GetBytes(s);
                    //h.Send(bytearr); //invia al client un messaggio

                    string connectionString;
                    SqlConnection cnn;
                    //connectionString = @"Data Source=PC1227;Initial Catalog=Magazzino;User ID=sa;Password=burbero2020";
                    connectionString = @"Data Source=LAPTOP-HKOJICES;Initial Catalog=Magazzino;Integrated Security=SSPI;";
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
                    Encryption en = new Encryption();
                    bool found = false;
                    while(OutPutSelectAll.Read() && found == false)
                    {
                        if (Elementi[0] == OutPutSelectAll[0].ToString() && Elementi[1] == en.Decrypt(OutPutSelectAll[1].ToString()))
                            found = true;
                    }
                    OutPutSelectAll.Close();
                    if(found == true)
                    {
                        switch (Elementi[2])
                        {
                            case "new":
                                string cmd = "SELECT * FROM dbo.Pezzo";
                                command = new SqlCommand(cmd, cnn);
                                OutPutSelectAll = command.ExecuteReader();
                                bool error = false;
                                while(OutPutSelectAll.Read() && error == false)
                                {
                                    if (Elementi[3] == OutPutSelectAll[0].ToString() || Elementi[5] == OutPutSelectAll[3].ToString())
                                        error = true;
                                }

                                break;
                            case "add":
                                break;
                            case "remove":
                                break;
                            case "check":
                                bytearr = Encoding.ASCII.GetBytes("004|");
                                h.Send(bytearr); //invia al client un messaggio
                                break;
                            case "database":
                                break;
                            case "select":
                                break;
                        }
                    }
                    else
                    {
                        bytearr = Encoding.ASCII.GetBytes("003|");
                        h.Send(bytearr); //invia al client un messaggio
                    }
                    ok = true;
                }
                k = true; //se c'è si offende e chiude la connesione
                h.Shutdown(SocketShutdown.Both); //aspetta che la socket finisca di fare ciò che sta facendo e chiude la socket
                h.Close();
            }
        }
    }
}
