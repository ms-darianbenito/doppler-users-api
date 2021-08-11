using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Doppler.UsersApi.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace Doppler.UsersApi.Test.EncryptionTest
{
    public static class EncryptionTestHelpers
    {
        public static IOptions<EncryptionSettings> CreateRealEncryptionOptions()
            => Options.Create(new ConfigurationBuilder()
                // The following environment variables should be defined with the real values:
                // * EncryptionSettings__InitVectorAsAsciiString
                // * EncryptionSettings__SaltValueAsAsciiString
                // * EncryptionSettings__Password
                // See more information and configuration in http://confluence.makingsense.com/display/DOP/Variables+de+entorno
                //  TODO: consider also using appsettings.json from DopplerForms project
                // .SetBasePath(Directory.GetCurrentDirectory())
                // .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .AddJsonFile("/run/secrets/appsettings.Secret.json", true)
                .AddKeyPerFile("/run/secrets", true)
                .Build()
                .GetSection(nameof(EncryptionSettings))
                .Get<EncryptionSettings>());

        public static IOptions<EncryptionSettings> CreateArbitraryEncryptionOptions()
            => Options.Create(new EncryptionSettings()
            {
                InitVectorAsAsciiString = "fedcba9876543210",
                SaltValueAsAsciiString = "0123456789abcdef",
                Password = "Esto es una prueba."
            });

        public static void AssertRealConfigurationValues(EncryptionSettings settings)
        {
            var seeMore = "See more information and configuration in http://confluence.makingsense.com/display/DOP/Variables+de+entorno";

            Assert.True(
                settings != null,
                $"{nameof(settings)} is Null. {seeMore}");
            Assert.True(
                settings.InitVectorAsAsciiString != null,
                $"{nameof(settings.InitVectorAsAsciiString)} is Null. {seeMore}");
            Assert.True(
                settings.SaltValueAsAsciiString != null,
                $"{nameof(settings.SaltValueAsAsciiString)} is Null. {seeMore}");
            Assert.True(
                settings.Password != null,
                $"{nameof(settings.Password)} is Null. {seeMore}");

            using var sha1 = SHA1.Create();
            string ComputeSha1(string value) =>
                string.Join(string.Empty, sha1.ComputeHash(Encoding.UTF8.GetBytes(value)).Select(x => x.ToString("x2")));

            Assert.True(
                "3e78685f77d9290435061f5a99c817bfae7fa393" == ComputeSha1(settings.InitVectorAsAsciiString),
                $"{nameof(settings.InitVectorAsAsciiString)} value is wrong. {seeMore}");
            Assert.True(
                "7b02d469d41c05010a4ebfb800a931198b7251b9" == ComputeSha1(settings.SaltValueAsAsciiString),
                $"{nameof(settings.SaltValueAsAsciiString)} value is wrong. {seeMore}");
            Assert.True(
                "2c6d6b31462b179a9100192737390ad1d7469ce0" == ComputeSha1(settings.Password),
                $"{nameof(settings.Password)} value is wrong. {seeMore}");
        }
    }
}
