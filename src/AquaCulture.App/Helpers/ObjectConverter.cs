using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace AquaCulture.App.Helpers
{
    public class ObjectConverter
    {
       

        public MemoryStream Base64ToStream(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
