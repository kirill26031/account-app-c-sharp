using System;
using System.Collections.Generic;

namespace WalletApp
{
    public class User
    {
        Guid _id;
        string _name;
        string _surname;
        string _email;
        List<Wallet> _wallets = new List<Wallet>();
        List<Category> _categories = new List<Category>();

        public Guid Id { 
            get => _id; 
            private set => _id = value; 
        }
        public string Name { 
            get => _name;
            private set => _name = value; 
        }
        public string Surname { 
            get => _surname;
            private set => _surname = value; 
        }
        public string Email { 
            get => _email; 
            private set => _email = value;
        }
        public List<Wallet> Wallets { 
            get => _wallets;
            private set => _wallets = value; 
        }
        public List<Category> Categories {
            get => _categories;
            set => _categories = value; 
        }

        public bool AddTransaction(Wallet wallet, decimal sum, Category category, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            return wallet.AddTransaction(sum, category, description, dateTime, files, _id);
        }

        public List<Transaction> ShowTransactions(Wallet wallet, int startPos = 0, int amountToShow = 10)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            return wallet.ShowTransactions(startPos, amountToShow);
        }

        public void DeleteTransaction(Wallet wallet, Guid idTransaction)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            wallet.DeleteTransaction(Id, idTransaction);
        }

        public void UpdateTransaction(Wallet wallet, Guid idTransaction, int sum, string description, DateTimeOffset dateTime, List<File> files)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            wallet.UpdateTransaction(Id, idTransaction, sum, description, dateTime, files);
        }

        public decimal ExpensesForLastMonth(Wallet wallet)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            return wallet.BalanceChangesLastMonth(false);
        }

        public decimal IncomeForLastMonth(Wallet wallet)
        {
            if (!Wallets.Contains(wallet)) 
                throw new AccessViolationException();
            return wallet.BalanceChangesLastMonth(true);
        }

        public void ShareWallet(Wallet wallet, User user)
        {
            if (!user.HasWallet(wallet) && HasWallet(wallet)) 
                user.AddWallet(wallet);
        }

        public bool HasWallet(Wallet wallet)
        {
            return Wallets.Contains(wallet);
        }

        private void AddWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
        }
    }
}
