using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppServer
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Programma Server\n\n");

            byte[] bytes = new byte[1024]; //buffer dei dati
            string data = null;
            //ottengo l'ip del destinatario
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[2];
            //IPAddress iPAddress = IPAddress.Parse("10.12.0.28");
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000); //creo un endpoint con il mio ip e la porta di comunicazione
            Console.WriteLine("IP: " + iPAddress.ToString());

            Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
            try
            {
                listener.Bind(localEndPoint); //bind della socket
                listener.Listen(10); //numero massimo client
                bool end = false;
                bool bruh = true; //serve per terminare la connessione
                while (!end)
                {
                    Console.WriteLine("Waiting for connection. . .");
                    Socket handler = null; //dichiara la socket
                    if (bruh) //se non è connesso a un client
                    {
                        handler = listener.Accept(); //accetta un client, è bloccante
                        bruh = false;
                    }
                    while (!bruh)
                    {
                        data = null; //messaggio ricevuto
                        bool ok = false;
                        while (!ok)
                        {
                            /*int bytesRec = handler.Receive(bytes); //riceve i bytes
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec); //trasforma in stringa*/
                            ok = true;
                        }
                        bruh = true; //se c'è si offende e chiude la connesione
                    }
                    handler.Shutdown(SocketShutdown.Both); //aspetta che la socket finisca di fare ciò che sta facendo e chiude la socket
                    handler.Close();
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
    }
}
