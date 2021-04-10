using System;

namespace WalletApp.WalletAppWPF.Navigation
{
    public interface INavigatable<TObject> where TObject: Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
