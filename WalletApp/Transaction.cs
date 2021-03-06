using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    class Transaction
    {
        Transaction()
        {
            _Id = Guid.NewGuid();
        }

        private Guid _Id;
        private decimal Sum { get; set; }
        private Category _Category;
        private Currency.CurrencyType _CurrencyType;
        private string Description { get; set; }
        private DateTimeOffset DateTime { get; set; }

        public Guid Id
        {
            get { return _Id; }
        }

        public Category Category
        {
            get => _Category; 
            private set => _Category = value;
        }

        public Currency.CurrencyType CurrencyType
        {
            get => _CurrencyType;
            private set => _CurrencyType = value;
        }

    }
}
