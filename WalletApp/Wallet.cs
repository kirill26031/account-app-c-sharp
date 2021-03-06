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

        public void AddTransaction(decimal sum, Category category, string description, DateTimeOffset dateTime, List<File> files) {
            //check if enough money
            if(_Balance >= -sum)
            {
                _Balance += sum;
                Transaction temp = new Transaction(sum, category, _Currency, description, dateTime, files);
                _Transactions.Add(temp);
            }
            else
            {
                throw new ArithmeticException();
            }
        }

        public List<Transaction> ShowTransactions(int startPos, int amountToShow)
        {
            List<Transaction> temp = new List<Transaction>();
            for (var i = startPos; i < Transactions.Count() + amountToShow; i++)
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
                var i = 0;
                foreach (Transaction transaction in Transactions)
                {
                    if (transaction.Id == idTransaction)
                    {
                        Transactions.RemoveAt(i);
                        return true;
                    }
                    else
                        i++;
                }                
            }
            return false;
        }

        public bool UpdateTransaction(Guid userId, Guid idTransaction, decimal sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            return false;
        }

        public bool UpdateSumOfTransaction(Guid userId, Guid idTransaction, decimal sum)
        {
            Transaction Tr = Transactions.Find(Tr => Tr.Id == idTransaction);
            if (Tr == null) throw new ArgumentException();
            return UpdateTransaction(userId, idTransaction, sum, Tr.Description, Tr.DateTime, Tr.Files);
        }

        public bool UpdateDescriptionOfTransaction(Guid userId, Guid idTransaction, string description)
        {
            Transaction Tr = Transactions.Find(Tr => Tr.Id == idTransaction);
            if (Tr == null) throw new ArgumentException();
            return UpdateTransaction(userId, idTransaction, Tr.Sum, description, Tr.DateTime, Tr.Files);
        }

        //...

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
                        sum += expense;
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
