using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Dotend.MBTrade.DTO
{
    public class DTOMBPublicTrades : DTOMB
    {
        public DateTime date { get; set; }
        public double price { get; set; }
        public double amount { get; set; }
        public long tid { get; set; }
        public MBEnumerables.OperationType type { get; set; }

        public DTOMBPublicTrades() : base("") { }
        public DTOMBPublicTrades(string pJsonData) : base(pJsonData)
        {
            foreach (DTOMBPublicTrades _item in this.convertJsonToObjectL(pJsonData))
            {
                foreach (PropertyInfo _prop in typeof(DTOMBPublicTrades).GetProperties())
                {
                    _prop.SetValue(this, _prop.GetValue(_item));
                }
                return;
            }
        }

        public List<DTOMBPublicTrades> convertJsonToObjectL(string pJsonData)
        {
            dynamic _data = new JavaScriptSerializer().DeserializeObject(pJsonData);
            
            List<DTOMBPublicTrades> lPT = new List<DTOMBPublicTrades>();

            NumberFormatInfo _provider = new NumberFormatInfo();
            DateTime _dataBase = new DateTime(1970, 1, 1);
            _provider.NumberDecimalSeparator = ".";
            _provider.NumberGroupSeparator = ",";

            foreach (var dado in _data)
            {
                try
                {
                    DTOMBPublicTrades PT = new DTOMBPublicTrades();
                    if (_data != null)
                    {
                        var teste = (((Dictionary<string, object>)dado)["type"]).ToString();
                        PT.date = _dataBase.AddSeconds(Convert.ToInt64((((Dictionary<string, object>)dado)["date"])));
                        PT.price = Convert.ToDouble((((Dictionary<string, object>)dado)["price"]), _provider);
                        PT.amount = Convert.ToDouble((((Dictionary<string, object>)dado)["amount"]), _provider);
                        PT.tid = Convert.ToInt64((((Dictionary<string, object>)dado)["tid"]));
                        PT.type = (((Dictionary<string, object>)dado)["type"]).ToString() == "buy" ? MBEnumerables.OperationType.Buy : MBEnumerables.OperationType.Sell;
                    }
                    lPT.Add(PT);
                }
                catch { lPT = null; }
            }

            return lPT;
        }


    }
}
