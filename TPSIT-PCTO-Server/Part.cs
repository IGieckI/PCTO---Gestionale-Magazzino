using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSIT_PCTO_Server
{
    class Part
    {
        private int _code;
        public Part(int codice)
        {
            _code = codice;
        }
        public int code
        {
            get
            {
                return _code;
            }
        }
    }
}
