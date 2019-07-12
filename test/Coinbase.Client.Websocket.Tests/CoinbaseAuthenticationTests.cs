using Coinbase.Client.Websocket.Utils;
using Xunit;

namespace Coinbase.Client.Websocket.Tests
{
    public class CoinbaseAuthenticationTests
    {
        [Fact]
        public void CreateSignature_ShouldReturnCorrectString()
        {
            var nonce = CoinbaseAuthentication.CreateAuthNonce(123456);
            var payload = CoinbaseAuthentication.CreateAuthPayload(nonce);
            var signature = CoinbaseAuthentication.CreateSignature(payload, "api_secret");

            Assert.Equal("f6bea0776d7db5b8f74bc930f5b8d6901376874cfc433cf4b68b688d78238e74", signature);
        }
    }
}
