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
        Wallet _wallet;

        public TransactionService(Wallet wallet)
        {
            _wallet = wallet;
        }

        private FileDataStorage<Transaction> _transactionStorage = new FileDataStorage<Transaction>();

        private FileDataStorage<Wallet> _walletStorage = new FileDataStorage<Wallet>();

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _transactionStorage.GetAllAsync();
        }

        public async Task AddOrUpdate(Transaction transaction)
        {
            await _transactionStorage.AddOrUpdateAsync(transaction);
        }
    }
}
