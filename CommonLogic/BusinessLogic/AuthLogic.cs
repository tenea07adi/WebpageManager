
using CommonAbstraction.Repository;
using CommonLogic.Helpers;
using DataModels.DataTransferObjects.Auth;
using DataModels.StorageModels.Auth;

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
    }
}
