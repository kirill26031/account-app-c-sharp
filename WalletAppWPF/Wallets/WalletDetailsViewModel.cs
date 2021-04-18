using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Wallets;
using Prism.Mvvm;
using WalletApp.WalletAppWPF.Models.Categories;
using Prism.Commands;
using WalletApp.WalletAppWPF.Services;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Models.Users;

namespace WalletApp.WalletAppWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private Wallet _wallet;
        private Action _shouldUpdate;
        private WalletService _walletService;
        private AuthenticationService _userService;
        private string _name;
        private string _description;
        private User _user;
        private Action<Wallet> _goToTransactions;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                ConfirmEditCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                ConfirmEditCommand.RaiseCanExecuteChanged();
            }
        }

        public decimal Balance
        {
            get
            {
                return _wallet.Balance;
            }
        }

        public string Currency
        {
            get
            {
                return Models.Common.Currency.PrintCurrency(_wallet.Currency);
            }
        }

        public List<Category> Categories
        {
            get
            {
                return _wallet.Categories;
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name} ({_wallet.Balance} {WalletApp.WalletAppWPF.Models.Common.Currency.PrintCurrency(_wallet.Currency)})";
            }
        }

        public DelegateCommand ViewTransactionsCommand => new DelegateCommand(() => _goToTransactions(_wallet));

        public DelegateCommand ConfirmEditCommand { get; }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand DeleteWalletCommand { get; }

        public WalletNavigatableTypes Type => WalletNavigatableTypes.Wallets;

        public Wallet Wallet => _wallet;

        public WalletDetailsViewModel(Wallet wallet, Action shouldUpdate, User user, Action<Wallet> goToTransactions)
        {
            _wallet = wallet;
            _shouldUpdate = shouldUpdate;
            _userService = new AuthenticationService();
            _walletService = new WalletService();
            ConfirmEditCommand = new DelegateCommand(ConfirmEdit, AreChangesExist);
            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
            _name = _wallet.Name;
            _description = _wallet.Description;
            _user = user;
            _goToTransactions = goToTransactions;
        }

        private bool AreChangesExist() => _name != _wallet.Name || _description != _wallet.Description;

        private async void ConfirmEdit()
        {
            _wallet.Name = _name;
            _wallet.Description = _description;
            RaisePropertyChanged(nameof(DisplayName));
            await _walletService.AddOrUpdate(_wallet);
            ConfirmEditCommand.RaiseCanExecuteChanged();
        }

        private async void DeleteWallet()
        {
            var walletsLeft = await _walletService.Delete(_wallet);
            _user.DeleteWallet(_wallet);
            await _userService.UpdateUser(_user);
            _shouldUpdate.Invoke();

            //RaisePropertyChanged("Wallets");
            //RaisePropertyChanged("CurrentWallet");
        }

        public void ClearSensitiveData()
        {
            _name = _wallet.Name;
            _description = _wallet.Description;
        }
    }
}
