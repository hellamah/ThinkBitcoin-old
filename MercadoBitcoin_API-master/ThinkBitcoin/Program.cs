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
        public static MBTAPI mbTapi = new MBTAPI("aad7a2985cab0ae996cf53e27121b447b8abc2788285ef1ef8dca1983d015d69", "b074bdc61a15f3f4fdf8f5d9cbfd1768", "8238");


        static void Main(string[] args)
        {
            while (true)
            {
                double porcent;
                double bTotal;
                double sTotal;
                double total;
                /*int bCount1;
                int sCount1;*/

                int bCount = 0;
                int sCount = 0;
                int compra = 0;
                int venda = 0;

                MBPublic mbPublic = new MBPublic();
                MBAccess mbAccess = new MBAccess();

                DTOMBMyFunds myFounds = mbAccess.getMyInfoAccount();

                porcent = myFounds.balanceBTCAvaliable * 0.05;






                List<DTOMBPublicTrades> Pubtrades = null;

                for (int count = 0; count < 6; count++)
                {
                    Pubtrades = new List<DTOMBPublicTrades>();

                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();

                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);



                    /*bTotal = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Sum(x => x.amount);
                    sTotal = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Sum(x => x.amount);
                    total = Pubtrades.Sum(x => x.amount);*/

                    bCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Count();
                    sCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Count();

                    if (bCount > sCount) compra++;
                    else venda++;


                    //O IF que quebrar o FOR precisa estar fora do FOR pra receber a decisão
                    if (compra >= 3 && venda == 0) break;
                    if (venda >= 3 && compra == 0) break;
                }

                if (compra >= 3 && venda == 0 && porcent > 0.001)
                {
                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();
                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);
                    bCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Count();
                    sCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Count();
                    var mediaOps = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Take(5).Sum(x=>x.price) /5;

                    if (bCount < sCount)
                    {
                        DTOMBOrder testeCompra = mbTapi.setBitCoinTradeSellMarket(porcent, mediaOps);
                    }
                    //TODO: VENDE!
                }

                if (venda >= 3 && compra == 0 && myFounds.balanceBRLAvaliable > 60)
                {
                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();
                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);
                    bCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Count();
                    sCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Count();
                    var mediaOps = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Take(5).Sum(x => x.price) / 5;

                    if (sCount/2 < bCount)
                    {
                        DTOMBOrder testeCompra = mbTapi.setBitCoinTradeBuyMarket(myFounds.balanceBRLAvaliable, mediaOps);

                    }
                    //
                }
            }



            //List<DTOMBOrder> teste = mbTapi.getMyOpenOrders(MBEnumerables.CoinType.Bit);

            DTOMBOrder dddd = mbTapi.getMy20Orders();


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
