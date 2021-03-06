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

                CurrentTransactions.Add(new Transaction(-200, Categories[3], CurrencyType, "Dinner", DateTimeOffset.Now.AddDays(-40), new List<File>()));
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
                CurrentTransactions.Add(new Transaction(5000, Categories[2], CurrencyType, "Art of communication", DateTimeOffset.Now.AddDays(-3), new List<File>()));
                CurrentTransactions.Add(new Transaction(500, Categories[0], CurrencyType, "Sold almost new sport equipment", DateTimeOffset.Now.AddDays(-2), new List<File>()));
            }
        }

        private Wallet InitWallet()
        {
            decimal InitialBalance = Convert.ToDecimal(5070.77);
            List<Category> CategoriesCopy = new List<Category>(this.Categories);
            Guid OwnerId = Guid.NewGuid();
            Wallet wallet = new Wallet("Test wallet", InitialBalance, Currency.CurrencyType.UAH, CategoriesCopy, OwnerId);
            return wallet;
        }

        private void AddTransactions(Wallet wallet, List<Transaction> transactions)
        {
            foreach (Transaction Tr in transactions)
            {
                wallet.AddTransaction(Tr.Sum, Tr.Category, Tr.Description, Tr.DateTime, Tr.Files);
            }
        }

        [Fact]
        public void WalletCommon()
        {
            // Setup
            decimal InitialBalance = Convert.ToDecimal(970.77);
            List<Category> CategoriesCopy = new List<Category>(this.Categories);
            Guid OwnerId = Guid.NewGuid();
            Wallet Wallet = new Wallet("Test wallet", InitialBalance, Currency.CurrencyType.UAH, CategoriesCopy, OwnerId);

            //Assert
            Assert.Equal(InitialBalance, Wallet.Balance);

        }

        [Fact]
        public void ExpensesAndIncome()
        {
            // Setup
            Wallet Wallet = InitWallet();
            AddTransactions(Wallet, UAHTransactions);

            //Expected
            decimal ExpectedMonthlyExpenses = 4650;
            decimal ExpectedMonthlyIncome = 5500;

            //Assert
            Assert.Equal(ExpectedMonthlyExpenses, Wallet.ExpensesForLastMonth());
            Assert.Equal(ExpectedMonthlyIncome, Wallet.IncomeForLastMonth());

        }

        [Fact]
        public void IllegalTransactionsAdding()
        {
            Wallet Wallet = InitWallet();
            try
            {
                Wallet.AddTransaction(-6001, Categories[0], "", DateTimeOffset.Now, new List<File>());
            }
            catch(Exception e){}
            finally
            {
                Assert.Equal(5000, Wallet.Balance);
            }
        }

    }
}
