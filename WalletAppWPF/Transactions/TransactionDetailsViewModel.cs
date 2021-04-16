using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Transactions;
using WalletApp.WalletAppWPF.Models.Common;

namespace WalletApp.WalletAppWPF.Transactions
{
    public class TransactionDetailsViewModel : BindableBase
    {
        private Transaction _transaction;

        public decimal Sum
        {
            get
            {
                return _transaction.Sum;
            }
            set
            {
                _transaction.UpdateTransaction(value, Description, DateTime, Files);
                RaisePropertyChanged(nameof(value));
            }
        }

        public string Description
        {
            get
            {
                return _transaction.Description;
            }
        }

        public DateTimeOffset DateTime
        {
            get
            {
                return _transaction.DateTime;
            }
        }
        public List<File> Files
        {
            get
            {
                return _transaction.Files;
            }
        }

        //public string DisplayName
        //{
        //    get
        //    {
        //        return $"{_wallet.Name} (${_wallet.Balance})";
        //    }
        //}

        public TransactionDetailsViewModel(Transaction transaction)
        {
            _transaction = transaction;
        }

    }
}
