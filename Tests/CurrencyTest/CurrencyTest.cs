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
            decimal valInUAH = 100;
            Currency.currencyType type = Currency.currencyType.UAH;

            // expected
            decimal valInUSD = Decimal.Divide(valInUAH, Convert.ToDecimal(27.64));

            // assert
            Assert.Equal(Currency.Convert(type, Currency.currencyType.USD, valInUAH), valInUSD);
        }

        [Fact]
        public void ConvertFromUSDtoUAH()
        {
            // arrange
            decimal valInUSD = 100;
            Currency.currencyType type = Currency.currencyType.USD;

            // expected
            decimal valInUAH = valInUSD * Convert.ToDecimal(27.64);

            // assert
            Assert.Equal(Currency.Convert(type, Currency.currencyType.UAH, valInUSD), valInUAH);
        }

        [Fact]
        public void ConvertFromUSDtoUSD()
        {
            // arrange
            decimal valInUSD = 100;
            Currency.currencyType type = Currency.currencyType.USD;

            // assert
            Assert.Equal(Currency.Convert(type, Currency.currencyType.USD, valInUSD), valInUSD);
        }

        [Fact]
        public void ConvertFromUAHtoUAH()
        {
            // arrange
            decimal valInUAH = 100;
            Currency.currencyType type = Currency.currencyType.USD;

            // assert
            Assert.Equal(Currency.Convert(type, Currency.currencyType.USD, valInUAH), valInUAH);
        }
    }
}
