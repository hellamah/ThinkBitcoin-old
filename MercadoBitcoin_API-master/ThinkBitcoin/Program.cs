using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Dotend.MBTrade;
using MBTrade.DTO.MBOperation;
using Dotend.MBTrade.DTO.MBTrades;
using Dotend.MBTrade.DTO;

namespace ThinkBitcoin
{
    class Program
    {
        static void Main(string[] args)
        {
            MBTAPI mbTapi = new MBTAPI("", "", "");
            MBAccess mbAccess = new MBAccess();
            MBOperation mbOp = new MBOperation();

            DTOMBMyFunds myFounds = mbAccess.getMyInfoAccount();

            List<MBTrades> trades = mbTapi.getLastBtcTrades().GetRange(0, 20); //Lista das ultimas 20 ordens de execução

            //double bTotal = tr
                
                
                
                
                
                
                
                
                
                
                
                ades.Where(x => x._type == MBEnumerables.OperationType.Buy).Sum(x => x.amount);
            double sTotal = trades.Where(x => x._type == MBEnumerables.OperationType.Sell).Sum(x => x.amount);
            double total = trades.Sum(x => x.amount);
            int bCount = 0;
            int sCount = 0;

            

            foreach (var trade in trades)
            {
                if (trade._type == MBEnumerables.OperationType.Buy)
                    bCount++;
                else 
                    sCount++;

                /*if ((bCount > 5 && sCount < 2) || (bCount > 7 && sCount)// TODAS AS REGRAS DE COMPRA
                    {
                    mbOp.Type = MBEnumerables.OperationType.Buy;
                    break;
                }
                else if ((sCount > 5 && bCount < 2))// TODAS AS REGRAS DE VENDA
                {
                    mbOp.Type = MBEnumerables.OperationType.Buy;
                    break;
                }*/

                //Saldo R$, BTC e LTC

                if (myFounds.balanceBTCAvaliable > 0.001)
                {
                    mbOp.Price = 1;//TODO: parte da estatistica vem aqui!
                }
            }




            

        }
    }
}
