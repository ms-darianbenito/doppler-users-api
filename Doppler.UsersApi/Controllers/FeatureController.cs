using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.UsersApi.DopplerSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Doppler.UsersApi.Controllers
{
    [Authorize]
    [ApiController]
    public class FeatureController
    {
        [AllowAnonymous]
        [HttpGet("/features")]
        public string GetFeatures()
        {
            return "This method will return a list of features";
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("accounts/{accountId:int:min(0)}/features")]
        public string GetFeaturesForAccountById(int accountId)
        {
            return $"Will return the list of features associated to the Account ID '{accountId}'";
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountname}/features")]
        public string GetFeaturesForAccountByEmail(string accountEmail)
        {
            return $"Will return the list of features associated to the Account email '{accountEmail}'";
        }
    }
}
