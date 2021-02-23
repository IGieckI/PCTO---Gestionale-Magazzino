using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TPSIT_PCTO_Server
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Prodotto> parts;
        List<Thread> accept;
        Thread connectionRequest;
        private readonly object _lockOperazioni = new object();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                parts = new List<Prodotto>();
                accept = new List<Thread>();
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddress = ipHostInfo.AddressList[1];
                //IPAddress iPAddress = IPAddress.Parse("10.12.0.28");
                IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000); //creo un endpoint con il mio ip e la porta di comunicazione
                Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //crea la socket
                connectionRequest = new Thread(() => Connect(listener));
                connectionRequest.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Connect(Socket listener)
        {
            while (true)
            {
                Socket handler = listener.Accept();
                accept.Add(new Thread(() => AcceptClient(handler)));
                accept[accept.Count - 1].Start();
            }
        }
        private void AcceptClient(Socket handler)
        {
            this.Dispatcher.Invoke(() =>
            {
                lblConnection.Content = "Connected";
            });
            byte[] bytes = new byte[1024]; //buffer dei dati
            string data = null; //messaggio
            bool ok = false; //bool per uscire
            while (!ok)
            {
                int bytesRec = handler.Receive(bytes); //riceve i bytes
                data = "";
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec); //trasforma in stringa
                string[] mex = data.Split('|');
                lock (_lockOperazioni)
                {
                    if (mex[0] == "ADD")
                    {
                        int codice;
                        string codiceS = "";
                        for (int i = 0; i < 8; i++)
                        {
                            codiceS += mex[1][i].ToString();
                        }
                        codice = int.Parse(codiceS);

                        foreach (Prodotto x in parts)
                        {
                            if (x.CodiceProdotto == codice)
                                throw new Exception("tutto ciò non dovrebbe succedere, codici uguali");
                        }
                        parts.Add(new Prodotto(codice));
                    }
                    if (mex[0] == "REMOVE")//nome|password|codiceOperazione|codiceProdotto
                    {
                        int codice;
                        string codiceS = "";
                        for (int i = 0; i < 8; i++)
                        {
                            codiceS += mex[1][i].ToString();
                        }
                        codice = int.Parse(codiceS);

                        for (int i = 0; i < parts.Count; i++)
                        {
                            if (parts[i].CodiceProdotto == codice)
                                parts.RemoveAt(i);
                        }
                    }
                    if (mex[0] == "DISCONNECT")
                    {
                        ok = true;
                    }
                }
            }
        }
    }
}
