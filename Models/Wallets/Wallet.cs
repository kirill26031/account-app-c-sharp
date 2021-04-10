using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.WalletAppWPF.Models.Wallets
{
    public class Wallet
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Balance})";
        }
    }
}
