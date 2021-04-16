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

namespace WalletApp.WalletAppWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private Wallet _wallet;
        private Action _shouldUpdate;
        private WalletService _service;
        private string _name;
        private string _description;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
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
                return $"{_wallet.Name} ({_wallet.Balance} {WalletApp.WalletAppWPF.Models.Common.Currency.PrintCurrency(_wallet._currency)})";
            }
        }

        public DelegateCommand ConfirmEditCommand        {            get;        }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand DeleteWalletCommand { get; }

        public WalletNavigatableTypes Type => WalletNavigatableTypes.Wallets;

        public WalletDetailsViewModel(Wallet wallet, Action shouldUpdate)
        {
            _wallet = wallet;
            _shouldUpdate = shouldUpdate;
            _service = new WalletService();
            ConfirmEditCommand = new DelegateCommand(ConfirmEdit);
            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
            _name = _wallet.Name;
            _description = _wallet.Description;
        }

        private async void ConfirmEdit()
        {
            _wallet.Name = _name;
            _wallet.Description = _description;
            RaisePropertyChanged(nameof(DisplayName));
            await _service.AddOrUpdate(_wallet);
        }

        private async void DeleteWallet()
        {
            var walletsLeft = await _service.Delete(_wallet);
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
