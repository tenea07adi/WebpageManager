using CommonAbstraction.DataModels;

namespace DataModels.DatabaseModels.WebpageSimpleInfo
{
    public class WebpageInfoCollection : BaseStorageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
