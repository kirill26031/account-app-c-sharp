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

namespace WalletApp.WalletAppWPF.Models.Wallets
{
    public class Wallet : IStorable
    {
        public Guid Guid { get; }
        private string _name;
        private decimal _balance;
        Currency.currencyType _currency;
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
            private set => _balance = value;
        }
        public List<Category> Categories
        {
            get => _categories;
            private set => _categories = value;
        }
        public Guid OwnerId
        {
            get => _ownerId;
            private set => _ownerId = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        Currency.currencyType Currency
        {
            // They have private accessibility level
            get => _currency;
            set => _currency = value;
        }
        List<Transaction> Transactions
        {
            // They have private accessibility level
            get => _transactions;
            set => _transactions = value;
        }

        public Wallet(string name, decimal balance, Currency.currencyType currency, List<Category> categories, Guid ownerId, string description)
        {
            Guid = Guid.NewGuid();
            _name = name;
            _balance = balance;
            _currency = currency;
            _categories = new List<Category>(categories);
            _ownerId = ownerId;
            Description = description;
        }

        public bool AddTransaction(decimal sum, Category category, string description, DateTimeOffset dateTime, List<File> files, Guid userId)
        {
            if (_balance >= -sum)
            {
                if (Categories.Contains(category))
                {
                    _balance += sum;
                    Transaction temp = new Transaction(sum, category, _currency, description, dateTime, files, userId);
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
                Balance -= tr.Sum;
                Transactions.RemoveAt(indexToRemove);
            }
            return true;
        }

        public bool UpdateTransaction(Guid userId, Guid idTransaction, decimal sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (userId != OwnerId)
                throw new AccessViolationException();
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Guid == idTransaction)
                {
                    decimal diff = sum - transaction.Sum;
                    decimal newBalance = Balance + diff;
                    if (newBalance < 0) return false;
                    Balance = newBalance;
                    return transaction.UpdateTransaction(sum, description, dateTime, files);
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
                if (DateTimeOffset.Compare(DateTimeOffset.Now.AddMonths(-1), transaction.dateTime) <= 0)
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