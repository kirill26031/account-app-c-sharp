using System;
using Xunit;
using WalletApp;

namespace WalletTest
{
    public class WalletTest
    {
        [Fact]
        public void CheckAdd()
        {
            // Arrange
            Wallet w = new Wallet();

            // Expected
            int x = 5;

            // Assert
            Assert.Equal(x, w.Add(2, 3));

        }
    }
}
