using CommonAbstraction.DataModels;
using CommonAbstraction.Repository;
using DatabaseLayer.DatabaseConnection;
using DataModels.StorageModels.Auth;
using DataModels.UtilityModels.Security;
using System.Linq;
using System.Reflection;

namespace DataAccessLayer.DbRepository.Generics
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseStorageModel, new()
    {
        private readonly DataBaseContext _dbContext;

        public GenericRepo(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T t, UserSecurityPass userSecurityPass)
        {
            DoWithProtected(t, userSecurityPass, null, c => { SetAdminId(c, userSecurityPass); Add(c); });
        }

        public void Add(T t)
        {
            t.CreatedAt = DateTime.Now;
            t.UpdatedAt = DateTime.Now;

            _dbContext.Set<T>().Add(t);
            _dbContext.SaveChanges();
        }

        public void Delete(int id, UserSecurityPass userSecurityPass)
        {
            DoWithProtected(Get(id), userSecurityPass, null, c => { Delete(c.Id); });
        }

        public void Delete(int id)
        {
            _dbContext.Set<T>().Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public T Get(int id, UserSecurityPass userSecurityPass)
        {
            return GetProtected(1, userSecurityPass, c => c.Id == id).FirstOrDefault();
        }

        public T Get(int id)
        {
            return _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public List<T> Get(UserSecurityPass userSecurityPass)
        {
            return GetProtected(100, userSecurityPass, null);
        }

        public List<T> Get()
        {
            return _dbContext.Set<T>().ToList();
        }

        public void Update(T t, UserSecurityPass userSecurityPass)
        {
            DoWithProtected(t, userSecurityPass, null, c => { Update(c); });
        }

        public void Update(T t)
        {
            t.UpdatedAt = DateTime.Now;

            _dbContext.Set<T>().Update(t);
            _dbContext.SaveChanges();
        }

        #region Utility
        private List<T> GetProtected(int numberOfRecords, UserSecurityPass userSecurityPass, Func<T, bool> aditionalCheck)
        {
            if(aditionalCheck == null)
            {
                var newT = new T();

                aditionalCheck = (newT) => { return true; };
            }

            if (!IsProtectedType() && userSecurityPass.Role >= UserSecurityPass.PassRole.Admin)
            {
                return Get().Take(numberOfRecords).ToList();
            }

            if (userSecurityPass.Role >= UserSecurityPass.PassRole.Webpage)
            {
                if (userSecurityPass.User != null)
                {
                    return _dbContext.Set<T>().Where(record => UserOwnThisRecord(record, userSecurityPass) && aditionalCheck(record)).Take(numberOfRecords).ToList();
                }

                if (userSecurityPass.WebPage != null)
                {
                    return _dbContext.Set<T>().Where(record => WebsiteOwnThisRecord(record, userSecurityPass) && aditionalCheck(record)).Take(numberOfRecords).ToList();
                }
            }

            return new List<T>();
        }

        private void DoWithProtected(T t, UserSecurityPass userSecurityPass, Func<T, bool> aditionalCheck, Action<T> action)
        {
            if (userSecurityPass.Role < UserSecurityPass.PassRole.User)
            {
                return;
            }

            if (!IsProtectedType() && userSecurityPass.Role != UserSecurityPass.PassRole.Admin)
            {
                return;
            }

            if (IsProtectedType() && userSecurityPass.Role == UserSecurityPass.PassRole.User && !UserOwnThisRecord(t, userSecurityPass))
            {
                return;
            }

            if (!aditionalCheck(t))
            {
                return;
            }

            action.Invoke(t);
        }

        private void SetAdminId(T t, UserSecurityPass userSecurityPass)
        {
            if (IsProtectedType())
            {
                if (userSecurityPass.User != null && userSecurityPass.User.Id == (Int32)t.GetType().GetProperties().SingleOrDefault(c => c.Name == "UserId").GetValue(t))
                {
                    SetAdminId(t, userSecurityPass.User.Id);
                }
            }
        }

        private void SetAdminId(T t, int userId)
        {
            PropertyInfo destinationProp = typeof(T).GetProperty("UserId");

            if (destinationProp != null)
            {
                destinationProp.SetValue(t, userId);
            }
        }

        private bool UserOwnThisRecord(T t, UserSecurityPass userSecurityPass)
        {
            return userSecurityPass.User != null && userSecurityPass.User.Id == (Int32)t.GetType().GetProperties().SingleOrDefault(c => c.Name == "UserId").GetValue(t);
        }

        private bool WebsiteOwnThisRecord(T t, UserSecurityPass userSecurityPass)
        {
            return userSecurityPass.WebPage.Id == (Int32)t.GetType().GetProperties().SingleOrDefault(c => c.Name == "WebpageId").GetValue(t);
        }

        private bool IsProtectedType()
        {
            return IsUserProtectedType() || IsWebpageOwnType();
        }

        private bool IsUserProtectedType()
        {
            return typeof(T).BaseType.Equals(typeof(BaseProtectedStorageModel));
        }

        private bool IsWebpageOwnType()
        {
            return typeof(T).BaseType.Equals(typeof(BaseWebpageOwnStorageModel));
        }

        #endregion
    }
}
