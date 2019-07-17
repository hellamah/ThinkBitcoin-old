using Dotend.MBTrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBTrade.DTO.MBOperation
{
    public class MBOperation
    {
        public MBEnumerables.OperationType Type { get; set; }

        public double Price { get; set; }

        public double Volume { get; set; }
    }

    
}
