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
        [HttpGet("/accounts/{accountname}/features")]
        public async Task<IActionResult> GetFeaturesForAccountByEmail(string accountEmail)
        {
            //TODO: validate accountEmail parameter?

            var features = await _featuresRepository.GetFeaturesByUserAccount(accountEmail);

            if (features == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(features);
        }
    }
}
