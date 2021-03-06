using System;
using System.Collections.Generic;

namespace WalletApp
{
    class User
    {
        Guid _Id;
        string _Name;
        string _Surname;
        string _Email;
        List<Wallet> _Wallets = new List<Wallet>();
        List<Category> _Categories = new List<Category>();

        public Guid Id { 
            get => _Id; 
            private set => _Id = value; 
        }
        public string Name { 
            get => _Name;
            private set => _Name = value; 
        }
        public string Surname { 
            get => _Surname;
            private set => _Surname = value; 
        }
        public string Email { 
            get => _Email; 
            private set => _Email = value;
        }
        public List<Wallet> Wallets { 
            get => _Wallets;
            private set => _Wallets = value; 
        }
        public List<Category> Categories {
            get => _Categories;
            set => _Categories = value; 
        }

        public bool AddTransaction(Wallet wallet, decimal sum, Category category, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            return wallet.AddTransaction(sum, category, description, dateTime, files);
        }

        public List<Transaction> ShowTransactions(Wallet wallet, int startPos = 0, int amountToShow = 10)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            return wallet.ShowTransactions(startPos, amountToShow);
        }

        public void DeleteTransaction(Wallet wallet, Guid idTransaction)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            wallet.DeleteTransaction(Id, idTransaction);
        }

        public void UpdateTransaction(Wallet wallet, Guid idTransaction, int sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            wallet.UpdateTransaction(Id, idTransaction, sum, description, dateTime, files);
        }

        public void UpdateSumOfTransaction(Wallet wallet, Guid idTransaction, int sum)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            wallet.UpdateSumOfTransaction(Id, idTransaction, sum);
        }

        //...

        public decimal ExpensesForLastMonth(Wallet wallet)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            return wallet.ExpensesForLastMonth();
        }

        public decimal IncomeForLastMonth(Wallet wallet)
        {
            if (!Wallets.Contains(wallet)) throw new AccessViolationException();
            return wallet.IncomeForLastMonth();
        }

        public void ShareWallet(Wallet wallet, User user)
        {
            if (!user.HasWallet(wallet)) user.AddWallet(wallet);
        }

        public bool HasWallet(Wallet wallet)
        {
            return Wallets.Contains(wallet);
        }

        public void AddWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
        }
    }
}
