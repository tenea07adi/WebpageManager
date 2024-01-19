using CommonAbstraction.DataModels;

namespace CommonAbstraction.Repository
{
    public interface IGenericProtectedRepo<T, TSecurityPass> : IGenericRepo<T>
        where T : BaseStorageModel
        where TSecurityPass : BaseUtilityModel
    {
        public T Get(int id, TSecurityPass userSecurityPass);

        public List<T> Get(TSecurityPass userSecurityPass);

        void Add(T t, TSecurityPass userSecurityPass);

        public void Update(T t, TSecurityPass userSecurityPass);

        public void Delete(int id, TSecurityPass userSecurityPass);
    }
}
