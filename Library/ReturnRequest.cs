using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ReturnRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
        public List<int> ProductIds { get; set; } = new List<int>();
        public string Reason { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
