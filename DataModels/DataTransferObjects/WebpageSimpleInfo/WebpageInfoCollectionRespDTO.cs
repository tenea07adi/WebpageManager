using CommonAbstraction.DataModels;

namespace DataModels.DataTransferObjects.WebpageSimpleInfo
{
    public class WebpageInfoCollectionRespDTO : BaseResponseDTO
    {
        public int UserId { get; set; }
        public int WebpageId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
