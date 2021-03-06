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

        public bool AddTransaction(decimal sum, Category category, string description, DateTimeOffset dateTime, List<File> files) {
            if(_Balance >= -sum)
            {
                if (Categories.Contains(category)) {
                    _Balance += sum;
                    Transaction temp = new Transaction(sum, category, _Currency, description, dateTime, files);
                    _Transactions.Add(temp);
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
            if(userId != OwnerId)
            {
                throw new AccessViolationException();
            }
            else
            {
                int IndexToRemove = Transactions.FindIndex(Tr => Tr.Id == idTransaction);
                if (IndexToRemove == -1) return false;
                else Transactions.RemoveAt(IndexToRemove);              
            }
            return true;
        }

        public bool UpdateTransaction(Guid userId, Guid idTransaction, decimal sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (userId != OwnerId)
                throw new AccessViolationException();
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Id == idTransaction)
                {
                    decimal Diff = sum - transaction.Sum;
                    decimal NewBalance = Balance + Diff;
                    if (NewBalance < 0) return false;
                    Balance = NewBalance;
                    return transaction.UpdateTransaction(sum, description, dateTime, files);
                }
            }
            throw new ArgumentException();
        }

        public decimal ExpensesForLastMonth()
        {
            decimal sum = 0;
            foreach(Transaction transaction in Transactions)
            {
                if (DateTimeOffset.Compare(DateTimeOffset.Now.AddMonths(-1), transaction.DateTime) <= 0)
                {
                    var expense = transaction.Sum;
                    if (expense < 0)
                    {
                        sum -= expense;
                    }
                }
            }
            return sum;
        }

        public decimal IncomeForLastMonth()
        {
            decimal sum = 0;
            foreach (Transaction transaction in Transactions)
            {
                if (DateTimeOffset.Compare(DateTimeOffset.Now.AddMonths(-1), transaction.DateTime) <= 0)
                {
                    var expense = transaction.Sum;
                    if (expense >= 0)
                    {
                        sum += expense;
                    }
                }
            }
            return sum;
        }
    }
}
