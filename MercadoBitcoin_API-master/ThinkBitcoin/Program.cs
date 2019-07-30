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
            bool buy = false;
            bool sell = false;
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

                for (int count = 0; count < 50; count++)
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


                    //O IF que quebrar o FOR precisa estar fora do for pra receber a decisão
                    if (compra >= 6 && venda <= compra / 6)
                    { sell = true; buy = false; } //mercado subindo, quase na hora de vender
                    if (venda >= 6 && compra <= venda / 6)
                    { buy = true; sell = false; } //mercado descendo, quase na hora de comprar

                    if (compra >= 6 && sCount >= bCount - 2 && sell)//TODO: esse if defini o ponto quase exato da venda
                    {
                        count = 0;
                        compra = 0;
                        venda = 0;
                        break;
                    }
                    if (venda >= 6 && bCount >= sCount - 2 && buy)
                    {
                        count = 0;
                        compra = 0;
                        venda = 0;
                        break;
                    }

                    System.Threading.Thread.Sleep(6000);
                }

                List<DTOMBOrder> myOrders = mbTapi.getMyOrders(MBEnumerables.CoinType.Bit);

                if (buy && myFounds.balanceBRLAvaliable >= 70)
                {
                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-300), TimeSpan.Zero).ToUnixTimeSeconds();
                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS).ToList();

                    var listaSell = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).ToList();//para definir o valor da compra, é necessario observar o por quanto esta sendo vendido
                    var primeiros = listaSell.Take(Convert.ToInt32(Math.Round((Pubtrades.Count() / 10f)))).ToList();
                    var ultimosValores = listaSell.Except(primeiros).ToList();

                    var menorValor = (primeiros.Sum(c => c.price) / primeiros.Count()) <= (ultimosValores.Sum(c => c.price) / ultimosValores.Count()) ? (primeiros.Sum(c => c.price) / primeiros.Count()) : (ultimosValores.Sum(c => c.price) / ultimosValores.Count());

                    var preco = myOrders.Where(p => p.type == MBEnumerables.OperationType.Sell).First().price;

                    if ((menorValor <= (preco + (preco * 0.02)))) //É necessário usar o valor da ultima venda para realizar COMPRA
                    {
                        DTOMBOrder ordemCompra = mbTapi.setBitCoinTradeBuyMarket(myFounds.balanceBRLAvaliable, menorValor); //TODO: o valor da quantidade de btc que será comprado deve avaliar a diferença entre valores de antigas compras realizadas com as atuais 
                    }

                    //{//TODO: VENDE!
                    //DTOMBOrder testeCompra = mbTapi.setBitCoinTradeSellMarket(porcent, mediaOps);
                    //}

                }

                if (sell && myFounds.balanceBTCAvaliable >= 0.001)
                {
                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();
                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);
                    //bCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).Count();
                    //sCount = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Count();
                    //var mediaOps = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).Take(5).Sum(x => x.price) / 5;


                    var listaBuy = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Buy).ToList();
                    var primeiros = listaBuy.Take(Convert.ToInt32(Math.Round((Pubtrades.Count() / 10f)))).ToList();
                    var ultimosValores = listaBuy.Except(primeiros).ToList();

                    var menorValor = (primeiros.Sum(c => c.price) / primeiros.Count()) >= (ultimosValores.Sum(c => c.price) / ultimosValores.Count()) ? (primeiros.Sum(c => c.price) / primeiros.Count()) : (ultimosValores.Sum(c => c.price) / ultimosValores.Count());

                    var preco = myOrders.Where(p => p.type == MBEnumerables.OperationType.Buy).First().price;

                    if (menorValor >= (preco + (preco * 0.02))) //É necessário usar o valor da ultima compra para realizar venda
                    {
                        DTOMBOrder ordemVenda = mbTapi.setBitCoinTradeSellMarket(myFounds.balanceBTCAvaliable, menorValor);
                    }
                    //TODO: COMPRA!
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
