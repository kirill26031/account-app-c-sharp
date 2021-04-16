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
using WalletApp.WalletAppWPF.Models.Users;

namespace WalletApp.WalletAppWPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private WalletService _service;
        private WalletDetailsViewModel _currentWallet;
        private Action _goto;
        private User _user;
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

        public Action ShouldUpdate => new Action(() => {
            FillWallets(_user.Wallets);
            CurrentWallet = null;
        });
        public WalletsViewModel(Action goTo, User user)
        {
            _goto = goTo;
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            AddWalletCommand = new DelegateCommand(_goto);
            _user = user;
            FillWallets(user.Wallets);
        }

        private void FillWallets(List<Wallet> wallets)
        {
            Wallets.Clear();
            foreach (var wallet in wallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet, ShouldUpdate, _user));
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
