using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp
{
    public class Wallet
    {
        private Guid _Id;
        private string _Name;
        private decimal _Balance;
        Currency.CurrencyType _Currency;
        List<Transaction> _Transactions;
        List<Category> _Categories;
        Guid _OwnerId;

        public Guid Guid {
            get => _Id; 
            private set => _Id = value; 
        }
        public string Name {
            get => _Name; 
            private set => _Name = value; 
        }
        public decimal Balance { 
            get => _Balance; 
            private set => _Balance = value; 
        }
        public List<Category> Categories { 
            get => _Categories; 
            private set => _Categories = value; 
        }
        public Guid OwnerId { 
            get => _OwnerId; 
            private set => _OwnerId = value; 
        }
        internal Currency.CurrencyType Currency
        {
            get => _Currency;
            private set => _Currency = value;
        }
        internal List<Transaction> Transactions { 
            get => _Transactions; 
            private set => _Transactions = value; 
        }

        public Wallet(string name, decimal balance, Currency.CurrencyType currency, List<Category> categories, Guid ownerId)
        {
            _Id = Guid.NewGuid();
            _Name = name;
            _Balance = balance;
            _Currency = currency;
            _Categories = new List<Category>(categories);
            _OwnerId = ownerId;
        }

        public bool IsOwner(Guid userId) { }

        public bool AddTransaction(Guid userId, decimal sum, Category category, string description, DateTimeOffset dateTimel, List<File> files) { 

        }

        public List<Transaction> ShowTransactions(Guid userId, int startPos = 0, int amountToShow = 10)
        {
            amountToShow = Math.Max(amountToShow, 10);
        }

        public bool deleteTransaction(Guid userId, Guid idTransaction)
        {

        }

        public bool updateTransaction(Guid userId, Guid idTransaction, int sum, string description, DateTimeOffset dateTime, List<File> files)
        {

        }

        public bool updateSumOfTransaction(Guid userId, Guid idTransaction, int sum)
        {
        }

        //...

        public int ExpensesForLastMonth(Guid userId)
        {

        }

        public int IncomeForLastMonth(Guid userId)
        {

        }
    }
}
