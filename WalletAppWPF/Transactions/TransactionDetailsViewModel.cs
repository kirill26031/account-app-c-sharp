using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Transactions;
using WalletApp.WalletAppWPF.Models.Common;
using Prism.Commands;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Services;
using WalletApp.WalletAppWPF.Models.Users;

namespace WalletApp.WalletAppWPF.Transactions
{
    public class TransactionDetailsViewModel : BindableBase
    {
        private Transaction _transaction;
        private readonly Wallet _wallet;
        private User _user;
        private readonly Action<Wallet> _update;
        private List<Category> _categories;
        private List<Category> _allCategories;
        private Currency.currencyType _currency;
        private TransactionService _transactionService;
        private AuthenticationService _userService;
        private DateTimeOffset _dateTimeOffset;

        private string _description;
        private decimal _sum;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }
        public decimal Sum 
        { get => _sum;
            set
            {
                _sum = value;
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }


        public List<Category> Categories
        {
            get => _allCategories;
            set
            {
                _categories = value;
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

      
        public List<File> Files
        {
            get
            {
                return _transaction.Files;
            }
        }

        public bool IsUAHChecked
        {
            get => _currency == Models.Common.Currency.currencyType.UAH;
            set 
            {
                _currency = value ? Models.Common.Currency.currencyType.UAH : Models.Common.Currency.currencyType.USD;
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsUSDChecked
        {
            get => _currency == Models.Common.Currency.currencyType.USD;
            set
            {
                _currency = value ? Models.Common.Currency.currencyType.USD : Models.Common.Currency.currencyType.UAH;
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }


        public DateTime DateTime
        {
            get => _dateTimeOffset.DateTime;
            set
            {
                _dateTimeOffset = new DateTimeOffset(value);
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public Transaction Transaction => _transaction;

        private bool AreChangesExist() => 
            ((_categories == null || _categories.Count == 0) ? false : _categories.First().Name != _transaction.Category.Name) ||
                    _currency != _transaction.CurrencyType || Sum != _transaction.Sum || Description != _transaction.Description ||
                    _dateTimeOffset != _transaction.DateTime;

        public TransactionDetailsViewModel(Transaction transaction, Wallet wallet, User user, Action goToAddingTransaction, Action goToWallets, Action<Wallet> update)
        {
            _transaction = transaction;
            _user = user;
            _wallet = wallet;
            _update = update;
            _allCategories = _wallet.Categories;
            _currency = transaction.CurrencyType;
            SaveEditCommand = new DelegateCommand(SaveEdit, CanSaveEdit);
            _transactionService = new TransactionService(_wallet);
            _userService = new AuthenticationService();
            _dateTimeOffset = _transaction.DateTime;
            Sum = transaction.Sum;
            Description = transaction.Description;
        }


        public DelegateCommand SaveEditCommand { get; }

        public DelegateCommand DeleteTransactionCommand { get => new DelegateCommand(DeleteTransaction); }

        private bool CanSaveEdit()
        {
            return Sum > 0 && !String.IsNullOrEmpty(Description) && AreChangesExist();
        }

        private async void SaveEdit()
        {
            if (_categories != null) _transaction.Category = _categories.First(); 
            _transaction.Description = Description;
            _transaction.Sum = Sum;
            _transaction.CurrencyType = _currency;
            _transaction.DateTime = _dateTimeOffset;
            SaveEditCommand.RaiseCanExecuteChanged();
            _update.Invoke(await _transactionService.Update(_transaction));
        }


        private async void DeleteTransaction()
        {
            Wallet wallet = await _transactionService.Delete(_transaction);
            _update.Invoke(wallet);

            //RaisePropertyChanged("Wallets");
            //RaisePropertyChanged("CurrentWallet");
        }
    }
}
