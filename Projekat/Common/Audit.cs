using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public class Audit
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set;}
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

    }
}
