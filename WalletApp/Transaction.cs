using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public class Transaction
    {
        Transaction()
        {
            _Id = Guid.NewGuid();
        }

        private Guid _Id;
        public decimal Sum { get; set; }
        private Category _Category;
        private Currency.CurrencyType _CurrencyType;
        public string Description { get; set; }
        public DateTimeOffset DateTime { get; set; }
        private List<File> _Files = new List<File>();

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

        public List<File> Files
        {
            get
            {
                List<File> copy = new List<File>();
                foreach (File File in _Files) {
                    copy.Add(new File(File));
                }
                return copy;
            }
            set => _Files = value;
        }

        public override string ToString()
        {
            return $"Transaction ${Id.ToString()} used ${Sum.ToString()} of ${_CurrencyType} at ${DateTime.ToString()}. Description: ${Description}";
        }
    }
}
