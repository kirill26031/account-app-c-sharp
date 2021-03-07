using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class WalletTests
    {
        public List<Category> Categories;
        public List<Transaction> UAHTransactions;
        public List<Transaction> USDTransactions;
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
        public void IllegallyBigTransaction()
        {
            Wallet Wallet = InitWallet();
            decimal Balance = Wallet.Balance;
            try
            {
                Wallet.AddTransaction(-Balance-100, Categories[0], "", DateTimeOffset.Now, new List<File>());
            }
            catch(Exception e){}
            finally
            {
                Assert.Equal(Balance, Wallet.Balance);
            }
        }

        [Fact]
        public void IllegallCategoryTransaction()
        {
            Wallet Wallet = InitWallet();
            AddTransactions(Wallet, UAHTransactions);
            int TransactionsAmount = UAHTransactions.Count;
            try
            {
                Wallet.AddTransaction(100,new Category("", "", ""), "", DateTimeOffset.Now, new List<File>());
            }
            catch (Exception e) { }
            finally
            {
                Assert.Equal(TransactionsAmount, Wallet.ShowTransactions(0, 10).Count);
            }
        }

        [Fact]
        public void IllegalTryToDeleteTransaction()
        {
            Wallet Wallet = InitWallet();
            AddTransactions(Wallet, UAHTransactions);
            int TransactionsAmount = UAHTransactions.Count;
            try
            {
                Wallet.DeleteTransaction(Guid.NewGuid(), UAHTransactions[0].Id);
            }
            catch (Exception e) { }
            finally
            {
                Assert.Equal(TransactionsAmount, Wallet.ShowTransactions(0, 10).Count);
            }

            try
            {
                Wallet.DeleteTransaction(Wallet.OwnerId, Guid.NewGuid());
            }
            catch (Exception e) { }
            finally
            {
                Assert.Equal(TransactionsAmount, Wallet.ShowTransactions(0, 10).Count);
            }
        }

        [Fact]
        public void IllegalTryToUpdateTransaction()
        {
            Wallet Wallet = InitWallet();
            AddTransactions(Wallet, UAHTransactions);
            decimal Balance = Wallet.Balance;
            try
            {
                Wallet.UpdateTransaction(Guid.NewGuid(), UAHTransactions[0].Id, 199, "", DateTimeOffset.Now, new List<File>());
            }
            catch (Exception e) { }
            finally
            {
                Assert.Equal(Balance, Wallet.Balance);
            }

            try
            {
                Wallet.UpdateTransaction(Wallet.OwnerId, Guid.NewGuid(), 199, "", DateTimeOffset.Now, new List<File>());
            }
            catch (Exception e) { }
            finally
            {
                Assert.Equal(Balance, Wallet.Balance);
            }
        }

        [Fact]
        public void AddingTransactions()
        {
            Wallet Wallet = InitWallet();
            decimal Balance = Wallet.Balance;
            foreach(Transaction Tr in UAHTransactions)
            {
                Wallet.AddTransaction(Tr.Sum, Tr.Category, Tr.Description, Tr.DateTime, Tr.Files);
                Balance += Tr.Sum;
                Assert.Equal(Balance, Wallet.Balance, 6);
            }
        }

        [Fact]
        public void ShowTransactions()
        {
            Wallet Wallet = InitWallet();
            decimal Balance = Wallet.Balance;
            Random Random = new Random();
            DateTimeOffset DateTime = DateTimeOffset.Now;
            for (int i=0; i<50; i++)
            {
                Wallet.AddTransaction(i, Categories[0], "", DateTime, new List<File>());
            }
            for(int i = 8; i<=10; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    int StartIndex = j * 5 + i;
                    List<Transaction> Transactions = Wallet.ShowTransactions(StartIndex, i);
                    Assert.Equal(i, Transactions.Count);
                    for(int k=0; k<i; ++k)
                    {
                        Assert.Equal(StartIndex+k, Transactions[k].Sum);
                    }
                }
            }
        }

        [Fact]
        public void DeleteTransactions()
        {
            Wallet Wallet = InitWallet();
            DateTimeOffset DateTime = DateTimeOffset.Now;
            List<Guid> NegativeIds = new List<Guid>();
            Random Random = new Random();
            decimal Sum = Wallet.Balance;
            for (int i=0; i<20; i++)
            {
                int Next = Random.Next(-100, 100);
                Wallet.AddTransaction(Next, Categories[0], "", DateTime, new List<File>());
                Wallet.AddTransaction(-Next, Categories[0], "", DateTime, new List<File>());
                if (Next < 0) NegativeIds.Add(Wallet.ShowTransactions(i*2, 1)[0].Id);
                else NegativeIds.Add(Wallet.ShowTransactions(i * 2+1, 1)[0].Id);
                Sum += Math.Abs(Next);
            }
            foreach(Guid Id in NegativeIds)
            {
                Wallet.DeleteTransaction(Wallet.OwnerId, Id);
            }

            // Assert
            Assert.Equal(Sum, Wallet.Balance);
        }

        [Fact]
        public void UpdateTransactions()
        {
            Wallet Wallet = InitWallet();
            DateTimeOffset DateTime = DateTimeOffset.Now;
            List<Guid> NegativeIds = new List<Guid>();
            Random Random = new Random();
            decimal Sum = Wallet.Balance;
            for (int i = 0; i < 20; i++)
            {
                int Next = Random.Next(-100, 100);
                Wallet.AddTransaction(Next, Categories[0], "", DateTime, new List<File>());
                Wallet.AddTransaction(-Next, Categories[0], "", DateTime, new List<File>());
                if (Next < 0) NegativeIds.Add(Wallet.ShowTransactions(i * 2, 1)[0].Id);
                else NegativeIds.Add(Wallet.ShowTransactions(i * 2 + 1, 1)[0].Id);
                Sum += Math.Abs(Next);
            }
            foreach (Guid Id in NegativeIds)
            {
                Wallet.UpdateTransaction(Wallet.OwnerId, Id, 100, "", DateTime, new List<File>());
            }

            // Assert
            Assert.Equal(Sum+NegativeIds.Count*100, Wallet.Balance);
        }

    }
}
