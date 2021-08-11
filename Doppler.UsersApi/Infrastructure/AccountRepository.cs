using Dapper;
using Doppler.UsersApi.Model;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Doppler.UsersApi.Services;

namespace Doppler.UsersApi.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly IEncryptionService _encryptionService;
        public AccountRepository(IDatabaseConnectionFactory connectionFactory, IEncryptionService encryptionService)
        {
            _connectionFactory = connectionFactory;
            _encryptionService = encryptionService;
        }
        public async Task<ContactInformation> GetContactInformation(string email)
        {
            using var connection = await _connectionFactory.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<ContactInformation>(@"
SELECT
    U.FirstName,
    U.LastName,
    U.Email,
    isnull(I.Code, '') AS Industry,
    U.Company, U.PhoneNumber AS Phone,
    U.Address, U.ZipCode,
    U.CityName AS City,
    isnull(S.StateCode, '') AS Province,
    isnull(CO.Code, '') AS Country,
    U.[AnswerSecurityQuestion],
    U.[IdSecurityQuestion]
FROM
    [User] U
    LEFT JOIN [State] S ON U.IdState = S.IdState
    LEFT JOIN [Country] CO ON S.IdCountry = CO.IdCountry
    LEFT JOIN [Industry] I ON I.IdIndustry = U.IdIndustry
WHERE
    U.Email = @email",
                new { email });

            if (result != null)
            {
                result.AnswerSecurityQuestion = _encryptionService.DecryptAES256(result.AnswerSecurityQuestion);
            }

            return result;
        }

        public async Task UpdateContactInformation(string accountName, ContactInformation contactInformation)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                //Update User
                var rowsAffected = await connection.ExecuteAsync(@"
UPDATE [User] SET
    FirstName = @firstname,
    LastName = @lastname,
    IdIndustry = (SELECT IdIndustry FROM [Industry] WHERE Code = @industrycode),
    Company = @company,
    PhoneNumber = @phonenumber,
    Address = @address,
    ZipCode = @zipcode,
    CityName = @cityname,
    IdState = ISNULL((SELECT IdState FROM [State] WHERE StateCode = @province AND CountryCode = @country), (SELECT IdState FROM [State] WHERE StateCode = 'NO-DEF' AND CountryCode = @country))
WHERE
    Email = @email;",
                new
                {
                    @firstname = contactInformation.Firstname,
                    @lastname = contactInformation.Lastname,
                    @industrycode = contactInformation.Industry,
                    @company = contactInformation.Company,
                    @phonenumber = contactInformation.Phone,
                    @address = contactInformation.Address,
                    @zipcode = contactInformation.ZipCode,
                    @cityname = contactInformation.City,
                    @province = contactInformation.Province,
                    @country = contactInformation.Country,
                    @email = accountName
                });
            }
        }
    }
}
