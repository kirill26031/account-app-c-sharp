using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Services;

namespace WalletApp.WalletAppWPF.Wallets
{
    class AddWalletViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private Action _goto;
        private Action _shouldUpdate;
        private WalletService _walletService;
        private AuthenticationService _userService;
        private List<string> _allCurrencies;
        private List<Category> _categories;
        private List<Category> _allCategories;
        private DelegateCommand _confirmCreationCommand;
        private DelegateCommand _goBackCommand;
        private Currency.currencyType _currency;
        private string _name;
        private decimal _balance;
        private string _description;
        private User _user;

        public AddWalletViewModel(Action goTo, User user, Action shouldUpdate)
        {
            _goto = goTo;
            _shouldUpdate = shouldUpdate;
            _walletService = new WalletService();
            _userService = new AuthenticationService();
            _allCurrencies = (from currency in Models.Common.Currency.AllCurrencies() select Models.Common.Currency.PrintCurrency(currency)).ToList();
            _confirmCreationCommand = new DelegateCommand(Confirm, CanConfirm);
            _goBackCommand = new DelegateCommand(() => _goto.Invoke());
            _balance = 0;
            _user = user;
            _ = FillAllCategories();
        }

        private async Task FillAllCategories()
        {
            _allCategories = await _userService.GetCategoriesForUser(_user.Guid);
            RaisePropertyChanged(nameof(Categories));
        }

        public WalletNavigatableTypes Type => WalletNavigatableTypes.AddWallet;

        public List<string> AllCurrencies
        {
            get => _allCurrencies;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public string Currency
        {
            get => Models.Common.Currency.PrintCurrency(_currency);
            set
            {
                switch (value) 
                {
                    case "UAH": _currency = Models.Common.Currency.currencyType.UAH; break;
                    default: _currency = Models.Common.Currency.currencyType.USD; break;
                }
            }
        }
          
        public List<Category> Categories
        {
            get => _categories == null ? _allCategories : _categories;
            set
            {
                _categories = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Category> AllCategories
        {
            get => _allCategories;
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ConfirmCreationCommand => _confirmCreationCommand;
        public DelegateCommand GoBackCommand => _goBackCommand;

        public DelegateCommand<string> RadioBtnChanged => new DelegateCommand<string>((content) => HandleRadioBtn(content));

        private void HandleRadioBtn(string content)
        {
            Currency = content;
        }

        public async void Confirm()
        {
            Wallet wallet = new Wallet(Guid.NewGuid(), Name, Balance, _currency, _categories, _user.Guid, Description);
            await _walletService.AddOrUpdate(wallet);
            _user.AddWallet(wallet);
            await _userService.UpdateUser(_user);
            _shouldUpdate.Invoke();
            _goto.Invoke();
        }

        public bool CanConfirm()
        {
            return Balance > 0 && !String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(Description) && _categories != null &&
                _categories.Count != 0;
        }

        public void ClearSensitiveData()
        {
            
        }
    }
}
