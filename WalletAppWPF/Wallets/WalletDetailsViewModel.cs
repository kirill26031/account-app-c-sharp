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

namespace WalletApp.WalletAppWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private Wallet _wallet;
        private WalletService _service;

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string Description
        {
            get
            {
                return _wallet.Description;
            }
            set
            {
                _wallet.Description = value;
                RaisePropertyChanged(nameof(DisplayName));
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

        public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
            _service = new WalletService();
            ConfirmEditCommand = new DelegateCommand(ConfirmEdit);
            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
        }

        private async void ConfirmEdit()
        {
            _service.AddOrUpdate(_wallet);
        }

        private async void DeleteWallet()
        {
            _service.Delete(_wallet);
        }
    }
}
