using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IIntegrationsRepository
    {
        Task<Dictionary<string, string>> GetIntegrationsStatusByUserAccount(string email);
    }
}
