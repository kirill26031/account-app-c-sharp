using System;
using Xunit;
using WalletApp;
using System.Collections.Generic;

namespace BusinessLogicTests
{
    public class WalletTests
    {
        public List<Category> categories;
        public List<Transaction> transactionsUAH;
        public List<Transaction> transactionsUSD;
        public WalletTests()
        {
            categories = new List<Category>();
            categories.Add(new Category("Sport", "For sport related goods", "awesomeicons.com/12"));
            categories.Add(new Category("Education", "For online courses", "awesomeicons.com/13"));
            categories.Add(new Category("Gambling", "For gambling", "awesomeicons.com/14"));
            categories.Add(new Category("Food", "For food", "awesomeicons.com/15"));

            transactionsUAH = new List<Transaction>();
            transactionsUSD = new List<Transaction>();
            Guid ownerId_1 = Guid.NewGuid();
            for (int i=0; i<2; ++i)
            {
                List<Transaction> CurrentTransactions = i == 0 ? transactionsUSD : transactionsUAH;
                Currency.currencyType CurrencyType = i == 0 ? Currency.currencyType.USD : Currency.currencyType.UAH;

                CurrentTransactions.Add(new Transaction(-200, categories[3], CurrencyType, "Dinner", DateTimeOffset.Now.AddDays(-40), new List<File>(), ownerId_1));
                CurrentTransactions.Add(new Transaction(-3000, categories[2], CurrencyType, "Lost at gambling", DateTimeOffset.Now.AddDays(-20), new List<File>() { 
                    new File(fileType.Image, "peopleindebt.com/126565/photo"),
                    new File(fileType.Text, "legalhelp24-7.com/document/454482")
                }, ownerId_1));
                CurrentTransactions.Add(new Transaction(-200, categories[0], CurrencyType, "Baseball bat", DateTimeOffset.Now.AddDays(-19), new List<File>(), ownerId_1));
                CurrentTransactions.Add(new Transaction(-400, categories[0], CurrencyType, "Baseball protection", DateTimeOffset.Now.AddDays(-19), new List<File>(){
                    new File(fileType.Text, "sportgoods.com/baseball/protection/12")
                }, ownerId_1));
                CurrentTransactions.Add(new Transaction(-1000, categories[1], CurrencyType, "Martial art academy", DateTimeOffset.Now.AddDays(-15), new List<File>(), ownerId_1));
                CurrentTransactions.Add(new Transaction(-50, categories[1], CurrencyType, "Book - how to become a warrior in 10 days", DateTimeOffset.Now.AddDays(-14), new List<File>(), ownerId_1));
                CurrentTransactions.Add(new Transaction(5000, categories[2], CurrencyType, "Art of communication", DateTimeOffset.Now.AddDays(-3), new List<File>(), ownerId_1));
                CurrentTransactions.Add(new Transaction(500, categories[0], CurrencyType, "Sold almost new sport equipment", DateTimeOffset.Now.AddDays(-2), new List<File>(), ownerId_1));
            }
        }

        private Wallet InitWallet()
        {
            decimal initialBalance = Convert.ToDecimal(5070.77);
            List<Category> categoriesCopy = new List<Category>(this.categories);
            Guid ownerId = Guid.NewGuid();
            string description = "desc";
            Wallet wallet = new Wallet("Test wallet", initialBalance, Currency.currencyType.UAH, categoriesCopy, ownerId, description);
            return wallet;
        }

        private void AddTransactions(Wallet wallet, List<Transaction> transactions)
        {
            foreach (Transaction Tr in transactions)
            {
                wallet.AddTransaction(Tr.Sum, Tr.Category, Tr.Description, Tr.dateTime, Tr.Files, Tr.CreatorId);
            }
        }

        [Fact]
        public void WalletCommon()
        {
            // Setup
            decimal initialBalance = Convert.ToDecimal(970.77);
            List<Category> categoriesCopy = new List<Category>(this.categories);
            Guid ownerId = Guid.NewGuid();
            string description = "desc";
            Wallet wallet = new Wallet("Test wallet", initialBalance, Currency.currencyType.UAH, categoriesCopy, ownerId, description);

            //Assert
            Assert.Equal(initialBalance, wallet.Balance);

        }

        [Fact]
        public void ExpensesAndIncome()
        {
            // Setup
            Wallet wallet = InitWallet();
            AddTransactions(wallet, transactionsUAH);

            //Expected
            decimal expectedMonthlyExpenses = 4650;
            decimal expectedMonthlyIncome = 5500;

            //Assert
            Assert.Equal(expectedMonthlyExpenses, wallet.BalanceChangesLastMonth(false));
            Assert.Equal(expectedMonthlyIncome, wallet.BalanceChangesLastMonth(true));

        }

        [Fact]
        public void IllegallyBigTransaction()
        {
            Wallet wallet = InitWallet();
            decimal balance = wallet.Balance;
            Guid ownerId1 = Guid.NewGuid();
            try
            {
                wallet.AddTransaction(-balance-100, categories[0], "", DateTimeOffset.Now, new List<File>(), ownerId1);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(balance, wallet.Balance);
            }
        }

        [Fact]
        public void IllegallCategoryTransaction()
        {
            Wallet wallet = InitWallet();
            AddTransactions(wallet, transactionsUAH);
            int transactionsAmount = transactionsUAH.Count;
            Guid ownerId1 = Guid.NewGuid();
            try
            {
                wallet.AddTransaction(100,new Category("", "", ""), "", DateTimeOffset.Now, new List<File>(), ownerId1);
            }
            catch (Exception e) { 
                Console.WriteLine(e.ToString()); 
            }
            finally
            {
                Assert.Equal(transactionsAmount, wallet.ShowTransactions(0, 10).Count);
            }
        }

        [Fact]
        public void IllegalTryToDeleteTransaction()
        {
            Wallet wallet = InitWallet();
            AddTransactions(wallet, transactionsUAH);
            int transactionsAmount = transactionsUAH.Count;
            try
            {
                wallet.DeleteTransaction(Guid.NewGuid(), transactionsUAH[0].Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(transactionsAmount, wallet.ShowTransactions(0, 10).Count);
            }

            try
            {
                wallet.DeleteTransaction(wallet.OwnerId, Guid.NewGuid());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(transactionsAmount, wallet.ShowTransactions(0, 10).Count);
            }
        }

        [Fact]
        public void IllegalTryToUpdateTransaction()
        {
            Wallet wallet = InitWallet();
            AddTransactions(wallet, transactionsUAH);
            decimal balance = wallet.Balance;
            try
            {
                wallet.UpdateTransaction(Guid.NewGuid(), transactionsUAH[0].Id, 199, "", DateTimeOffset.Now, new List<File>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(balance, wallet.Balance);
            }

            try
            {
                wallet.UpdateTransaction(wallet.OwnerId, Guid.NewGuid(), 199, "", DateTimeOffset.Now, new List<File>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Assert.Equal(balance, wallet.Balance);
            }
        }

        [Fact]
        public void AddingTransactions()
        {
            Wallet wallet = InitWallet();
            decimal balance = wallet.Balance;
            foreach(Transaction tr in transactionsUAH)
            {
                wallet.AddTransaction(tr.Sum, tr.Category, tr.Description, tr.dateTime, tr.Files, tr.CreatorId);
                balance += tr.Sum;
                Assert.Equal(balance, wallet.Balance, 6);
            }
        }

        [Fact]
        public void ShowTransactions()
        {
            Wallet wallet = InitWallet();
            decimal balance = wallet.Balance;
            Random random = new Random();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            Guid ownerId1 = Guid.NewGuid();
            for (int i=0; i<50; i++)
            {
                wallet.AddTransaction(i, categories[0], "", dateTime, new List<File>(), ownerId1);
            }
            for(int i = 8; i<=10; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    int startIndex = j * 5 + i;
                    List<Transaction> Transactions = wallet.ShowTransactions(startIndex, i);
                    Assert.Equal(i, Transactions.Count);
                    for(int k=0; k<i; ++k)
                    {
                        Assert.Equal(startIndex+k, Transactions[k].Sum);
                    }
                }
            }
        }

        [Fact]
        public void DeleteTransactions()
        {
            Wallet wallet = InitWallet();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            List<Guid> negativeIds = new List<Guid>();
            Random random = new Random();
            decimal sum = wallet.Balance;
            Guid ownerId1 = Guid.NewGuid();
            for (int i=0; i<20; i++)
            {
                int next = random.Next(-100, 100);
                wallet.AddTransaction(next, categories[0], "", dateTime, new List<File>(), ownerId1);
                wallet.AddTransaction(-next, categories[0], "", dateTime, new List<File>(), ownerId1);
                if (next < 0) negativeIds.Add(wallet.ShowTransactions(i*2, 1)[0].Id);
                else negativeIds.Add(wallet.ShowTransactions(i * 2+1, 1)[0].Id);
                sum += Math.Abs(next);
            }
            foreach(Guid id in negativeIds)
            {
                wallet.DeleteTransaction(wallet.OwnerId, id);
            }

            // Assert
            Assert.Equal(sum, wallet.Balance);
        }

        [Fact]
        public void UpdateTransactions()
        {
            Wallet wallet = InitWallet();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            List<Guid> negativeIds = new List<Guid>();
            Random random = new Random();
            decimal sum = wallet.Balance;
            Guid ownerId1 = Guid.NewGuid();
            for (int i = 0; i < 20; i++)
            {
                int next = random.Next(-100, 100);
                wallet.AddTransaction(next, categories[0], "", dateTime, new List<File>(), ownerId1);
                wallet.AddTransaction(-next, categories[0], "", dateTime, new List<File>(), ownerId1);
                if (next < 0) negativeIds.Add(wallet.ShowTransactions(i * 2, 1)[0].Id);
                else negativeIds.Add(wallet.ShowTransactions(i * 2 + 1, 1)[0].Id);
                sum += Math.Abs(next);
            }
            foreach (Guid id in negativeIds)
            {
                wallet.UpdateTransaction(wallet.OwnerId, id, 100, "", dateTime, new List<File>());
            }

            // Assert
            Assert.Equal(sum+negativeIds.Count*100, wallet.Balance);
        }

    }
}
