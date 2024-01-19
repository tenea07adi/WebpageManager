using CommonAbstraction.DataModels;

namespace DataModels.UtilityModels.Security
{
    public class ContextSecurityData : BaseUtilityModel
    {
        public string AuthenticationToken { get; set; }
        public string RequesterHttpDomain { get; set; }
        public string HttpRequestType { get; set; }

    }
}
