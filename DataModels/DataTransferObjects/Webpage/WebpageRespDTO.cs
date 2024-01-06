using CommonAbstraction.DataModels;

namespace DataModels.DataTransferObjects.Webpage
{
    public class WebpageRespDTO : BaseResponseDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string WebDomain { get; set; }
    }
}
