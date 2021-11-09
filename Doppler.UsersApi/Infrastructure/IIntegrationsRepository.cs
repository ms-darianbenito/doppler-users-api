using Doppler.UsersApi.Model;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IIntegrationsRepository
    {
        Task<Integrations> GetIntegrationsByUserAccount(string email);
    }
}
