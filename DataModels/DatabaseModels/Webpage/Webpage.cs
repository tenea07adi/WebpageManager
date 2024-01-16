using CommonAbstraction.DataModels;

namespace DataModels.DatabaseModels.Webpage
{
    public class Webpage : BaseProtectedStorageModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string WebDomain { get; set; }
    }
}
