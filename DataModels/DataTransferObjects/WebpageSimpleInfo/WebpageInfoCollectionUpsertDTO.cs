using CommonAbstraction.DataModels;

namespace DataModels.DataTransferObjects.WebpageSimpleInfo
{
    public class WebpageInfoCollectionUpsertDTO : BaseUpsertDTO
    {
        public int WebpageId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
