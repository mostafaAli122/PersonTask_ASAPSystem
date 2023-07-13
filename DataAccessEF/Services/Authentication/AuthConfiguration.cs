using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Services.Authentication
{
    public class AuthConfiguration
    {
        public string JwtValidAudience { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtSecretKey { get; set; }
        public string JwtEncryptionKey { get; set; }
        public int JwtExpireInHours { get; set; }
        public int LoginNoOfFailedTrials { get; set; }
        public long? InsuranceCompanyId { get; set; }
    }
}
