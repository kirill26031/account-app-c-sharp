using System;
using Xunit;
using WalletApp;

namespace CurrencyTest
{
    public class CurrencyTest
    {
        [Fact]
        public void ConvertFromUAHtoUSD()
        {
            // arrange
            decimal ValInUAH = 100;
            Currency.CurrencyType type = Currency.CurrencyType.UAH;

            // expected
            decimal ValInUSD = Decimal.Divide(ValInUAH, Convert.ToDecimal(27.64));

            // assert
            Assert.Equal(Currency.convert(type, Currency.CurrencyType.USD, ValInUAH), ValInUSD);
        }

        [Fact]
        public void ConvertFromUSDtoUAH()
        {
            // arrange
            decimal ValInUSD = 100;
            Currency.CurrencyType type = Currency.CurrencyType.USD;

            // expected
            decimal ValInUAH = ValInUSD * Convert.ToDecimal(27.64);

            // assert
            Assert.Equal(Currency.convert(type, Currency.CurrencyType.UAH, ValInUSD), ValInUAH);
        }

        [Fact]
        public void ConvertFromUSDtoUSD()
        {
            // arrange
            decimal ValInUSD = 100;
            Currency.CurrencyType type = Currency.CurrencyType.USD;

            // assert
            Assert.Equal(Currency.convert(type, Currency.CurrencyType.USD, ValInUSD), ValInUSD);
        }

        [Fact]
        public void ConvertFromUAHtoUAH()
        {
            // arrange
            decimal ValInUAH = 100;
            Currency.CurrencyType type = Currency.CurrencyType.USD;

            // assert
            Assert.Equal(Currency.convert(type, Currency.CurrencyType.USD, ValInUAH), ValInUAH);
        }
    }
}
