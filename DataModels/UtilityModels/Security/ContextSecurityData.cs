using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.UtilityModels.Security
{
    public class ContextSecurityData
    {
        public string AuthenticationToken { get; set; }
        public string RequesterHttpDomain { get; set; }
        public string HttpRequestType { get; set; }

    }
}
