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
using WalletApp.WalletAppWPF.Navigation;

namespace WalletApp.WalletAppWPF.Transactions
{
    public class AddTransactionViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private Transaction _transaction;
        private readonly Wallet _wallet;
        private User _user;
        private List<Category> _categories;
        private List<Category> _allCategories;
        private TransactionService _transactionService;
        private AuthenticationService _userService;
        Action<Wallet> _goBackToTransactions;

        public string Description
        {
            get => _transaction.Description;
            set
            {
                _transaction.Description = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }
        public decimal Sum
        {
            get => _transaction.Sum;
            set
            {
                _transaction.Sum = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }


        public List<Category> Categories
        {
            get => _allCategories;
            set
            {
                _categories = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsUAHChecked
        {
            get => _transaction.CurrencyType == Models.Common.Currency.currencyType.UAH;
            set
            {
                _transaction.CurrencyType = value ? Models.Common.Currency.currencyType.UAH : Models.Common.Currency.currencyType.USD;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsUSDChecked
        {
            get => _transaction.CurrencyType == Models.Common.Currency.currencyType.USD;
            set
            {
                _transaction.CurrencyType = value ? Models.Common.Currency.currencyType.USD : Models.Common.Currency.currencyType.UAH;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }


        public DateTime DateTime
        {
            get => _transaction.DateTime.DateTime;
            set
            {
                _transaction.DateTime = new DateTimeOffset(value);
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public Transaction Transaction => _transaction;


        public AddTransactionViewModel(Wallet wallet, User user, Action<Wallet> goBackToTransactions)
        {
            _transaction = new Transaction(0, wallet.Categories.First(), Currency.currencyType.UAH, "", new DateTimeOffset(), new List<File>(), user.Guid);
            _user = user;
            _wallet = wallet;
            _allCategories = _wallet.Categories;
            _goBackToTransactions = goBackToTransactions;
            ConfirmCreationCommand = new DelegateCommand(CreateNewTransaction, CanCreate);
            _transactionService = new TransactionService(_wallet);
            _userService = new AuthenticationService();
        }


        public DelegateCommand ConfirmCreationCommand { get; }

        public DelegateCommand GoBackCommand => new DelegateCommand(() => _goBackToTransactions(_wallet));

        public WalletNavigatableTypes Type => WalletNavigatableTypes.AddTransaction;

        private async void CreateNewTransaction()
        {
            _goBackToTransactions.Invoke(await _transactionService.Add(_transaction));
        }

        private bool CanCreate()
        {
            return _wallet.CanAddTransaction(_transaction) && !String.IsNullOrEmpty(Description) && _categories != null && _categories.Count != 0;
        }

        public void ClearSensitiveData()
        {
        }
    }
}
