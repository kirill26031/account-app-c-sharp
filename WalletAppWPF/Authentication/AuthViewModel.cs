using System;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Navigation;

namespace WalletApp.WalletAppWPF.Authentication
{
    public class AuthViewModel : NavigationBase<AuthNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        private Action _signInSuccess;
        private Action<User> _setUser;


        public AuthViewModel(Action signInSuccess, Action<User> setUser)
        {
            _signInSuccess = signInSuccess;
            _setUser = setUser;
            Navigate(AuthNavigatableTypes.SignIn);
        }
        
        protected override INavigatable<AuthNavigatableTypes> CreateViewModel(AuthNavigatableTypes type)
        {
            if (type == AuthNavigatableTypes.SignIn)
            {
                return new SignInViewModel(() => Navigate(AuthNavigatableTypes.SignUp), _signInSuccess, _setUser);
            }
            else
            {
                return new SignUpViewModel(() => Navigate(AuthNavigatableTypes.SignIn));
            }
        }

        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Auth;
            }
        }

        public void ClearSensitiveData()
        {
            CurrentViewModel.ClearSensitiveData();
        }
    }
}
