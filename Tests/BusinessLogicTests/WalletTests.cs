using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class WalletTests
    {
        List<Category> Categories;
        public WalletTests()
        {
            Categories = new List<Category>();
            Categories.Add(new Category("Sport", "For sport related goods", "awesomeicons.com/12"));
            Categories.Add(new Category("Education", "For online courses", "awesomeicons.com/13"));
            Categories.Add(new Category("Gambling", "For gambling", "awesomeicons.com/14"));
            Categories.Add(new Category("Food", "For food", "awesomeicons.com/15"));
        }

        [Fact]
        public void WalletCommon()
        {
            // Setup
            decimal InitialBalance = Convert.ToDecimal(970.77);
            List<Category> CategoriesCopy = new List<Category>(this.Categories);
            Guid OwnerId = Guid.NewGuid();
            Wallet wallet = new Wallet("Test wallet", InitialBalance, Currency.CurrencyType.UAH, CategoriesCopy, OwnerId);
            Assert.Equal(InitialBalance, wallet.Balance);

        }
    }
}
