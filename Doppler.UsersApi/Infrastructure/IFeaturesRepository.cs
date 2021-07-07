using Doppler.UsersApi.Model;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IFeaturesRepository
    {
        Task<Features> GetFeaturesByUserAccount(string email);
    }
}
