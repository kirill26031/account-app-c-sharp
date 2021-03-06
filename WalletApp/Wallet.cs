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
        private Guid _Guid;
        private string _Name;
        private decimal _Balance;
        Currency _Currency;
        List<Transaction> _Transactions;
        List<Category> _Categories;
        Guid _OwnerId;

        public Guid Guid {
            get => _Guid; 
            private set => _Guid = value; 
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
        internal Currency Currency { 
            get => _Currency; 
            private set => _Currency = value; 
        }
        internal List<Transaction> Transactions { 
            get => _Transactions; 
            private set => _Transactions = value; 
        }


    }
}
