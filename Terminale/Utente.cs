using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminale
{
    class Utente
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Utente(string nome, string password)
        {
            Username = nome;
            Password = password;
        }
    }
}
