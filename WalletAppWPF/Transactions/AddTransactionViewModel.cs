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
    public class AddTransactionViewModel : BindableBase
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

        public string Description { get; set; }
        public decimal Sum { get; set; }


        public List<Category> Categories
        {
            get => _allCategories;
            set
            {
                _categories = value;
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
            set => _currency = value ? Models.Common.Currency.currencyType.UAH : Models.Common.Currency.currencyType.USD;
        }

        public bool IsUSDChecked
        {
            get => _currency == Models.Common.Currency.currencyType.USD;
            set => _currency = value ? Models.Common.Currency.currencyType.USD : Models.Common.Currency.currencyType.UAH;
        }

        public string Date
        {
            get => _dateTimeOffset.Date.ToString();
        }

        public string Time
        {
            get => _dateTimeOffset.TimeOfDay.ToString();
        }

        public DateTimeOffset DateTimeOffset
        {
            set
            {
                _dateTimeOffset = value;
                RaisePropertyChanged(nameof(Date));
                RaisePropertyChanged(nameof(Time));
            }
        }

        public Transaction Transaction => _transaction;

        public AddTransactionViewModel(Transaction transaction, Wallet wallet, User user, Action goToAddingTransaction, Action goToWallets, Action<Wallet> update)
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
            return Sum > 0 && !String.IsNullOrEmpty(Description);
        }

        private async void SaveEdit()
        {
            if (_categories != null) _transaction.Category = _categories.First();
            _transaction.Description = Description;
            _transaction.Sum = Sum;
            _transaction.CurrencyType = _currency;
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
