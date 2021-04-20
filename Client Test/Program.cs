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
            iPAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];
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
                bytes = Encoding.ASCII.GetBytes($"{username}|{CreateMD5(password)}|check");
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
                    string[] arr = str.Split('|');
                    arr[1] = CreateMD5(arr[1]);
                    str = "";
                    for(int i=0;i<arr.Length;i++)
                    {
                        if (i == arr.Length - 1)
                            str += arr[i];
                        else
                            str += arr[i] + "|";
                    }
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

            string CreateMD5(string input)
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
