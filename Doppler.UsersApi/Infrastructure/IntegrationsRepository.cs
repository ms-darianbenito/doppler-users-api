using Doppler.UsersApi.Model;
using System.Data;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Doppler.UsersApi.Infrastructure
{
    public class IntegrationsRepository : IIntegrationsRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        public IntegrationsRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Dictionary<string, string>> GetIntegrationsStatusByUserAccount(string email)
        {
            using (IDbConnection connection = await _connectionFactory.GetConnection())
            {
                var results = await connection.QueryAsync<IntegrationStatus>(@"
SELECT Name + 'Status' [Integration]
    ,CASE StatusValues.[Status]
        WHEN 1 THEN 'alert'
        WHEN 0 THEN 'connected'
        ELSE 'disconnected'
    END [Status]
FROM dbo.ThirdPartyApp tpa
LEFT JOIN (
    SELECT tpa.Name [Integration]
        ,CASE
            WHEN tpaxu.ConnectionErrors > 1
                THEN 1
            ELSE 0
            END [Status]
    FROM dbo.ThirdPartyAppXUser tpaxu
    JOIN dbo.[user] u ON u.iduser = tpaxu.iduser
    LEFT JOIN dbo.ThirdPartyApp tpa ON tpa.IdThirdPartyApp = tpaxu.IdThirdPartyApp
    WHERE u.Email = @Email
    ) StatusValues ON StatusValues.Integration = tpa.Name

UNION

SELECT 'DkimStatus' [Integration]
    ,CASE
        WHEN dkim.AlertCounter > 0
            THEN 'alert'
        WHEN dkim.ConnectedCounter > 0
            THEN 'connected'
        ELSE 'disconnected'
    END [Status]
FROM (
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
FROM dbo.[User] u
LEFT JOIN dbo.DomainInformationXUser dixu ON dixu.IdUser = u.IdUser
WHERE u.Email = @email
GROUP BY dixu.IdUser
) dkim

UNION

SELECT 'CustomDomainStatus' [Integration]
    ,CASE
        WHEN IdDomainVerificationStatus= 1
            THEN 'alert'
        WHEN IdDomainVerificationStatus=3
            THEN 'alert'
        WHEN IdDomainVerificationStatus=2
            THEN 'connected'
        WHEN IdDomainVerificationStatus IS NULL
            THEN 'disconnected'
        END [Status]
FROM dbo.[User] u
LEFT JOIN dbo.CustomDomain cd ON u.IdUser = cd.IdUser
WHERE u.Email = @Email

UNION

SELECT 'BigQueryStatus' [Integration]
    ,CASE
        WHEN
            SUM(CASE
                WHEN ua.email IS NOT NULL
                    THEN 1
                ELSE 0
            END) > 0 THEN 'connected'
        ELSE 'disconnected'
    END[Status]
FROM dbo.[User] u
LEFT JOIN datastudio.UserAccessByUser ua ON u.IdUser = ua.IdUser AND ua.validTo > GETUTCDATE()
WHERE u.Email = @Email
GROUP BY u.iduser

UNION

SELECT 'ApiKeyStatus' [Integration]
    ,CASE
        WHEN u.ApiKey IS NULL
            THEN 'disconnected'
        ELSE 'connected'
        END [Status]
FROM dbo.[user] u
WHERE u.Email = @Email

UNION

SELECT 'GoogleAnaliyticStatus' [Integration]
    ,CASE
        WHEN u.EnableGoogleAnalytic = 1
            THEN 'connected'
        ELSE 'disconnected'
        END [Status]
FROM dbo.[user] u
WHERE u.Email = @Email",
                    new { Email = email });

                return results.ToDictionary(
                    key => key.Integration.Replace(" ", ""),
                    Value => string.IsNullOrEmpty(Value.Status) ? "disconnected" : Value.Status);
            }
        }
    }
}
