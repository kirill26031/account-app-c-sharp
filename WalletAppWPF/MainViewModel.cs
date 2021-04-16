using Prism.Commands;
using System;
using WalletApp.WalletAppWPF.Authentication;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Wallets;

namespace WalletApp.WalletAppWPF
{
    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {
        User _user;
        public MainViewModel()
        {
            SetUser = (u) => SetUSER(u);
            Navigate(MainNavigatableTypes.Auth);
        }
        
        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            if (type == MainNavigatableTypes.Auth)
            {
                return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets), SetUser);
            }
            else
            {
                return new WalletViewModel(_user);
            }
        }

        public Action<User> SetUser { get; }
        public void SetUSER(User user)
        {
            _user = user;
        }
    }
}
