using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Navigation;

namespace WalletApp.WalletAppWPF.Wallets
{
    class WalletViewModel : NavigationBase<WalletNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        INavigatable<WalletNavigatableTypes> _currentViewModel;
        public WalletViewModel()
        {
            Navigate(WalletNavigatableTypes.Wallets);
        }

        public MainNavigatableTypes Type => MainNavigatableTypes.Wallets;

        public void ClearSensitiveData()
        {
            _currentViewModel.ClearSensitiveData();
        }

        protected override INavigatable<WalletNavigatableTypes> CreateViewModel(WalletNavigatableTypes type)
        {
            _currentViewModel = ( type == WalletNavigatableTypes.Wallets ? 
                new WalletsViewModel(() => Navigate(WalletNavigatableTypes.AddWallet)) : 
                new AddWalletViewModel(() => Navigate(WalletNavigatableTypes.Wallets)));
            return _currentViewModel;
        }


    }
}
