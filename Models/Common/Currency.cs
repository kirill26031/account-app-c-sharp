using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.WalletAppWPF.Models.Common
{
    public static class Currency
    {
        private static List<decimal> exchangeRate = new List<decimal>() 
        { 
            1,
            System.Convert.ToDecimal(27.64)
        };
        public enum currencyType
        {
            USD,
            UAH
        }

        public static decimal Convert(currencyType fromC, currencyType toC, decimal from)
        {
            return Decimal.Divide(Decimal.Multiply(from, exchangeRate[(int)toC]), exchangeRate[(int)fromC]);
        }
    }
}
