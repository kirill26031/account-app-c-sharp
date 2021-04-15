using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Services;

namespace WalletApp.WalletAppWPF.Wallets
{
    class AddWalletViewModel : INavigatable<WalletNavigatableTypes>
    {
        private Action _goto;
        private List<string> _allCurrencies;
        private List<Category> _categories;
        private DelegateCommand _confirmCreationCommand;
        private DelegateCommand _goBackCommand;
        private Currency.currencyType _currency;

        public AddWalletViewModel(Action goTo)
        {
            _goto = goTo;
            _allCurrencies = (from currency in Models.Common.Currency.AllCurrencies() select Models.Common.Currency.PrintCurrency(currency)).ToList();
            _categories = WalletService.AllCategories();
            _confirmCreationCommand = new DelegateCommand(Confirm, CanConfirm);
            _goBackCommand = new DelegateCommand(_goto);
            _currency = Models.Common.Currency.currencyType.UAH;
        }

        public WalletNavigatableTypes Type => WalletNavigatableTypes.AddWallet;

        public List<string> AllCurrencies
        {
            get => _allCurrencies;
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

        public DelegateCommand ConfirmCreationCommand => _confirmCreationCommand;
        public DelegateCommand GoBackCommand => _goBackCommand;

        public void Confirm()
        {

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
