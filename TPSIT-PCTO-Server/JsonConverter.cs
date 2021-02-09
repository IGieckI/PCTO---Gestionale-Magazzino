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
        public string  Serialize(Part part)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(part);
        }
        
        public Part Deserialize(string jsonFile)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Part>(jsonFile);
        }
    }
}
