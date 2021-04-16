using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Transactions;

namespace WalletApp.WalletAppWPF.Wallets
{
    class WalletViewModel : NavigationBase<WalletNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        INavigatable<WalletNavigatableTypes> _currentViewModel;
        private User _user;
        private Wallet _currentWallet;
        public WalletViewModel(User user)
        {
            _user = user;
            Navigate(WalletNavigatableTypes.Wallets);
        }

        public MainNavigatableTypes Type => MainNavigatableTypes.Wallets;

        public void ClearSensitiveData()
        {
            _currentViewModel.ClearSensitiveData();
        }

        protected override INavigatable<WalletNavigatableTypes> CreateViewModel(WalletNavigatableTypes type)
        {
            switch (type)
            {
                case WalletNavigatableTypes.Wallets:
                    _currentViewModel = new WalletsViewModel(() => Navigate(WalletNavigatableTypes.AddWallet), _user, _goToTransactions); break;
                case WalletNavigatableTypes.AddWallet:
                    _currentViewModel = new AddWalletViewModel(() => Navigate(WalletNavigatableTypes.Wallets), _user, new Action(DeleteAllOtherViewModels)); break;
                case WalletNavigatableTypes.Transactions:
                    _currentViewModel = new TransactionsViewModel(_user, _currentWallet, () => Navigate(WalletNavigatableTypes.Wallets), () => Navigate(WalletNavigatableTypes.AddTransaction)); break;
                //case WalletNavigatableTypes.AddTransaction:
                //    _currentViewModel = new AddTransactionViewModel(_user, )
            }
            return _currentViewModel;
        }


        private void _goToTransactions(Wallet wallet)
        {
            _currentWallet = wallet;
            Navigate(WalletNavigatableTypes.Transactions);
        }
    }
}
