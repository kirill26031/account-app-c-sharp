﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Navigation;

namespace WalletApp.WalletAppWPF.Wallets
{
    class WalletViewModel : NavigationBase<WalletNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        INavigatable<WalletNavigatableTypes> _currentViewModel;
        private User _user;
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
            _currentViewModel = ( type == WalletNavigatableTypes.Wallets ? 
                new WalletsViewModel(() => Navigate(WalletNavigatableTypes.AddWallet), _user) : 
                new AddWalletViewModel(() => Navigate(WalletNavigatableTypes.Wallets), _user, new Action(DeleteAllOtherViewModels)));
            return _currentViewModel;
        }


    }
}
