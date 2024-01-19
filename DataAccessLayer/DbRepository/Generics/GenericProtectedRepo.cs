using CommonAbstraction.DataModels;
using CommonAbstraction.Repository;
using DatabaseLayer.DatabaseConnection;
using DataModels.UtilityModels.Security;
using System.Reflection;

namespace DataAccessLayer.DbRepository.Generics
{
    public class GenericProtectedRepo<T, TSecurityPass> : GenericRepo<T>, IGenericProtectedRepo<T, TSecurityPass>
        where T : BaseProtectedStorageModel, new()
        where TSecurityPass : UserSecurityPass
    {
        public GenericProtectedRepo(DataBaseContext dbContext) : base(dbContext)
        {
        }

        public void Add(T t, TSecurityPass userSecurityPass)
        {
            try
            {
                SetAdminId(t, userSecurityPass);
                DoWithProtected(t, userSecurityPass, null, c => { Add(c); });
            }
            catch 
            {
                return;
            }
        }

        public void Delete(int id, TSecurityPass userSecurityPass)
        {
            DoWithProtected(Get(id), userSecurityPass, null, c => { Delete(c.Id); });
        }

        public T Get(int id, TSecurityPass userSecurityPass)
        {
            return GetProtected(1, userSecurityPass, c => c.Id == id).FirstOrDefault();
        }

        public List<T> Get(TSecurityPass userSecurityPass)
        {
            return GetProtected(100, userSecurityPass, null);
        }

        public void Update(T t, TSecurityPass userSecurityPass)
        {
            DoWithProtected(t, userSecurityPass, null, c => { Update(c); });
        }

        #region Utility
        private List<T> GetProtected(int numberOfRecords, UserSecurityPass userSecurityPass, Func<T, bool> aditionalCheck)
        {
            if (aditionalCheck == null)
            {
                var newT = new T();

                aditionalCheck = (newT) => { return true; };
            }

            if (!IsProtectedType() &&  userSecurityPass.Role >= UserSecurityPass.PassRole.Admin)
            {
                return Get().Take(numberOfRecords).ToList();
            }

            if (userSecurityPass.Role >= UserSecurityPass.PassRole.Webpage)
            {
                if (userSecurityPass.User != null)
                {
                    return _dbContext.Set<T>().AsEnumerable().Where(record => UserOwnThisRecord(record, userSecurityPass) && aditionalCheck(record)).Take(numberOfRecords).ToList();
                }

                if (userSecurityPass.WebPage != null)
                {
                    return _dbContext.Set<T>().AsEnumerable().Where(record => WebsiteOwnThisRecord(record, userSecurityPass) && aditionalCheck(record)).Take(numberOfRecords).ToList();
                }
            }

            return new List<T>();
        }

        private void DoWithProtected(T t, UserSecurityPass userSecurityPass, Func<T, bool> aditionalCheck, Action<T> action)
        {
            if(userSecurityPass == null)
            {
                return;
            }

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

            if (aditionalCheck != null && !aditionalCheck(t))
            {
                return;
            }

            action?.Invoke(t);
        }

        private void SetAdminId(T t, UserSecurityPass userSecurityPass)
        {
            if (IsProtectedType())
            {
                if (userSecurityPass.User != null)
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
