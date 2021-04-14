using System;
using DataStorage;

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

        public DBUser(string firstName, string lastName, string email, string login, string hash)
        {
            Guid = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Hash = hash;
        }

    }
}
