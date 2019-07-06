using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Dotend.MBTrade;
using Dotend.MBTrade.DTO;

namespace ThinkBitcoin
{
    class Program
    {
        public MBEnumerables.OperationType operationType;
        void Main(string[] args)
        {

            MBTAPI mbTapi = new MBTAPI("", "", "");
            MBAccess mbAccess = new MBAccess();

            
            List<MBTrades> trades = mbTapi.getLastBtcTrades().GetRange(0,20); //Lista das ultimas 20 ordens de execução

            double bTotal = 0;
            double sTotal = 0;
            double total = 0;
            int bCount = 0;
            int sCount = 0;

            foreach(var trade in trades)
            {
                #region COMPRA
                if (trade.Type == MBEnumerables.OperationType.Buy)
                {
                    bTotal += trade.amount;
                    bCount++;

                    if((bCount > 5 && sCount < 2) || (bCount > 7 && sCount)// TODAS AS REGRAS DE COMPRA
                    {
                        operationType = MBEnumerables.OperationType.Buy;
                        break;
                    }
                }
                #endregion

                #region VENDA
                else if (trade.Type == MBEnumerables.OperationType.Sell)
                {
                    sTotal += trade.amount;
                    sCount++;

                    if ((sCount > 5 && bCount < 2))// TODAS AS REGRAS DE VENDA
                    {
                        operationType = MBEnumerables.OperationType.Sell;
                    }
                }
                #endregion

            }


            DTOMBMyFunds myFounds = mbAccess.getMyInfoAccount(); //Saldo R$, BTC e LTC


            if (myFounds.balanceBTCAvaliable > 0.001)
            {
                if(operationType == MBEnumerables.OperationType.Sell)
                {

                }
            }

        }
    }
}
