using System;
using System.Collections.Generic;
using WalletApp.WalletAppWPF.Models.Wallets;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Transactions;
using DataStorage;

namespace WalletApp.WalletAppWPF.Models.Users
{
    public class User
    {
        public User(Guid guid, string firstName, string lastName, string email, string login)
        {
            Guid = guid;
            _name = firstName;
            _surname = lastName;
            Email = email;
            Login = login;
        }

        public Guid Guid { get; }
        string _name;
        string _login;
        string _surname;
        string _email;
        List<Wallet> _wallets = new List<Wallet>();
        List<Category> _categories = new List<Category>();


        public string FirstName
        {
            get => _name;
            private set => _name = value;
        }

        public string Login
        {
            get => _login;
            private set => _login = value;
        }
        public string LastName
        {
            get => _surname;
            private set => _surname = value;
        }
        public string Email
        {
            get => _email;
            private set => _email = value;
        }
        public List<Wallet> Wallets
        {
            get => _wallets;
            private set => _wallets = value;
        }
        public List<Category> Categories
        {
            get => _categories;
            set => _categories = value;
        }


        public bool AddTransaction(Wallet wallet, decimal sum, Category category, string description, DateTimeOffset dateTime, 
            List<Common.File> files)
        {
            if (!Wallets.Contains(wallet))
                throw new AccessViolationException();
            return wallet.AddTransaction(sum, category, description, dateTime, files, Guid);
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
            wallet.DeleteTransaction(Guid, idTransaction);
        }

        public void UpdateTransaction(Wallet wallet, Guid idTransaction, int sum, string description, DateTimeOffset dateTime, 
            List<Common.File> files)
        {
            if (!Wallets.Contains(wallet))
                throw new AccessViolationException();
            wallet.UpdateTransaction(Guid, idTransaction, sum, description, dateTime, files);
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