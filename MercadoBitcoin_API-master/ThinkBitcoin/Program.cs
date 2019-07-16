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
            MBTAPI mbTapi = new MBTAPI("aad7a2985cab0ae996cf53e27121b447b8abc2788285ef1ef8dca1983d015d69", "b074bdc61a15f3f4fdf8f5d9cbfd1768", "8238");
            MBPublic mbPublic = new MBPublic();

            decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
            decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();

            List<DTOMBPublicTrades> Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);
            List<DTOMBOrder> teste = mbTapi.getMyOpenOrders(MBEnumerables.CoinType.Bit);

           // List<MBTrades> trades = mbTapi.getLastBtcTrades();//.GetRange(980, 20).OrderByDescending(x => x.date).ToList();

            double bTotal = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Sum(x => x.amount);
            double sTotal = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Sum(x => x.amount);
            double total = Pubtrades.Sum(x => x.amount);

            int bCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Count();
            int sCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Count();


            if (bCount > sCount)
            {

            }










            MBAccess mbAccess = new MBAccess();

            DTOMBOrder dddd = mbTapi.getMy20Orders();
            DTOMBMyFunds myFounds = mbAccess.getMyInfoAccount();

            //DateTime dt = DateTime.Now;

             //Lista das ultimas 20 ordens de execução

            //double bTotal = tr
                
                
                
                
                
                
                
                
                
                
                
            //    ades.Where(x => x._type == MBEnumerables.OperationType.Buy).Sum(x => x.amount);
            

            

            /*foreach (var trade in trades)
            {
                if (trade._type == MBEnumerables.OperationType.Buy)
                    bCount++;
                else 
                    sCount++;*/

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

           /*     if (myFounds.balanceBTCAvaliable > 0.001)
                {
                    //mbOp.Price = 1;//TODO: parte da estatistica vem aqui!
                }*
            }*/




            

        }
    }
}
