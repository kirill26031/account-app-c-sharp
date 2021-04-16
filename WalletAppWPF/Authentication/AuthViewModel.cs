using System;
using WalletApp.WalletAppWPF.Navigation;

namespace WalletApp.WalletAppWPF.Authentication
{
    public class AuthViewModel : NavigationBase<AuthNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        private Action _signInSuccess;
        private Action<Guid> _setUserGUID;


        public AuthViewModel(Action signInSuccess, Action<Guid> setUserGUID)
        {
            _signInSuccess = signInSuccess;
            _setUserGUID = setUserGUID;
            Navigate(AuthNavigatableTypes.SignIn);
        }
        
        protected override INavigatable<AuthNavigatableTypes> CreateViewModel(AuthNavigatableTypes type)
        {
            if (type == AuthNavigatableTypes.SignIn)
            {
                return new SignInViewModel(() => Navigate(AuthNavigatableTypes.SignUp), _signInSuccess, _setUserGUID);
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
