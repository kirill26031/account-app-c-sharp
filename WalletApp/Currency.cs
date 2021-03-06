using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public static class Currency
    {
        private static List<decimal> ExchangeRate = new List<decimal>() 
        { 
            1, 
            Convert.ToDecimal(27.64)
        };
        public enum CurrencyType
        {
            USD,
            UAH
        }

        public static decimal convert(CurrencyType fromC, CurrencyType toC, decimal from)
        {
            return from * ExchangeRate[(int)toC] / ExchangeRate[(int)fromC];
        }
    }
}
