using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSIT_PCTO_Server
{
    class Part
    {
        private ulong _code;
        public Part(ulong codice)
        {
            _code = codice;
        }
        public ulong code
        {
            get
            {
                return _code;
            }
        }
    }
}
