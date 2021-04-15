using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Transactions;
using DataStorage;

namespace WalletApp.WalletAppWPF.Services
{
    public class TransactionService
    {
        private FileDataStorage<Transaction> _storage = new FileDataStorage<Transaction>();

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _storage.GetAllAsync();
        }
    }
}
