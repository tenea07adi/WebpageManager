
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
        public static AuthenticationToken GenerateAuthenticationToken()
        {
            AuthenticationToken authenticationToken = new AuthenticationToken()
            {
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

        public static UserSecurityPass GenerateUserSecurityPass(IGenericRepo<User> userRepo, IGenericRepo<Webpage> webpageRepo, IGenericRepo<AuthenticationToken> tokenRepo, ContextSecurityData contextSecurityData)
        {
            User user = GetUserByAuthenticationToken(userRepo, tokenRepo, contextSecurityData.AuthenticationToken);
            Webpage webpage = GetWebpageByDomain(webpageRepo, contextSecurityData.RequesterHttpDomain);

            if(webpage == null && user == null)
            {
                return null;
            }

            UserSecurityPass userSecurityPass = new UserSecurityPass()
            {
                User = user,
                WebPage = webpage
            };

            return userSecurityPass;
        }

        public static User GetUserByAuthenticationToken(IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> tokenRepo, string token)
        {
            int userId = tokenRepo.Get().Where(c => c.AccessToken == token).FirstOrDefault().UserId;

            if(userId == null) 
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
    
    }
}
