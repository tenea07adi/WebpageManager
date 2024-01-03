
namespace DataModels.DataTransferObjects.Auth
{
    public class LogInResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreashToken { get; set; }
        public DateTime AccessTokenExpirationMoment { get; set; }
        public DateTime RefreashTokenExpirationMoment { get; set; }
    }
}
