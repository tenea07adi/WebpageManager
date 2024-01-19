using CommonAbstraction.DataModels;
using CommonAbstraction.Repository;
using DatabaseLayer.DatabaseConnection;

namespace DataAccessLayer.DbRepository.Generics
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseStorageModel, new()
    {
        protected readonly DataBaseContext _dbContext;

        public GenericRepo(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T t)
        {
            t.CreatedAt = DateTime.Now;
            t.UpdatedAt = DateTime.Now;

            _dbContext.Set<T>().Add(t);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbContext.Set<T>().Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            return _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public List<T> Get()
        {
            return _dbContext.Set<T>().ToList();
        }

        public void Update(T t)
        {
            t.UpdatedAt = DateTime.Now;

            _dbContext.Set<T>().Update(t);
            _dbContext.SaveChanges();
        }

    }
}
