using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Users;
using WalletApp.WalletAppWPF.Models.Wallets;

namespace WalletApp.WalletAppWPF.Services
{
    public class WalletService
    {
        private FileDataStorage<Wallet> _storage = new FileDataStorage<Wallet>();

        public async Task<List<Wallet>> GetWallets()
        {
            //List<Category> categories = new List<Category>
            //{
            //    new Category("Sport", "For sport related goods", "awesomeicons.com/12", "#ebeb34"),
            //    new Category("Games", "Games category", "", "#dbdb7f")
            //};
            //await _storage.AddOrUpdateAsync(new Wallet("Wallet1", 10, Models.Common.Currency.currencyType.UAH, categories, new List<Models.Transactions.Transaction>(), new Guid(), "Description"));
            return await _storage.GetAllAsync();
        }

        public async void AddOrUpdate(Wallet wallet)
        {
            await _storage.AddOrUpdateAsync(wallet);
        }

        public async void Delete(Wallet wallet)
        {
            _storage.Delete(wallet.Guid);
        }
    }
}
