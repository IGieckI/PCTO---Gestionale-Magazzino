using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPSIT_PCTO_Server
{
    class JsonConverter
    {
        public void Converte(Part part)
        {
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(part);
            using (StreamWriter sw = new StreamWriter("output.json"))
            {
                sw.WriteLine(str);
            }
        }
    }
}
