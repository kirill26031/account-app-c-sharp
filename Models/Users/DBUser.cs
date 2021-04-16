using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataStorage;
using WalletApp.WalletAppWPF.Models.Categories;

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

        public DBUser(string firstName, string lastName, string email, string login, string hash, List<Category> categories)
        {
            Guid = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Hash = hash;
            Categories = categories;
        }

        [JsonConstructor]
        public DBUser(Guid guid, string firstName, string lastName, string email, string login, string hash, List<Category> categories)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Hash = hash;
            Categories = categories;
        }

    }
}
