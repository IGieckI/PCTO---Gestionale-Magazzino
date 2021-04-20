using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Terminale
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _nomePCDB = "PC1229";
        private List<Prodotto> _prodotti = new List<Prodotto>();
        private List<Utente> _utenti = new List<Utente>();
        private List<string> Users = new List<string>();
        private List<string> Passwords = new List<string>();
        private bool firstTime = true;
        public MainWindow()
        {
            InitializeComponent();

            string str = "\0\u0005check\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
            string safd = (str.Replace("\0", "").Replace("\u0005",""));

            DataGridTextColumn colonnaUsername = new DataGridTextColumn();
            colonnaUsername.Header = "Username";
            colonnaUsername.IsReadOnly = true;
            colonnaUsername.Binding = new Binding("Username");
            dgUtenti.Columns.Add(colonnaUsername);
            DataGridTextColumn colonnaPassword = new DataGridTextColumn();
            colonnaPassword.Header = "Password";
            colonnaPassword.IsReadOnly = true;
            colonnaPassword.Binding = new Binding("Password");
            dgUtenti.Columns.Add(colonnaPassword);

            DataGridTextColumn colonnaNome = new DataGridTextColumn();
            colonnaNome.Header = "Codice";
            colonnaNome.IsReadOnly = true;
            colonnaNome.Binding = new Binding("Codice");
            dgProdotti.Columns.Add(colonnaNome);
            DataGridTextColumn colonnaCodice = new DataGridTextColumn();
            colonnaCodice.Header = "Nome";
            colonnaCodice.IsReadOnly = true;
            colonnaCodice.Binding = new Binding("Nome");
            dgProdotti.Columns.Add(colonnaCodice);
            DataGridTextColumn colonnaQuantità = new DataGridTextColumn();
            colonnaQuantità.Header = "Quantita";
            colonnaQuantità.IsReadOnly = false;
            colonnaQuantità.Binding = new Binding("Quantita");
            dgProdotti.Columns.Add(colonnaQuantità);

            ListUpdate();

            firstTime = false;
        }

        static void ModificaSuPezzo(string comando, out SqlCommand command, SqlConnection cnn, SqlDataAdapter adapter)
        {
            command = new SqlCommand(comando, cnn);
            adapter.InsertCommand = new SqlCommand(comando, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
        }

        private void btnConfermaUtente_Click(object sender, RoutedEventArgs e)
        {
            ListUpdate();

            if (txtUsername.Text == "")
            {
                MessageBox.Show("L'username non può essere vuoto", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (txtPassword.Password == "")
            {
                MessageBox.Show("La password non può essere vuota", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020");
            SqlCommand command;
            String sql;
            SqlDataAdapter adapter = new SqlDataAdapter();
            cnn.Open();
            sql = $"INSERT INTO Utenti VALUES('{txtUsername.Text}','{StringToMD5(txtPassword.Password).ToLower()}')";
            command = new SqlCommand(sql, cnn);
            ModificaSuPezzo(sql, out command, cnn, adapter);
            cnn.Close();
            _utenti.Add(new Utente(txtUsername.Text, StringToMD5(txtPassword.Password).ToLower()));
            txtUsername.Text = "";
            txtPassword.Password = "";
            ListUpdate();
        }

        private void btnConfermaProdotto_Click(object sender, RoutedEventArgs e)
        {
            ListUpdate();

            if (txtCodice.Text == "")
            {
                MessageBox.Show("Il codice non può essere vuoto", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (txtNome.Text == "")
            {
                MessageBox.Show("Il nome del prodotto non può essere vuoto", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                if (int.Parse(txtQuantita.Text) < 0)
                    MessageBox.Show("Non puoi inserire quantità negative", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("La quantità deve essere un numero", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020");
            SqlCommand command;
            String sql;
            SqlDataAdapter adapter = new SqlDataAdapter();
            cnn.Open();
            sql = $"INSERT INTO Prodotti VALUES('{txtCodice.Text}',{txtQuantita.Text},'{txtNome.Text}')";
            command = new SqlCommand(sql, cnn);
            ModificaSuPezzo(sql, out command, cnn, adapter);
            cnn.Close();
            _prodotti.Add(new Prodotto(txtCodice.Text,int.Parse(txtQuantita.Text), txtNome.Text));
            txtCodice.Text = "";
            txtNome.Text = "";
            txtQuantita.Text = "";
            ListUpdate();
        }

        private void btnAggiorna_Click(object sender, RoutedEventArgs e)
        {
            ListUpdate();
        }

        private void ListUpdate()
        {
            try
            {
                _prodotti.Clear();
                _utenti.Clear();
                dgProdotti.Items.Clear();
                dgUtenti.Items.Clear();

                SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020");
                cnn.Open();
                string sql = $"SELECT * FROM Utenti";
                SqlCommand command = new SqlCommand(sql, cnn);
                SqlDataReader OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                {
                    string str = OutPutSelectAll[0].ToString() + "," + OutPutSelectAll[1].ToString();
                    dgUtenti.Items.Add(new Utente(OutPutSelectAll[0].ToString(), OutPutSelectAll[1].ToString()));
                    _utenti.Add(new Utente(OutPutSelectAll[0].ToString(), OutPutSelectAll[1].ToString()));
                }
                cnn.Close();

                cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020");
                cnn.Open();
                sql = $"SELECT * FROM Prodotti";
                command = new SqlCommand(sql, cnn);
                OutPutSelectAll = command.ExecuteReader();
                while (OutPutSelectAll.Read())
                {
                    string str = OutPutSelectAll[0].ToString() + "," + OutPutSelectAll[1].ToString() + "," + OutPutSelectAll[2].ToString();
                    dgProdotti.Items.Add(new Prodotto(OutPutSelectAll[0].ToString(), int.Parse(OutPutSelectAll[1].ToString()), OutPutSelectAll[2].ToString()));
                    _prodotti.Add(new Prodotto(OutPutSelectAll[0].ToString(), int.Parse(OutPutSelectAll[1].ToString()), OutPutSelectAll[2].ToString()));
                }
                cnn.Close();

                dgProdotti.Items.Refresh();
                dgUtenti.Items.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Errore(Terminale)",MessageBoxButton.OK,MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
            
        }

        private void dgUtenti_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void dgProdotti_LayoutUpdated(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCDB};Initial Catalog=Magazzino;User ID=sa;Password=burbero2020");
            cnn.Open();
            string sql = $"SELECT * FROM Prodotti";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader OutPutSelectAll = command.ExecuteReader();
            while (OutPutSelectAll.Read())
            {
                _prodotti.Add(new Prodotto(OutPutSelectAll[0].ToString(), int.Parse(OutPutSelectAll[1].ToString()), OutPutSelectAll[2].ToString()));
            }
            cnn.Close();
        }

        private void dgUtenti_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            int a;

            /*SqlConnection cnn = new SqlConnection($@"Data Source={_nomePCUtenti};Initial Catalog={_nomeDatabaseUtenti};Integrated Security=SSPI;");
            cnn.Open();
            string sql = $"SELECT * FROM {_nomeTabellaUtenti}";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader OutPutSelectAll = command.ExecuteReader();
            while (OutPutSelectAll.Read())
            {
                _utenti.Add(new Utente(OutPutSelectAll[0].ToString(), OutPutSelectAll[1].ToString()));
            }
            cnn.Close();*/
        }

        public static string StringToMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
