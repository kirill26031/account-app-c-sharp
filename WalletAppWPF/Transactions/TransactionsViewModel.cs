using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Models.Transactions;
using WalletApp.WalletAppWPF.Services;
using WalletAppWPF.Transactions;
using System.Collections.ObjectModel;

namespace WalletApp.WalletAppWPF.Transactions
{
    public class TransactionsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private TransactionService _service;
        private TransactionDetailsViewModel _currentTransaction;
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set; }

        public TransactionDetailsViewModel CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }
            set
            {
                _currentTransaction = value;
                RaisePropertyChanged();
            }
        }

        public TransactionsViewModel()
        {
            _service = new TransactionService();
            Transactions = new ObservableCollection<TransactionDetailsViewModel>();
            FillTransactions();
        }

        private async void FillTransactions()
        {

            foreach (var wallet in await _service.GetTransactions())
            {
                Transactions.Add(new TransactionDetailsViewModel(wallet));
            }
        }

        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Transactions;
            }
        }
        public void ClearSensitiveData()
        {

        }
    }
}