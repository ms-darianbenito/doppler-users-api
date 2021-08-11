using Xunit;

namespace Doppler.UsersApi.Test.EncryptionTest
{
    public class EncryptionEnvironmentTests
    {
        [Fact]
        public void Environment_variables_should_be_properly_set()
        {
            // Arrange
            var options = EncryptionTestHelpers.CreateRealEncryptionOptions();

            // Assert
            EncryptionTestHelpers.AssertRealConfigurationValues(options.Value);
        }
    }
}
