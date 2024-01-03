using CommonAbstraction.DataModels;

namespace CommonAbstraction.Repository
{
    public interface IGenericRepo<T> where T : BaseStorageModel
    {
        public T Get(int id);
        public List<T> Get();
        public void Add(T t);
        public void Update(T t);
        public void Delete(int id);
    }
}
