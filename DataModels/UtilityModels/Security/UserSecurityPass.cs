using DataModels.DatabaseModels.Webpage;
using DataModels.StorageModels.Auth;

namespace DataModels.UtilityModels.Security
{
    public class UserSecurityPass
    {
        public enum PassRole  { Unauthorized, Webpage, User, Admin }

        public PassRole Role 
        { 
            get 
            { 
                if(User != null)
                {
                    if(User.Role == User.UserRole.Admin)
                    {
                        return PassRole.Admin;
                    }

                    return PassRole.User;
                }

                if(WebPage != null)
                {
                    return PassRole.Webpage;
                }

                return PassRole.Unauthorized; 
            } 
        }

        public User User { get; set; }

        public Webpage WebPage { get; set; }

    }
}
