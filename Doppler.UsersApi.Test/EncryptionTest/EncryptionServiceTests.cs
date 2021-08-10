using Doppler.UsersApi.Encryption;
using Xunit;

namespace Doppler.UsersApi.Test.EncryptionTest
{
    public class EncryptionServiceTests
    {
        [Theory]
        [InlineData("clear", "dYhbPfgiBnYY9LlMiyRHpw==")]
        [InlineData("12345\"@,%'+$", "TffgclaqDHX2dLZX2q9HIg==")]
        [InlineData(" ", "BNeWpso6CvKBXARk9S7dlg==")]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void EncryptionService_with_arbitrary_configuration_should_encrypt_as_expected(string clear, string encrypted)
        {
            // Arrange
            var service = new EncryptionService(EncryptionTestHelpers.CreateArbitraryEncryptionOptions());

            // Act
            var result = service.EncryptAES256(clear);

            // Assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        [InlineData("dYhbPfgiBnYY9LlMiyRHpw==", "clear")]
        [InlineData("TffgclaqDHX2dLZX2q9HIg==", "12345\"@,%'+$")]
        [InlineData("BNeWpso6CvKBXARk9S7dlg==", " ")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void EncryptionService_with_arbitrary_configuration_should_decrypt_as_expected(string encrypted, string clear)
        {
            // Arrange
            var service = new EncryptionService(EncryptionTestHelpers.CreateArbitraryEncryptionOptions());

            // Act
            var result = service.DecryptAES256(encrypted);

            // Assert
            Assert.Equal(clear, result);
        }

        [Fact]
        public void EncryptionService_should_work_fine_when_it_is_executed_multiple_times()
        {
            // Arrange
            var service = new EncryptionService(EncryptionTestHelpers.CreateArbitraryEncryptionOptions());

            // Act / Assert
            Assert.Equal("dYhbPfgiBnYY9LlMiyRHpw==", service.EncryptAES256("clear"));
            Assert.Equal("TffgclaqDHX2dLZX2q9HIg==", service.EncryptAES256("12345\"@,%'+$"));
            Assert.Equal("clear", service.DecryptAES256("dYhbPfgiBnYY9LlMiyRHpw=="));
            Assert.Equal("12345\"@,%'+$", service.DecryptAES256("TffgclaqDHX2dLZX2q9HIg=="));
            Assert.Equal("TffgclaqDHX2dLZX2q9HIg==", service.EncryptAES256("12345\"@,%'+$"));
        }
    }
}
