using DataStorage;
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
        private FileDataStorage<Wallet> _storage = new FileDataStorage<Wallet>();

        public async Task<List<Wallet>> GetWallets()
        {
            //await _storage.AddOrUpdateAsync(new Wallet("Name", 100, Models.Common.Currency.currencyType.UAH, new List<Models.Categories.Category>(), new Guid(), "Descr"));
            return await _storage.GetAllAsync();
        }
    }
}
