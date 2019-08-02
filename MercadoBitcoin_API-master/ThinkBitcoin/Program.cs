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
            //theFirst();
            while (true)
            {
                List<DTOMBPublicTrades> Pubtrades = null;
                MBPublic mbPublic = new MBPublic();
                Pubtrades = new List<DTOMBPublicTrades>();

                decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-200), TimeSpan.Zero).ToUnixTimeSeconds();

                Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS);

                var myLastOrder = mbTapi.getMyOrders(MBEnumerables.CoinType.Bit).Where(p => p.status == 4).First();

                if (myLastOrder.type == MBEnumerables.OperationType.Buy)
                {//TODO:VENDER!
                    if (Pubtrades.Take(4).Where(x=>x.type == MBEnumerables.OperationType.Sell).Count() >= 3)
                    {//.Sum() / 4 > (myLastOrder.price + (myLastOrder.price * 0.006))) {
                        var valVenda = (Pubtrades.Take(4).Where(x => x.type == MBEnumerables.OperationType.Sell).Select(p=> p.price).Sum() / Pubtrades.Take(4).Where(x => x.type == MBEnumerables.OperationType.Buy).Count());
                    }
                }
                else
                {//TODO:COMPRAR!(preco + (preco * 0.006))
                    if (Pubtrades.Take(4).Select(c =>c.price).Sum() / 4 > (myLastOrder.price + (myLastOrder.price * 0.006))) { }

                }
            }
        }

        public async Task theFirst()
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

                for (int count = 0; count < 15; count++)
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

                    System.Threading.Thread.Sleep(600);
                }

                if (myFounds == null) myFounds = mbAccess.getMyInfoAccount();
                List<DTOMBOrder> myOrders = mbTapi.getMyOrders(MBEnumerables.CoinType.Bit);

                if (buy && myFounds.balanceBRLAvaliable >= 70)
                {
                    decimal unixS = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeSeconds();
                    decimal unixSMenor = new DateTimeOffset(DateTime.UtcNow.AddSeconds(-300), TimeSpan.Zero).ToUnixTimeSeconds();
                    Pubtrades = mbPublic.getPublicTrades30s(MBEnumerables.CoinType.Bit, unixSMenor, unixS).ToList();

                    var listaSell = Pubtrades.Where(x => x.type == MBEnumerables.OperationType.Sell).ToList();//para definir o valor da compra, é necessario observar o por quanto esta sendo vendido
                    var primeiros = listaSell.Take(Convert.ToInt32(Math.Round((Pubtrades.Count() / 10f)))).ToList();
                    var ultimosValores = listaSell.Except(primeiros).ToList();

                    var menorValor = Math.Round(((primeiros.Sum(c => c.price) / primeiros.Count()) <= (ultimosValores.Sum(c => c.price) / ultimosValores.Count()) ? (primeiros.Sum(c => c.price) / primeiros.Count()) : (ultimosValores.Sum(c => c.price) / ultimosValores.Count())), 2);

                    var preco = myOrders.Where(p => p.type == MBEnumerables.OperationType.Sell && p.status == 4).First().price;

                    if (preco > 0 && (menorValor <= (preco + (preco * 0.006)))) //É necessário usar o valor da ultima venda para realizar COMPRA
                    {
                        DTOMBOrder ordemCompra = mbTapi.setBitCoinTradeBuy(myOrders.Where(p => p.type == MBEnumerables.OperationType.Sell && p.status == 4).First().quantity, menorValor); //TODO: o valor da quantidade de btc que será comprado deve avaliar a diferença entre valores de antigas compras realizadas com as atuais 
                    }

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

                    var menorValor = Math.Round(((primeiros.Sum(c => c.price) / primeiros.Count()) >= (ultimosValores.Sum(c => c.price) / ultimosValores.Count()) ? (primeiros.Sum(c => c.price) / primeiros.Count()) : (ultimosValores.Sum(c => c.price) / ultimosValores.Count())), 2);

                    var preco = myOrders.Where(p => p.type == MBEnumerables.OperationType.Buy && p.status == 4).First().price;

                    if (preco > 0 && menorValor >= (preco + (preco * 0.006))) //É necessário usar o valor da ultima compra para realizar venda
                    {
                        DTOMBOrder ordemVenda = mbTapi.setBitCoinTradeSell(myFounds.balanceBTCAvaliable, menorValor);
                    }
                }
            }
        }
    }
}
