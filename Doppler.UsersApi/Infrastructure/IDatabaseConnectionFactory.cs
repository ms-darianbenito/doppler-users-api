using System.Data;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> GetConnection();
    }
}
