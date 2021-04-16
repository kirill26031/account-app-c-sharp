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

        public async Task AddOrUpdate(Wallet wallet)
        {
            await _storage.AddOrUpdateAsync(wallet);
        }

        public async Task<List<Wallet>> Delete(Wallet wallet)
        {
            return await _storage.Delete(wallet.Guid);
        }

        internal async Task<List<Wallet>> WalletsByGuids(List<Guid> walletGuids)
        {
            return (from wallet in (await _storage.GetAllAsync()) where walletGuids.Contains(wallet.Guid) select wallet).ToList();
        }

        public static List<Category> AllCategories()
        {
            List<Category> categories = new List<Category>
            {
                new Category("Sport", "For sport related goods", "awesomeicons.com/12", "#ebeb34"),
                new Category("Games", "Games category", "", "#dbdb7f"),
                new Category("Education", "For online courses", "awesomeicons.com/13", "#58e8d7"),
                new Category("Gambling", "For gambling", "awesomeicons.com/14", "#eb1738"),
                new Category("Food", "For food", "awesomeicons.com/15", "#223001")
            };
            return categories;
        }

        internal async Task<Wallet> GetWallet(Guid guid)
        {
            return await _storage.GetAsync(guid);
        }
    }
}
