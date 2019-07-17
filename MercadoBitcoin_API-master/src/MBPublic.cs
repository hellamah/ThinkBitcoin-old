using Dotend.MBTrade;
using Dotend.MBTrade.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Dotend.MBTrade
{
    /// <summary>
    /// Class de acesso do usuário, contendo métodos simples para integrar com o a API do Mercado Bitcoin.
    /// 
    /// </summary>
    public class MBPublic : MBAccess
    {
        private string _error;
        private int _codeError;

        /// <summary>
        /// Função que retorna todas as suas ordens em aberto, fazendo filtro pelos parâmentros
        /// passados pela função
        /// </summary>
        /// <param name="pCoinType"></param>
        /// <param name="pNumberDays"></param>
        /// <returns></returns>
        public List<DTOMBPublicTrades> getPublicTrades30s(MBEnumerables.CoinType pCoinType, decimal pDe, decimal pPara)
        {
            List<DTOMBPublicTrades> _listOrders = new List<DTOMBPublicTrades>();
            string _json;
            string pParameters = string.Empty;

            if (pDe > 0 && pPara == 0)
                pParameters = "/" + pDe.ToString();
            else if (pDe > 0 && pPara > 0)
                pParameters = "/" + pDe.ToString() + "/" + pPara.ToString();



            _json = getPublicDataMBbyMethod(MBEnumerables.SearchType.Trades, MBEnumerables.CoinType.Bit, pParameters);


            if (validateJsonReturn(_json))
            {
                /*_return = new DTOMBPublicTrades(_json);
                return _return;*/


                DTOMBPublicTrades _orderBase = new DTOMBPublicTrades();

                foreach (DTOMBPublicTrades _order in _orderBase.convertJsonToObject(_json))
                {
                    _listOrders.Add(_order);
                }
            }
            else
            {
                _listOrders = null;
            }

            return _listOrders;
        }



        #region "Helper functions"

        private bool validateJsonReturn(string pJson)
        {
            bool _valid = false;

            if (pJson != string.Empty)
            {
                var _data = new JavaScriptSerializer().DeserializeObject(pJson);

                if (_data != null)
                {
                    //if (((Dictionary<string, object>)_data).ContainsKey("status_code"))
                    //{
                    //    if (Convert.ToString(((Dictionary<string, object>)_data)["status_code"]) == "100")
                            _valid = true;
                    //    else
                    //    {
                    //        if (((Dictionary<string, object>)_data).ContainsKey("error_message"))
                    //            _error = Convert.ToString(((Dictionary<string, object>)_data)["error_message"]);
                    //        else
                    //        {
                    //            _codeError = 3;
                    //            _error = "Json em formato incorreto do pardão do MercadoBitcoin.";
                    //        }
                    //    }
                    //    int.TryParse(Convert.ToString(((Dictionary<string, object>)_data)["status_code"]), out _codeError);
                    //}
                    //else
                    //{
                    //    _codeError = 3;
                    //    _error = "Json em formato incorreto do pardão do MercadoBitcoin.";
                    //}

                }
                else
                {
                    _codeError = 2;
                    _error = "Json em formato incorreto, não possivel converter.";
                }

            }
            else
            {
                _codeError = 1;
                _error = "Dados não retornados corretamente.";
            }

            return _valid;
        }

        #endregion
    }
}