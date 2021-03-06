using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class TransactionTests
    {
        List<Category> Categories;
        List<File> Files;
        public TransactionTests()
        {
            Categories = new List<Category>();
            Categories.Add(new Category("Sport", "For sport related goods", "awesomeicons.com/12"));
            Categories.Add(new Category("Education", "For online courses", "awesomeicons.com/13"));
            Categories.Add(new Category("Gambling", "For gambling", "awesomeicons.com/14"));
            Categories.Add(new Category("Food", "For food", "awesomeicons.com/15"));

            Files = new List<File>();
            Files.Add(new File(FileType.Image, "https://m.imdb.com/title/tt4654462/mediaviewer/rm3092398848/"));
        }

        [Fact]
        public void TransactionCommon()
        {
            // Arrange
            decimal InitialBalance_1 = Convert.ToDecimal(970.77);
            List<Category> CategoriesCopy_1 = new List<Category>(this.Categories);
            Guid OwnerId_1 = Guid.NewGuid();
            Wallet wallet_1 = new Wallet("Test wallet 1", InitialBalance_1, Currency.CurrencyType.UAH, CategoriesCopy_1, OwnerId_1);

            decimal InitialBalance_2 = 250;
            List<Category> CategoriesCopy_2 = new List<Category>(this.Categories);
            Guid OwnerId_2 = Guid.NewGuid();
            Wallet wallet_2 = new Wallet("Test wallet 2", InitialBalance_2, Currency.CurrencyType.USD, CategoriesCopy_2, OwnerId_2);

            // Act
            wallet_1.AddTransaction(450, Categories[0], "Soccers", DateTimeOffset.Now, Files);
            wallet_2.AddTransaction(
                Currency.convert(Currency.CurrencyType.UAH, Currency.CurrencyType.USD, 450), 
                Categories[0], "Soccers", DateTimeOffset.Now, Files);

            // Assert

            //Math.Round //Assert fails due to 450 != 449.9999999..., division error is assumed 
            Assert.Equal(wallet_1.ShowTransactions(0,1)[0].Sum, 
                 (Currency.convert(Currency.CurrencyType.USD, Currency.CurrencyType.UAH, wallet_2.ShowTransactions(0,1)[0].Sum)), 6);
        }
    }
}
