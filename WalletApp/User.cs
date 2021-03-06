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
    }
}
