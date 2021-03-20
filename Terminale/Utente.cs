using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminale
{
    class Utente
    {
        public string Nome { get; set; }
        public string Password { get; set; }
        public Utente(string nome, string password)
        {
            Nome = nome;
            Password = password;
        }
    }
}
