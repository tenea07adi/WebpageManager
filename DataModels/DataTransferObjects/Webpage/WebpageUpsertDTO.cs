using CommonAbstraction.DataModels;

namespace DataModels.DataTransferObjects.Webpage
{
    public class WebpageUpsertDTO : BaseUpsertDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string WebDomain { get; set; }
    }
}
