using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024]; //buffer dei dati
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[1];
                //IPAddress ipAddress = IPAddress.Parse("10.12.0.28");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
                sender.Connect(remoteEP); //connette all'endpoint
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                //string messaggio = ""; //messaggio da inviare
                string ricevuto = null; //messsaggio ricevuto
                //Console.WriteLine("Inserire Alias: ");
                //messaggio = Console.ReadLine(); //legge messaggi
                //string alias = messaggio;
                //byte[] msg = Encoding.ASCII.GetBytes(messaggio); //crea un messaggio
                //int bytesSent = sender.Send(msg); //manda un messaggio
                bool ok = false;
                while (!ok)
                {
                    ricevuto = null;
                    int bytesRec = sender.Receive(bytes); //riceve i bytes
                    ricevuto += Encoding.ASCII.GetString(bytes, 0, bytesRec); //trasforma in stringa
                }
                sender.Shutdown(SocketShutdown.Both); //chiude le socket come nel server
                sender.Close();
            }
            catch (Exception ex)
            { }
        }
    }
}
