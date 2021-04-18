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

    //Currently suppports transactions in result of which can have negative balance. (lines 119, 192)
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
            if (Categories.Contains(category))
            {
                Transaction temp = new Transaction(guid, sum, category, currencyType, description, dateTime, files, userId);
                if (CanAddTransaction(temp))
                {
                    decimal transformedSum = Models.Common.Currency.Convert(currencyType, Currency, sum);
                    _balance += transformedSum;
                    _transactions.Add(temp);
                    return true;
                }
                else return false;
            }
            else
                throw new AccessViolationException();
        }

        public bool CanAddTransaction(Transaction transaction)
        {
            decimal transformedSum = Models.Common.Currency.Convert(transaction.CurrencyType, Currency, transaction.Sum);
            return transformedSum >= 0 || Balance >= -transformedSum;
        }

        public bool CanDeleteTransaction(Transaction transaction)
        {
            decimal transformedSum = Models.Common.Currency.Convert(transaction.CurrencyType, Currency, transaction.Sum);
            return transformedSum <= 0 || Balance >= transformedSum;
        }

        public bool CanUpdateTransaction(Transaction fromTransaction, Transaction toTransaction)
        {
            var balance = Balance;
            decimal fromTransformedSum = Models.Common.Currency.Convert(fromTransaction.CurrencyType, Currency, fromTransaction.Sum);
            decimal toTransformedSum = Models.Common.Currency.Convert(toTransaction.CurrencyType, Currency, toTransaction.Sum);
            balance += toTransformedSum - fromTransformedSum;

            return balance >= 0;
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
                if (CanDeleteTransaction(tr))
                {
                    decimal transformedSum = Models.Common.Currency.Convert(tr.CurrencyType, Currency, tr.Sum);
                    Balance -= transformedSum;
                    Transactions.RemoveAt(indexToRemove);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateTransaction(Guid userId, Guid idTransaction, decimal sum, Currency.currencyType currency, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (userId != OwnerId)
                throw new AccessViolationException();
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Guid == idTransaction)
                {
                    Transaction temp = new Transaction(sum, transaction.Category, currency, description, dateTime, files, userId);
                    if (!CanUpdateTransaction(transaction, temp))
                        return false;
                    decimal diff = sum - Models.Common.Currency.Convert(transaction.CurrencyType, currency, transaction.Sum);
                    decimal newBalance = Balance + Models.Common.Currency.Convert(currency, Currency, diff);
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