using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class WalletTests
    {
        List<Category> Categories;
        List<Transaction> UAHTransactions;
        List<Transaction> USDTransactions;
        public WalletTests()
        {
            Categories = new List<Category>();
            Categories.Add(new Category("Sport", "For sport related goods", "awesomeicons.com/12"));
            Categories.Add(new Category("Education", "For online courses", "awesomeicons.com/13"));
            Categories.Add(new Category("Gambling", "For gambling", "awesomeicons.com/14"));
            Categories.Add(new Category("Food", "For food", "awesomeicons.com/15"));

            UAHTransactions = new List<Transaction>();
            USDTransactions = new List<Transaction>();
            for(int i=0; i<2; ++i)
            {
                List<Transaction> CurrentTransactions = i == 0 ? USDTransactions : UAHTransactions;
                Currency.CurrencyType CurrencyType = i == 0 ? Currency.CurrencyType.USD : Currency.CurrencyType.UAH;

                CurrentTransactions.Add(new Transaction(-3000, Categories[2], CurrencyType, "Lost at gambling", DateTimeOffset.Now.AddDays(-20), new List<File>() { 
                    new File(FileType.Image, "peopleindebt.com/126565/photo"),
                    new File(FileType.Text, "legalhelp24-7.com/document/454482")
                }));
                CurrentTransactions.Add(new Transaction(-200, Categories[0], CurrencyType, "Baseball bat", DateTimeOffset.Now.AddDays(-19), new List<File>()));
                CurrentTransactions.Add(new Transaction(-400, Categories[0], CurrencyType, "Baseball protection", DateTimeOffset.Now.AddDays(-19), new List<File>(){
                    new File(FileType.Text, "sportgoods.com/baseball/protection/12")
                }));
                CurrentTransactions.Add(new Transaction(-1000, Categories[1], CurrencyType, "Martial art academy", DateTimeOffset.Now.AddDays(-15), new List<File>()));
                CurrentTransactions.Add(new Transaction(-50, Categories[1], CurrencyType, "Book - how to become a warrior in 10 days", DateTimeOffset.Now.AddDays(-14), new List<File>()));
            }
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
