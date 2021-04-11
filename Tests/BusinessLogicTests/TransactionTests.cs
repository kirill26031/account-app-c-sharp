using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class TransactionTests
    {
        private List<Category> categories;
        private List<File> files;
        public TransactionTests()
        {
            categories = new List<Category>();
            categories.Add(new Category("Sport", "For sport related goods", "awesomeicons.com/12"));
            categories.Add(new Category("Education", "For online courses", "awesomeicons.com/13"));
            categories.Add(new Category("Gambling", "For gambling", "awesomeicons.com/14"));
            categories.Add(new Category("Food", "For food", "awesomeicons.com/15"));

            files = new List<File>();
            files.Add(new File(fileType.Image, "https://m.imdb.com/title/tt4654462/mediaviewer/rm3092398848/"));
        }

        [Fact]
        public void TransactionCommon()
        {
            // Arrange
            decimal initialBalance_1 = Convert.ToDecimal(970.77);
            List<Category> categoriesCopy_1 = new List<Category>(this.categories);
            Guid ownerId_1 = Guid.NewGuid();
            string description = "desc";
            Wallet wallet_1 = new Wallet("Test wallet 1", initialBalance_1, Currency.currencyType.UAH, categoriesCopy_1, ownerId_1, description);

            decimal initialBalance_2 = 250;
            List<Category> categoriesCopy_2 = new List<Category>(this.categories);
            Guid ownerId_2 = Guid.NewGuid();
            Wallet wallet_2 = new Wallet("Test wallet 2", initialBalance_2, Currency.currencyType.USD, categoriesCopy_2, ownerId_2, description);

            // Act
            wallet_1.AddTransaction(450, categories[0], "Soccers", DateTimeOffset.Now, files, ownerId_1);
            wallet_2.AddTransaction(
                Currency.Convert(Currency.currencyType.UAH, Currency.currencyType.USD, 450),
                categories[0], "Soccers", DateTimeOffset.Now, files, ownerId_2);

            // Assert
            Assert.Equal(wallet_1.ShowTransactions(0, 1)[0].Sum,
                 (Currency.Convert(Currency.currencyType.USD, Currency.currencyType.UAH, wallet_2.ShowTransactions(0, 1)[0].Sum)), 6);
        }
    }
}