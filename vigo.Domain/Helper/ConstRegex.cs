using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Helper
{
    public class ConstRegex
    {
        public const string EMAIL_REGEX = "^[A-Za-z0-9](([a-zA-Z0-9,=\\.!\\-#|\\$%\\^&\\*\\+/\\?_`\\{\\}~]+)*)@(?:[0-9a-zA-Z-]+\\.)+[a-zA-Z]{2,9}$";
        public const string PHONE_REGEX = "^\\d{10}$";
    }
}
