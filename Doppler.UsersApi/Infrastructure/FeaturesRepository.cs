using Doppler.UsersApi.Model;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public class FeaturesRepository : IFeaturesRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public FeaturesRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Features> GetFeaturesByUserAccount(string email)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                var results = await connection.QueryAsync<Features>(@"
                    SELECT ISNULL(uf.[ShippingLimitEnabled], ISNULL(utp.[ShippingLimitEnabled], 0)) AS ContactPolicies,
                            ISNULL(uf.[BigQueryEnabled], ISNULL(utp.[BigQueryEnabled], 0)) AS BigQuery,
                            ISNULL(uf.[SmartCampaignsEnabled], ISNULL(utp.[SmartCampaignsEnabled], 0)) AS SmartCampaigns,
                            ISNULL(uf.[SmartCampaignsPlusEnabled], ISNULL(utp.[SmartCampaignsPlusEnabled], 0)) AS SmartCampaingsExtraCustomizations,
                            ISNULL(uf.[SmartSubjectCampaignsEnabled], ISNULL(utp.[SmartSubjectCampaignsEnabled], 0)) AS SmartSubjectCampaigns
                    FROM dbo.[User] U
                    LEFT JOIN UserFeatures UF ON U.IdUser = UF.IdUser
                    LEFT JOIN dbo.BillingCredits BC ON BC.IdBillingCredit = U.IdCurrentBillingCredit
                    LEFT JOIN dbo.UserTypesPlans UTP ON UTP.IdUserTypePlan = BC.IdUserTypePlan
                    WHERE U.Email = @Email",
                    new { Email = email });
                return results.FirstOrDefault();
            }
        }
    }
}
