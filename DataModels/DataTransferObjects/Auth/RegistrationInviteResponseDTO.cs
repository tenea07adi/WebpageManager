
namespace DataModels.DataTransferObjects.Auth
{
    public class RegistrationInviteResponseDTO
    {
        public int SenderId { get; set; }
        public string RegistrationKey { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime ExpirationMoment { get; set; }
    }
}
