using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Client_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /* byte[] bytes = new byte[1024]; //buffer dei dati
             try
             {
                 IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                 IPAddress ipAddress = ipHostInfo.AddressList[2];
                 //IPAddress ipAddress = IPAddress.Parse("10.12.0.28");
                 IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                 Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
                 sender.Connect(remoteEP); //connette all'endpoint
                 Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                 int bytesRec = sender.Send(bytes);
                 Console.ReadKey();
                 sender.Shutdown(SocketShutdown.Both); //chiude le socket come nel server
                 sender.Close();
             }
             catch (Exception ex)
             {

             }        */

            String password = "Dario002";

            Encryption encryption = new Encryption();

            string passwordCriptata = encryption.Encrypt(password);

            Console.WriteLine($"Password:{password}");
            Console.WriteLine($"Password Criptata:{passwordCriptata}");
            Console.WriteLine($"Password Decriptata:{encryption.Decrypt(passwordCriptata)}");
            Console.ReadLine();
        }
    }   

}
