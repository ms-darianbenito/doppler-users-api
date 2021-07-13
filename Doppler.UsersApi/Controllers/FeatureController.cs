using System.Threading.Tasks;
using Doppler.UsersApi.DopplerSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using Doppler.UsersApi.Infrastructure;

namespace Doppler.UsersApi.Controllers
{
    [Authorize]
    [ApiController]
    public class FeatureController
    {
        private readonly ILogger _logger;
        private readonly IFeaturesRepository _featuresRepository;

        public FeatureController(ILogger<FeatureController> logger, IFeaturesRepository featuresRepository)
        {
            _logger = logger;
            _featuresRepository = featuresRepository;
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/features")]
        public async Task<IActionResult> GetFeaturesForAccountByEmail(string accountName)
        {
            var features = await _featuresRepository.GetFeaturesByUserAccount(accountName);

            if (features == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(features);
        }
    }
}
