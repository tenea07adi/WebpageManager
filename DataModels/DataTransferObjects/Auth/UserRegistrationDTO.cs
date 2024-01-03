
namespace DataModels.DataTransferObjects.Auth
{
    public class UserRegistrationDTO
    {
        public string RegistrationKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayedName { get; set; }
    }
}
