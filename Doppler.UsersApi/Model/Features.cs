using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.UsersApi.Model
{
    public class Features
    {
        public bool ContactPolicies { get; set; }

        public bool BigQuery { get; set; }

        public bool SmartCampaigns { get; set; }

        public bool SmartCampaingsExtraCustomizations { get; set; }

        public bool SmartSubjectCampaigns { get; set; }
    }
}
