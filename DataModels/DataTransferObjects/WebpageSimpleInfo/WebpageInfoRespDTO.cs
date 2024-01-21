
using CommonAbstraction.DataModels;

namespace DataModels.DataTransferObjects.WebpageSimpleInfo
{
    public class WebpageInfoRespDTO : BaseResponseDTO
    {
        public int UserId { get; set; }
        public int WebpageId { get; set; }

        public int CollectionId { get; set; }
        public string Info { get; set; }

        public bool HaveStartMoment { get; set; }
        public DateTime StartMoment { get; set; }

        public bool HaveExpirationMoment { get; set; }
        public DateTime ExpirationMoment { get; set; }
    }
}
