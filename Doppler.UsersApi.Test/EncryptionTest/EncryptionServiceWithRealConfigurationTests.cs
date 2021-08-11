using Doppler.UsersApi.Encryption;
using Xunit;

namespace Doppler.UsersApi.Test.EncryptionTest
{
    public class EncryptionServiceWithRealConfigurationTests
    {

        [Theory]
        [InlineData("clear", "79BjLzEu72QDF1QyXUguQA==")]
        [InlineData("12345\"@,%'+$", "8f6EuffZ7Zrfn3Ox1OFQKQ==")]
        [InlineData(" ", "Z0fdwf26Rmo0TgYa2XhoiQ==")]
        [InlineData("29826", "xNF3dznNbH1W07KAQvqwGQ==")]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void EncryptionService_with_real_configuration_should_encrypt_as_expected(string clear, string encrypted)
        {
            // Arrange
            var options = EncryptionTestHelpers.CreateRealEncryptionOptions();
            EncryptionTestHelpers.AssertRealConfigurationValues(options.Value);
            var service = new EncryptionService(options);

            // Act
            var result = service.EncryptAES256(clear);

            // Assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        [InlineData("79BjLzEu72QDF1QyXUguQA==", "clear")]
        [InlineData("8f6EuffZ7Zrfn3Ox1OFQKQ==", "12345\"@,%'+$")]
        [InlineData("Z0fdwf26Rmo0TgYa2XhoiQ==", " ")]
        [InlineData("xNF3dznNbH1W07KAQvqwGQ==", "29826")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void EncryptionService_with_real_configuration_should_decrypt_as_expected(string encrypted, string clear)
        {
            // Arrange
            var options = EncryptionTestHelpers.CreateRealEncryptionOptions();
            EncryptionTestHelpers.AssertRealConfigurationValues(options.Value);
            var service = new EncryptionService(options);

            // Act
            var result = service.DecryptAES256(encrypted);

            // Assert
            Assert.Equal(clear, result);
        }
    }
}
