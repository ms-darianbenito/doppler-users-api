using Doppler.UsersApi.DopplerSecurity;
using Doppler.UsersApi.Infrastructure;
using Doppler.UsersApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Controllers
{
    [Authorize]
    [ApiController]
    public class AccountController
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountController(ILogger<FeatureController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [HttpGet("/accounts/{accountName}/contact-information")]
        public async Task<IActionResult> GetContactInformation(string accountName)
        {
            var contactInformation = await _accountRepository.GetContactInformation(accountName);

            if (contactInformation == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(contactInformation);
        }

        [HttpPut("/accounts/{accountName}/contact-information")]
        public async Task<IActionResult> UpdateContactInformation(string accountName, [FromBody] ContactInformation contactInformation)
        {
            _logger.LogDebug("Updating Contact Information.");

            await _accountRepository.UpdateContactInformation(accountName, contactInformation);

            return new OkObjectResult("Successfully");
        }
    }
}
