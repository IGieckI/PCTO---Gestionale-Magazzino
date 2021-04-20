using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminale
{
    class Prodotto
    {
        public string Codice { get; set; }
        public string Nome { get; set; }
        public int Quantita { get; set; }
        public Prodotto(string codice, int quantita, string nome)
        {
            Codice = codice;
            Nome = nome;
            Quantita = quantita;
        }
    }
}
