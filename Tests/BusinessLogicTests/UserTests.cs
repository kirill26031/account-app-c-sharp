using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class UserTests
    {
        public List<Category> Categories;
        public List<Transaction> UAHTransactions;
        public List<Transaction> USDTransactions;

        public UserTests()
        {
            WalletTests Wt = new WalletTests();
            Categories = Wt.Categories;
            UAHTransactions = Wt.UAHTransactions;
            USDTransactions = Wt.USDTransactions;
        }

        [Fact]
        public void SimpleSharingWallet()
        {
            decimal InitialBalance = 500;
            decimal Number1 = 100;
            decimal Number2 = -50;
            User First = new User() { Categories = Categories };
            Wallet Wallet = new Wallet("Test wallet of user 1", InitialBalance, Currency.CurrencyType.USD, Categories, First.Id);
            First.Wallets.Add(Wallet);
            User Second = new User() { Categories = Categories };
            First.ShareWallet(Wallet, Second);
            First.AddTransaction(Wallet, Number1, Categories[0], "", DateTimeOffset.Now, new List<File>());
            Second.AddTransaction(Wallet, Number2, Categories[0], "", DateTimeOffset.Now, new List<File>());
            Assert.Equal(InitialBalance + Number1 + Number2, Wallet.Balance);
        }

        [Fact]
        public void TryToUseWalletWithoutAccess()
        {
            User First = new User() { Categories = Categories };
            Wallet Wallet = new Wallet("Test wallet of user 1", 500, Currency.CurrencyType.USD, Categories, First.Id);
            First.Wallets.Add(Wallet);
            User Second = new User() { Categories = Categories };
            try
            {
                Second.AddTransaction(Wallet, 89, Categories[0], "", DateTimeOffset.Now, new List<File>());
            }
            catch (AccessViolationException e) { }
            finally
            {
                Assert.Equal(500, Wallet.Balance);
            }
        }
    }
}
