using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Helper
{
    public class CustomException : Exception
    {
        public CustomException(string mess) : base(mess){
        }
    }
}
