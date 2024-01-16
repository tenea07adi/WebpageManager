using CommonAbstraction.DataModels;

namespace DataModels.DatabaseModels.WebpageSimpleInfo
{
    public class WebpageInfoCollection : BaseWebpageOwnStorageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
