using Prism.Commands;
using System;
using WalletApp.WalletAppWPF.Authentication;
using WalletApp.WalletAppWPF.Navigation;
using WalletApp.WalletAppWPF.Wallets;

namespace WalletApp.WalletAppWPF
{
    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {
        Guid _userGuid;
        public MainViewModel()
        {
            SetUserGUID = (guid) => SetUserGuid(guid);
            Navigate(MainNavigatableTypes.Auth);
        }
        
        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            if (type == MainNavigatableTypes.Auth)
            {
                return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets), SetUserGUID);
            }
            else
            {
                return new WalletViewModel(_userGuid);
            }
        }

        public Action<Guid> SetUserGUID { get; }
        public void SetUserGuid(Guid guid)
        {
            _userGuid = guid;
        }
    }
}
