using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DataStorage;
using WalletApp.WalletAppWPF.Models.Categories;
using WalletApp.WalletAppWPF.Models.Wallets;

namespace WalletApp.WalletAppWPF.Models.Users
{
    public class DBUser : IStorable
    {
        public Guid Guid { get; }
        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get;}
        public string Login { get;}
        public string Hash { get;}

        public List<Category> Categories { get; }

        public List<Guid> WalletGuids { get; }

        public DBUser(Guid guid, string firstName, string lastName, string email, string login, string hash, List<Category> categories, List<Wallet> wallets)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Hash = hash;
            Categories = categories;
            WalletGuids = (from wallet in wallets select wallet.Guid).ToList(); 
        }

        [JsonConstructor]
        public DBUser(Guid guid, string firstName, string lastName, string email, string login, string hash, List<Category> categories, List<Guid> walletGuids)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Hash = hash;
            Categories = categories;
            WalletGuids = walletGuids;
        }

    }
}
