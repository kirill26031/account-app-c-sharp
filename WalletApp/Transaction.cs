using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    class Transaction
    {
        private Guid _Id;
        private decimal _Sum;
        private Category _Category;
        Currency.CurrencyType _CurrencyType;
    }
}
