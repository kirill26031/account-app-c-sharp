using System;
using Xunit;
using WalletApp;

namespace CurrencyTest
{
    public class CurrencyTest
    {
        [Fact]
        public void Init()
        {
            decimal ValInUAH = 100;
            Currency.CurrencyType type = Currency.CurrencyType.UAH;

            // expected
            decimal ValInUSD = Decimal.Divide(ValInUAH, Convert.ToDecimal(27.64));

            // assert
            Assert.Equal(Currency.convert(type, Currency.CurrencyType.USD, ValInUAH), ValInUSD);
        }
    }
}
