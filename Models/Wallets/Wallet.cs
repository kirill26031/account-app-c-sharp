using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.WalletAppWPF.Models.Transactions;
using WalletApp.WalletAppWPF.Models.Common;
using WalletApp.WalletAppWPF.Models.Categories;
using DataStorage;
using System.Text.Json.Serialization;

namespace WalletApp.WalletAppWPF.Models.Wallets
{
    public class Wallet : IStorable
    {
        public Guid Guid { get; }
        private string _name;
        private decimal _balance;
        private Currency.currencyType _currency;
        
        private List<Transaction> _transactions = new List<Transaction>();
        private List<Category> _categories = new List<Category>();
        Guid _ownerId;
        string _description;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public decimal Balance
        {
            get => _balance;
            set => _balance = value;
        }
        public List<Category> Categories
        {
            get => _categories;
            set => _categories = value;
        }
        public Guid OwnerId
        {
            get => _ownerId;
            set => _ownerId = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public Currency.currencyType Currency
        {
            get => _currency;
            set => _currency = value;
        }
        public List<Transaction> Transactions
        {
            // They have private accessibility level
            get => _transactions;
            set => _transactions = value;
        }

        
        public Wallet(Guid guid, string name, decimal balance, Currency.currencyType currency, List<Category> categories, Guid ownerId, string description)
        {
            Guid = guid;
            Name = name;
            Balance = balance;
            Currency = currency;
            Categories = new List<Category>(categories);
            Transactions = new List<Transaction>();
            OwnerId = ownerId;
            Description = description;
        }

        [JsonConstructor]
        public Wallet(Guid guid, string name, decimal balance, Currency.currencyType currency, List<Category> categories, List<Transaction> transactions, Guid ownerId, string description)
        {
            Guid = guid;
            Name = name;
            Balance = balance;
            Currency = currency;
            Categories = new List<Category>(categories);
            OwnerId = ownerId;
            Description = description;
            Transactions = transactions;
        }

        public Wallet(string name, decimal balance, Guid ownerId, string description)
        {
            Guid = Guid.NewGuid();
            Name = name;
            OwnerId = ownerId;
            Description = description;
        }

        public Wallet(Wallet wallet)
        {
            _balance = wallet._balance;
            _categories = wallet._categories;
            Guid = wallet.Guid;
            _currency = wallet._currency;
            _description = wallet._description;
            _name = wallet._name;
            _ownerId = wallet._ownerId;
            _transactions = wallet._transactions;
        }

        public void AddTransaction(Transaction t)
        {
            AddTransaction(t.Guid, t.Sum, t.CurrencyType, t.Category, t.Description, t.DateTime, t.Files, t.CreatorId);
        }

        public bool AddTransaction(Guid guid, decimal sum, Currency.currencyType currencyType, Category category, string description, DateTimeOffset dateTime, List<File> files, Guid userId)
        {
            decimal transformedSum = Models.Common.Currency.Convert(currencyType, Currency, sum);
            if (_balance >= -sum)
            {
                if (Categories.Contains(category))
                {
                    _balance += transformedSum;
                    Transaction temp = new Transaction(guid, sum, category, currencyType, description, dateTime, files, userId);
                    _transactions.Add(temp);
                    return true;
                }
                else
                    throw new AccessViolationException();
            }
            else
                throw new ArithmeticException();
        }

        public List<Transaction> ShowTransactions(int startPos, int amountToShow)
        {
            amountToShow %= 10;
            if (amountToShow == 0)
                amountToShow = 10;
            List<Transaction> temp = new List<Transaction>();
            for (var i = startPos; i < Math.Min(Transactions.Count(), startPos + amountToShow); i++)
            {
                temp.Add(Transactions[i]);
            }
            return temp;
        }

        public bool DeleteTransaction(Guid userId, Guid idTransaction)
        {
            if (userId != OwnerId)
            {
                throw new AccessViolationException();
            }
            else
            {
                int indexToRemove = Transactions.FindIndex(Tr => Tr.Guid == idTransaction);
                if (indexToRemove == -1) return false;
                Transaction tr = Transactions[indexToRemove];
                decimal transformedSum = Models.Common.Currency.Convert(tr.CurrencyType, Currency, tr.Sum);
                Balance -= transformedSum;
                Transactions.RemoveAt(indexToRemove);
            }
            return true;
        }

        public bool UpdateTransaction(Guid userId, Guid idTransaction, decimal sum, Currency.currencyType currency, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (userId != OwnerId)
                throw new AccessViolationException();
            decimal transformedSum = Models.Common.Currency.Convert(currency, Currency, sum);
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Guid == idTransaction)
                {
                    decimal diff = transformedSum - transaction.Sum;
                    decimal newBalance = Balance + diff;
                    if (newBalance < 0) return false;
                    Balance = newBalance;
                    return transaction.UpdateTransaction(sum, currency, description, dateTime, files);
                }
            }
            throw new ArgumentException();
        }

        //if income == true then returns income, else returns expenses
        public decimal BalanceChangesLastMonth(bool income)
        {
            decimal sum = 0;
            foreach (Transaction transaction in Transactions)
            {
                if (DateTimeOffset.Compare(DateTimeOffset.Now.AddMonths(-1), transaction.DateTime) <= 0)
                {
                    var expense = transaction.Sum;
                    if (income)
                    {
                        if (expense >= 0)
                        {
                            sum += expense;
                        }
                    }
                    else
                    {
                        if (expense < 0)
                        {
                            sum -= expense;
                        }
                    }
                }
            }
            return sum;
        }

        public override string ToString()
        {
            return _ownerId + " " + _name + " " + _balance;
        }
    }
}