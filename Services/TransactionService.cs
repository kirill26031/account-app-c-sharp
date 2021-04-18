using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Transactions;
using DataStorage;
using WalletApp.WalletAppWPF.Models.Wallets;

namespace WalletApp.WalletAppWPF.Services
{
    public class TransactionService
    {
        public Wallet _wallet;

        public TransactionService(Wallet wallet)
        {
            _wallet = wallet;
        }

        private WalletService _walletService = new WalletService();

        //public async Task<List<Transaction>> GetTransactions()
        //{
        //    return await _transactionStorage.GetAllAsync();
        //}

        public async Task<Wallet> Update(Transaction t)
        {
            _wallet.UpdateTransaction(t.CreatorId, t.Guid, t.Sum, t.CurrencyType, t.Description, t.DateTime, t.Files);
            await _walletService.AddOrUpdate(_wallet);
            return _wallet;
        }

        public async Task<Wallet> Add(Transaction t)
        {
            _wallet.AddTransaction(t);
            await _walletService.AddOrUpdate(_wallet);
            return _wallet;
        }

        public async Task<Wallet> Delete(Transaction transaction)
        {
            _wallet.DeleteTransaction(_wallet.OwnerId, transaction.Guid);
            await _walletService.AddOrUpdate(_wallet);
            return _wallet;
        }
    }
}
