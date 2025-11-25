using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ReturnResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ReturnId { get; set; } = string.Empty;
    }
}
