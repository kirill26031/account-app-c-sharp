using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Services;
using Prism.Mvvm;
using Prism.Commands;

namespace WalletApp.WalletAppWPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private WalletService _service;
        private WalletDetailsViewModel _currentWallet;
        private Action _goto;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }
        
        public DelegateCommand AddWalletCommand { get; }

        public WalletsViewModel(Action goTo, Guid _ownerId)
        {
            _goto = goTo;
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            AddWalletCommand = new DelegateCommand(_goto);
            FillWallets();
        }

        private async void  FillWallets()
        {

            foreach(var wallet in await _service.GetWallets())
            {
                Wallets.Add(new WalletDetailsViewModel(wallet, Wallets));
            }
        }

        public WalletNavigatableTypes Type 
        {
            get
            {
                return WalletNavigatableTypes.Wallets;
            }
        }
        public void ClearSensitiveData()
        {
            
        }
    }
}
