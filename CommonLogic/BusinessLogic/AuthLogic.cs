
using CommonAbstraction.Repository;
using CommonLogic.Helpers;
using DataModels.DatabaseModels.Webpage;
using DataModels.StorageModels.Auth;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Http;

namespace CommonLogic.BusinessLogic
{
    public static class AuthLogic
    {
        public static AuthenticationToken GenerateAuthenticationToken(int userId)
        {
            AuthenticationToken authenticationToken = new AuthenticationToken()
            {
                UserId = userId,
                AccessToken = CryptographyHelper.GenerateRandomString(10),
                RefreashToken = CryptographyHelper.GenerateRandomString(10),
                AccessTokenExpirationMoment = DateTime.Now.AddDays(1),
                RefreashTokenExpirationMoment = DateTime.Now.AddDays(7)
            };

            return authenticationToken;
        }

        public static ContextSecurityData GetContextSecurityData(HttpContext httpContext) 
        {
            ContextSecurityData securityData = new ContextSecurityData();

            securityData.RequesterHttpDomain = httpContext.Request.Headers["Referer"].ToString();
            securityData.AuthenticationToken = httpContext.Request.Headers["AccessToken"].ToString();
            securityData.HttpRequestType = httpContext.Request.Protocol;

            return securityData;
        }

        public static UserSecurityPass GenerateUserSecurityPass(IGenericRepo<Webpage> webpageRepo, IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> tokenRepo, ContextSecurityData contextSecurityData)
        {
            User user = GetUserByAuthenticationToken(userRepo, tokenRepo, contextSecurityData.AuthenticationToken);
            Webpage webpage = GetWebpageByDomain(webpageRepo, contextSecurityData.RequesterHttpDomain);

            UserSecurityPass userSecurityPass = new UserSecurityPass()
            {
                User = user,
                WebPage = webpage
            };

            return userSecurityPass;
        }

        public static User GetUserByAuthenticationToken(IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> tokenRepo, string token)
        {

            AuthenticationToken t = tokenRepo.Get().Where(c => c.AccessToken == token).FirstOrDefault();

            if (!IsValidAuthenticationToken(t))
            {
                return null;
            }

            int userId = t.UserId;

            if (userId == 0) 
            {
                return null;
            }

            var user = userRepo.Get().Where(c => c.Id == userId).FirstOrDefault();

            if(user == null)
            {
                return null;
            }

            return user;
        }

        public static Webpage GetWebpageByDomain(IGenericRepo<Webpage> webpageRepo, string domain)
        {
            Webpage webpage = webpageRepo.Get().Where(c => c.WebDomain == domain).FirstOrDefault();

            if(webpage == null)
            {
                return null;
            }

            return webpage;
        }
    
        public static bool IsValidAuthenticationToken(AuthenticationToken token)
        {
            if(token == null)
            {
                return false;
            }

            if(token.AccessToken == null || token.AccessToken == String.Empty) 
            {
                return false;
            }

            if(token.AccessTokenExpirationMoment <= DateTime.Now) 
            {
                return false;
            }

            return true;
        }
    }
}
