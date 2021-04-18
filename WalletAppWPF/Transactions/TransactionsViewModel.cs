using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Models.Transactions;
using WalletApp.WalletAppWPF.Services;
using WalletApp.WalletAppWPF.Transactions;
using System.Collections.ObjectModel;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Models.Users;
using Prism.Commands;

namespace WalletApp.WalletAppWPF.Transactions
{
    public class TransactionsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private TransactionService _service;
        private TransactionDetailsViewModel _currentTransaction;
        Wallet _wallet;
        private User _user;
        private Action _goToWallets;
        private Action _goToAddingTransaction;


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

        public DelegateCommand GoToWallets => new DelegateCommand(_goToWallets);

        public DelegateCommand AddTransaction => new DelegateCommand(_goToAddingTransaction);

        public TransactionsViewModel(User user, Wallet wallet, Action goToWallets, Action goToAddingTransaction)
        {
            _user = user;
            _goToAddingTransaction = goToAddingTransaction;
            _goToWallets = goToWallets;
            _wallet = wallet;
            _service = new TransactionService(wallet);
            Transactions = FillTransactions();
            if (Transactions.Count > 0) CurrentTransaction = Transactions.First();
        }

        private ObservableCollection<TransactionDetailsViewModel> FillTransactions()
        {
            var transactions = new ObservableCollection<TransactionDetailsViewModel>();
            foreach (var transaction in _wallet.Transactions)
            {
                transactions.Add(new TransactionDetailsViewModel(transaction, _wallet, _user, _goToAddingTransaction, _goToWallets, new Action<Wallet>(UpdateWallet)));
            }
            return transactions;
        }

        WalletNavigatableTypes INavigatable<WalletNavigatableTypes>.Type => WalletNavigatableTypes.Transactions;

        public void ClearSensitiveData()
        {

        }

        public void UpdateWallet(Wallet wallet)
        {
            _wallet = wallet;
            Guid currentGuid = CurrentTransaction.Transaction.Guid;
            Transactions = FillTransactions();
            foreach(TransactionDetailsViewModel tr in Transactions)
            {
                if(tr.Transaction.Guid == currentGuid)
                {
                    CurrentTransaction = tr;
                    break;
                }
            }
            if (CurrentTransaction == null && Transactions.Count > 0) CurrentTransaction = Transactions.First();
            RaisePropertyChanged(nameof(Transactions));
        }
    }
}