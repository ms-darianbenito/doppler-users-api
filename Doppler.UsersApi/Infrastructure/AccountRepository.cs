using Dapper;
using Doppler.UsersApi.Model;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public AccountRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<ContactInformation> GetContactInformation(string email)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                var results = await connection.QueryAsync<ContactInformation>(@"
SELECT
    U.FirstName,
    U.LastName,
    U.Email,
    isnull(I.Code, '') AS Industry,
    U.Company, U.PhoneNumber AS Phone,
    U.Address, U.ZipCode,
    U.CityName AS City,
    isnull(S.Name, '') AS Province,
    isnull(CO.Code, '') AS Country
FROM
    [User] U
    LEFT JOIN [State] S ON U.IdState = S.IdState
    LEFT JOIN [Country] CO ON S.IdCountry = CO.IdCountry
    LEFT JOIN [Industry] I ON I.IdIndustry = U.IdIndustry
WHERE
    U.Email = @email",
                    new { email });
                return results.FirstOrDefault();
            }
        }

        public async Task UpdateContactInformation(string accountName, ContactInformation contactInformation)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                //Get Industry Id
                var industry = await connection.QueryAsync<string>(@"
SELECT
    IdIndustry
FROM
    [Industry]
WHERE
    Code = @industrycode",
                    new { @industrycode = contactInformation.Industry });
                //Get State Id
                //TODO add this condition when column Code is adds to State: StateCode = @province
                var state = await connection.QueryAsync<string>(@"
SELECT
    IdState
FROM
    [State]
WHERE
    --StateCode = @province AND
    CountryCode = @country",
                    new
                    {
                        @province = contactInformation.Province,
                        @country = contactInformation.Country
                    });
                //Update User
                await connection.QueryAsync(@"
UPDATE [User] SET
    FirstName = @firstname,
    LastName = @lastname,
    IdIndustry = @industry,
    Company = @company,
    PhoneNumber = @phonenumber,
    Address = @address,
    ZipCode = @zipcode,
    CityName = @cityname
WHERE
    Email = @email;",
                new
                {
                    @firstname = contactInformation.Firstname,
                    @lastname = contactInformation.Lastname,
                    @industry = industry.FirstOrDefault(),
                    @company = contactInformation.Company,
                    @phonenumber = contactInformation.Phone,
                    @address = contactInformation.Address,
                    @zipcode = contactInformation.ZipCode,
                    @cityname = contactInformation.City,
                    //TODO add this set when column Code is adds to State: IdState = @state y @state = state.FirstOrDefault()
                    @email = accountName
                });
            }
        }
    }
}
