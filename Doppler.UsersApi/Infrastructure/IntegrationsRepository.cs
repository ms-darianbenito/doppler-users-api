using Doppler.UsersApi.Model;
using System.Data;
using System.Linq;
using Dapper;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public class IntegrationsRepository : IIntegrationsRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public IntegrationsRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Integrations> GetIntegrationsStatusByUserAccount(string email)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                var results = await connection.QueryAsync<Integrations>(@"
SELECT
    CASE
        WHEN u.ApiKey IS NULL
            THEN 'disconnected'
        ELSE 'connected'
        END ApiKeyStatus
    ,CASE
        WHEN dkim.AlertCounter > 0
            THEN 'alert'
        WHEN dkim.ConnectedCounter > 0
            THEN 'connected'
        ELSE 'disconnected'
        END DkimStatus
    ,CASE
        WHEN CustomDomain.AlertCounter > 0
            THEN 'alert'
        WHEN CustomDomain.ConnectedCounter > 0
            THEN 'connected'
        ELSE 'disconnected'
        END CustomDomainStatus
    ,CASE
        WHEN integrations.[Tokko Broker] > 9
            THEN 'alert'
        WHEN integrations.[Tokko Broker] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END TokkoStatus
    ,CASE
        WHEN integrations.[TiendaNube] > 9
            THEN 'alert'
        WHEN integrations.[TiendaNube] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END TiendanubeStatus
    ,CASE
        WHEN integrations.[Datahub] > 9
            THEN 'alert'
        WHEN integrations.[Datahub] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END DatahubStatus
    ,CASE
        WHEN integrations.[VTEX] > 9
            THEN 'alert'
        WHEN integrations.[VTEX] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END VtexStatus
    ,CASE
        WHEN integrations.[PrestaShop] > 9
            THEN 'alert'
        WHEN integrations.[PrestaShop] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END PrestashopStatus
    ,CASE
        WHEN integrations.[Shopify] > 9
            THEN 'alert'
        WHEN integrations.[Shopify] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END ShopifyStatus
    ,CASE
        WHEN integrations.[Magento] > 9
            THEN 'alert'
        WHEN integrations.[Magento] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END MagentoStatus
    ,CASE
        WHEN integrations.[Zoho] > 9
            THEN 'alert'
        WHEN integrations.[Zoho] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END ZohoStatus
    ,CASE
        WHEN integrations.[WooCommerce] > 9
            THEN 'alert'
        WHEN integrations.[WooCommerce] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END WooCommerceStatus
    ,CASE
        WHEN integrations.[Easycommerce] > 9
            THEN 'alert'
        WHEN integrations.[Easycommerce] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END EasycommerceStatus
    ,CASE
        WHEN integrations.[BmwRspCrm] > 9
            THEN 'alert'
        WHEN integrations.[BmwRspCrm] IS NOT NULL
            THEN 'connected'
        ELSE 'disconnected'
        END BmwRspCrmStatus
FROM dbo.[user] u
LEFT JOIN (
    SELECT PVT.iduser
        ,PVT.[2] [Tokko Broker]
        ,PVT.[3] [TiendaNube]
        ,PVT.[4] [Datahub]
        ,PVT.[5] [VTEX]
        ,PVT.[6] [PrestaShop]
        ,PVT.[7] [Shopify]
        ,PVT.[8] [Magento]
        ,PVT.[9] [Zoho]
        ,PVT.[10] [WooCommerce]
        ,PVT.[11] [Easycommerce]
        ,PVT.[12] [BmwRspCrm]
    FROM (
        SELECT tpaxu.IdUser
            ,tpaxu.ConnectionErrors
            ,tpaxu.IdThirdPartyApp
        FROM dbo.ThirdPartyAppXUser tpaxu
        JOIN dbo.[user] u ON u.iduser = tpaxu.iduser
        WHERE u.Email = @Email
        ) AS tabExpr
    PIVOT(SUM(ConnectionErrors) FOR IdThirdPartyApp IN (
                [2]
                ,[3]
                ,[4]
                ,[5]
                ,[6]
                ,[7]
                ,[8]
                ,[9]
                ,[10]
                ,[11]
                ,[12]
                )) AS PVT
    ) integrations ON integrations.iduser = u.iduser
LEFT JOIN (
    SELECT dixu.IdUser
        ,COUNT(CASE
                WHEN IdDomainSpfStatus = 3
                    OR IdDomainKeyStatus = 3
                    THEN 1
                WHEN IdDomainSpfStatus = 1
                    OR IdDomainKeyStatus = 1
                    THEN 1
                END) AlertCounter
        ,COUNT(CASE
                WHEN IdDomainSpfStatus = 2
                    AND IdDomainKeyStatus = 2
                    THEN 1
                END) ConnectedCounter
    FROM dbo.DomainInformationXUser dixu
    GROUP BY dixu.IdUser
    ) dkim ON dkim.IdUser = u.IdUser
LEFT JOIN (
    SELECT cd.IdUser
        ,COUNT(CASE
                WHEN IdDomainVerificationStatus = 1
                    THEN 1
                WHEN IdDomainVerificationStatus = 3
                    THEN 1
                END) alertCounter
        ,COUNT(CASE
                WHEN IdDomainVerificationStatus = 2
                    THEN 1
                END) ConnectedCounter
    FROM dbo.CustomDomain cd
    GROUP BY cd.IdUser
    ) CustomDomain ON CustomDomain.IdUser = u.IdUser
WHERE u.Email = @Email",
                    new { Email = email });
                return results.FirstOrDefault();
            }
        }
    }
}
