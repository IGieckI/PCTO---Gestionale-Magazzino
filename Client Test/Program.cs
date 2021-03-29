using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

namespace Client_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = new byte[1024]; //buffer dei dati
            byte[] bytes2 = new byte[1024]; //buffer dei dati

            WriteLine("waiting for a response from the server...");

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            //IPAddress ipAddress = IPAddress.Parse("10.12.0.28");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            Socket sender;
            bool login = false;

            Clear();

            do
            {
                Write("Username: ");
                string username = ReadLine();
                Write("Password: ");
                string password = ReadLine();

                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
                sender.Connect(remoteEP); //connette all'endpoint
                bytes = Encoding.ASCII.GetBytes($"{username}|{password}|check");
                int bytesRec = sender.Send(bytes);
                int bytesRec2 = sender.Receive(bytes2); //riceve i bytes
                string data = Encoding.ASCII.GetString(bytes2, 0, bytesRec2); //trasforma in stringa

                sender.Shutdown(SocketShutdown.Both); //chiude le socket come nel server
                sender.Close();

                if (data.Split('|')[0] == "004")
                    login = true;
                Clear();
                WriteLine("Credenziali errate!");
            } while (!login);

            Clear();

            try
            {
                while(true)
                {
                    Clear();
                    WriteLine("Digita 'exit' per uscire");
                    Write("Digita un comando: ");
                    string str = ReadLine();
                    if (str == "exit")
                        break;
                    bytes = Encoding.ASCII.GetBytes(str);

                    sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
                    sender.Connect(remoteEP); //connette all'endpoint

                    int bytesRec = sender.Send(bytes);
                    int bytesRec2 = sender.Receive(bytes2); //riceve i bytes

                    string data = Encoding.ASCII.GetString(bytes2, 0, bytesRec2); //trasforma in stringa
                    Console.WriteLine(data);
                    Console.ReadKey();

                    sender.Shutdown(SocketShutdown.Both); //chiude le socket come nel server
                    sender.Close();
                }
                
            }
            catch (Exception ex)
            {

            }

            /*String password = "Dario002";

            Encryption encryption = new Encryption();

            string passwordCriptata = encryption.Encrypt(password);

            Console.WriteLine($"Password:{password}");
            Console.WriteLine($"Password Criptata:{passwordCriptata}");
            Console.WriteLine($"Password Decriptata:{encryption.Decrypt(passwordCriptata)}");
            Console.ReadLine();*/
        }
    }

}
