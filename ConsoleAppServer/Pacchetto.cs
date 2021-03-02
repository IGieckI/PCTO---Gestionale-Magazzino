using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSIT_PCTO_Server
{
    class Pacchetto
    {
        public string Username;
        public string Password;
        public string CodiceAzione;
        public string ProductCode;
        public Pacchetto()
        {

        }
        public Pacchetto(string username, string password, string codiceAzione, string productCode)
        {
            Username = username;
            Password = password;
            CodiceAzione = codiceAzione;
            ProductCode = productCode;
        }
        public override string ToString()
        {
            return $"{Username}|{Password}|{CodiceAzione}|{ProductCode}";
        }
    }
}
