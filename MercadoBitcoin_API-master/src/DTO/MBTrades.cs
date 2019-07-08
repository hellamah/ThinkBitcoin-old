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

namespace Dotend.MBTrade.DTO.MBTrades
{
    [DataContract]
    public class MBTrades
    {
        public MBEnumerables.OperationType _type { get; set; }
        public DateTime _data { get; set; }

        private double Ddate { get; set; }

        [DataMember]
        public long tid { get; set; } //identificador
        [DataMember]
        public double date
        {
            get { return Ddate; }
            set
            {
                _data = new DateTime(1970, 01, 01).AddSeconds(value);
                Ddate = value;
            }
        }//data e hora da operação

        public string Type//tipo da operação
        {
            set
            {
                if (value == MBEnumerables.OperationType.Buy.ToString()) _type = MBEnumerables.OperationType.Buy;
                else _type = MBEnumerables.OperationType.Sell;

            }
        }
        [DataMember]
        public double price { get; set; } //preço do moeda
        [DataMember]
        public double amount { get; set; } //quantidade da moeda ofertada

    }
}
