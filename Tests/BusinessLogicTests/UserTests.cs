using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class UserTests
    {
        private List<Category> categories;

        public UserTests()
        {
            WalletTests Wt = new WalletTests();
            categories = Wt.categories;
        }

        [Fact]
        public void SimpleSharingWallet()
        {
            decimal initialBalance = 500;
            decimal sum1 = 100;
            decimal sum2 = -50;
            User user1 = new User() { Categories = categories };
            string description = "desc";
            Wallet wallet = new Wallet("Test wallet of user 1", initialBalance, Currency.currencyType.USD, categories, user1.Id, description);
            user1.Wallets.Add(wallet);
            User user2 = new User() { Categories = categories };
            user1.ShareWallet(wallet, user2);
            user1.AddTransaction(wallet, sum1, categories[0], "", DateTimeOffset.Now, new List<File>());
            user2.AddTransaction(wallet, sum2, categories[0], "", DateTimeOffset.Now, new List<File>());
            Assert.Equal(initialBalance + sum1 + sum2, wallet.Balance);
        }

        [Fact]
        public void TryToUseWalletWithoutAccess()
        {
            User user1 = new User() { Categories = categories };
            string description = "desc";
            Wallet wallet = new Wallet("Test wallet of user 1", 500, Currency.currencyType.USD, categories, user1.Id, description);
            user1.Wallets.Add(wallet);
            User user2 = new User() { Categories = categories };
            try
            {
                user2.AddTransaction(wallet, 89, categories[0], "", DateTimeOffset.Now, new List<File>());
            }
            catch (AccessViolationException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(500, wallet.Balance);
            }
        }
    }
}
