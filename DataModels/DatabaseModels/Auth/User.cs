using CommonAbstraction.DataModels;

namespace DataModels.StorageModels.Auth
{
    public class User : BaseStorageModel
    {
        public enum UserRole { User, Admin }
        public UserRole Role { get; set; }
        public string DisplayedName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

    }
}
