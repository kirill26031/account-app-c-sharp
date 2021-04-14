using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Models.Wallets;

namespace WalletApp.WalletAppWPF.Services
{
    public class WalletService
    {
        private static List<Wallet> Users = new List<Wallet>()
        {
        };

        public List<Wallet> GetWallets()
        {
            return Users.ToList();
        }
    }
}
