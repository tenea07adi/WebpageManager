using CommonAbstraction.DataModels;

namespace DataModels.DatabaseModels.WebpageSimpleInfo
{
    public class WebpageInfo : BaseWebpageOwnStorageModel
    {
        public int CollectionId { get; set; }
        public string Info { get; set; }

        public bool HaveStartMoment { get; set; }
        public DateTime StartMoment { get; set; }

        public bool HaveExpirationMoment { get; set; }
        public DateTime ExpirationMoment { get; set; }
    }
}
