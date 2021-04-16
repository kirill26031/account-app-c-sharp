using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Services;

namespace WalletApp.WalletAppWPF.Wallets
{
    class AddWalletViewModel : INavigatable<WalletNavigatableTypes>
    {
        private Action _goto;
        private Action _shouldUpdate;
        private WalletService _walletService;
        private List<string> _allCurrencies;
        private List<Category> _categories;
        private DelegateCommand _confirmCreationCommand;
        private DelegateCommand _goBackCommand;
        private Currency.currencyType _currency;
        private string _name;
        private decimal _balance;
        private string _description;
        private Guid _ownerId;

        public AddWalletViewModel(Action goTo, Guid ownerId, Action shouldUpdate)
        {
            _goto = goTo;
            _shouldUpdate = shouldUpdate;
            _walletService = new WalletService();
            _allCurrencies = (from currency in Models.Common.Currency.AllCurrencies() select Models.Common.Currency.PrintCurrency(currency)).ToList();
            _categories = WalletService.AllCategories();
            _confirmCreationCommand = new DelegateCommand(Confirm, CanConfirm);
            _goBackCommand = new DelegateCommand(() => _goto.Invoke());
            _currency = Models.Common.Currency.currencyType.UAH;
            _balance = 0;
            _ownerId = ownerId;
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
            get => _categories;
            set
            {
                _categories = value;
                ConfirmCreationCommand.RaiseCanExecuteChanged();
            }
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
            Wallet wallet = new Wallet(Guid.NewGuid(), Name, Balance, _currency, _categories, _ownerId, Description);
            await _walletService.AddOrUpdate(wallet);
            _shouldUpdate.Invoke();
            _goto.Invoke();
        }

        public bool CanConfirm()
        {
            return true;
        }

        public void ClearSensitiveData()
        {
            
        }
    }
}
