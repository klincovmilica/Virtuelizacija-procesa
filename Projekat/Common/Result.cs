using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public List<Load> Loads { get; set; } = new List<Load>();

        [DataMember]
        public List<Audit> Audits { get; set; } = new List<Audit>();


    }
}
