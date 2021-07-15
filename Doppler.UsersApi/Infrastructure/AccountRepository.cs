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
    U.IdIndustry AS Industry,
    U.Company, U.PhoneNumber AS Phone,
    U.Address, U.ZipCode,
    U.CityName AS City,
    S.Name AS Province,
    CO.Code AS Country
FROM
    [User] U
    INNER JOIN [State] S ON U.IdState = S.IdState
    INNER JOIN [Country] CO ON S.IdCountry = CO.IdCountry
WHERE
    U.Email = @email",
                    new { email });
                return results.FirstOrDefault();
            }
        }
    }
}
