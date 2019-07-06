using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace Dotend.MBTrade.DTO
{
    [DataContract]
    public class MBTrades
    {
        private MBEnumerables.OperationType _type;

        [DataMember]
        public long tid { get; set; }
        [DataMember]
        public decimal date { get; set; }

        public MBEnumerables.OperationType Type { get => _type; set => _type = value; }
        [DataMember]
        public double price { get; set; }
        [DataMember]
        public double amount { get; set; }

    }
}
