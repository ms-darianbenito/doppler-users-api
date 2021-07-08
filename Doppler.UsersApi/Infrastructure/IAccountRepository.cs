using Doppler.UsersApi.Model;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IAccountRepository
    {
        Task<ContactInformation> GetContactInformation(string accountName);
    }
}
