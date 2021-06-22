using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Doppler.UsersApi.DopplerSecurity
{
    public class DopplerSecurityOptions
    {
        public IEnumerable<SecurityKey> SigningKeys { get; set; } = new SecurityKey[0];
    }
}
