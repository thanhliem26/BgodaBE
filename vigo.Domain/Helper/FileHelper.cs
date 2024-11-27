using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Helper
{
    public class FileHelper
    {
        public static string BaseUri = "http://localhost:2002/";
        public static string FileUri(string fileName)
        {
            return BaseUri + "resource/" + fileName;
        }
    }
}
