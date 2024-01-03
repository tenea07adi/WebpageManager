using CommonAbstraction.DataModels;

namespace DataModels.StorageModels.Auth
{
    public class RegistrationInvite : BaseStorageModel
    {
        public int SenderId { get; set; }
        public string RegistrationKey { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime ExpirationMoment { get; set; }
    }
}
