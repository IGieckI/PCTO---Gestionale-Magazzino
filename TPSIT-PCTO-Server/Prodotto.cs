using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSIT_PCTO_Server
{
    class Prodotto
    {
        public int CodiceProdotto { get; set; }
        public string NomeProdotto { get; set; }
        public string Descrizione { get; set; }
        public Prodotto(string nomeProdotto, int codice)
        {
            CodiceProdotto = codice;
            NomeProdotto = nomeProdotto;
        }
        public Prodotto(int codice)
        {
            CodiceProdotto = codice;
        }
    }
}
