using System.Threading.Tasks;
using Doppler.UsersApi.DopplerSecurity;
using Doppler.UsersApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Doppler.UsersApi.Controllers
{
    [Authorize]
    [ApiController]
    public class IntegrationController
    {
        private readonly ILogger _logger;
        private readonly IIntegrationsRepository _integrationsRepository;

        public IntegrationController(ILogger<FeatureController> logger, IIntegrationsRepository integrationsRepository)
        {
            _logger = logger;
            _integrationsRepository = integrationsRepository;
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/integrations")]
        public async Task<IActionResult> GetIntegrationsConnectionsByEmail(string accountName)
        {
            var integrations = await _integrationsRepository.GetIntegrationsByUserAccount(accountName);

            if (integrations == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(integrations);
        }
    }
}
