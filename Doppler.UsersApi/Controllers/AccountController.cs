using Doppler.UsersApi.DopplerSecurity;
using Doppler.UsersApi.Infrastructure;
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

        [HttpGet("/accounts/{email}/contactinformation")]
        public async Task<IActionResult> GetContactInformation(string email)
        {
            var contactInformation = await _accountRepository.GetContactInformation(email);

            if (contactInformation == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(contactInformation);
        }
    }
}
