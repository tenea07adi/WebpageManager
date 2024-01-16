using CommonAbstraction.DataModels;

namespace DataModels.StorageModels.Auth
{
    public class AuthenticationToken : BaseStorageModel
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreashToken { get; set; }
        public DateTime AccessTokenExpirationMoment { get; set; }
        public DateTime RefreashTokenExpirationMoment { get; set; }
    }
}
