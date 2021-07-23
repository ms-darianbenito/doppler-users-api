using Doppler.UsersApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Infrastructure
{
    public interface IAccountRepository
    {
        Task<ContactInformation> GetContactInformation(string accountName);
        Task UpdateContactInformation(string accountName, ContactInformation contactInformation);
    }
}
