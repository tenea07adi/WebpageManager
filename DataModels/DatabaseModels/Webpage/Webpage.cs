using CommonAbstraction.DataModels;

namespace DataModels.DatabaseModels.Webpage
{
    public class Webpage : BaseStorageModel
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string WebDomain { get; set; }
    }
}
