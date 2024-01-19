using CommonAbstraction.Repository;
using CommonLogic.BusinessLogic;
using DataModels.DatabaseModels.Webpage;
using DataModels.StorageModels.Auth;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.ActionFilters
{
    public class AuthActionFilterAttribute : ActionFilterAttribute
    {
        private UserSecurityPass.PassRole _role;

        public AuthActionFilterAttribute() : this(UserSecurityPass.PassRole.Unauthorized)
        {
            
        }

        public AuthActionFilterAttribute(UserSecurityPass.PassRole role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var contextSecurityData = AuthLogic.GetContextSecurityData(context.HttpContext);

            IGenericRepo<User> usersRepo = context.HttpContext.RequestServices.GetService<IGenericRepo<User>>();
            IGenericRepo<Webpage> webpageRepo = context.HttpContext.RequestServices.GetService<IGenericRepo<Webpage>>();
            IGenericRepo<AuthenticationToken> tokenRepo = context.HttpContext.RequestServices.GetService<IGenericRepo<AuthenticationToken>>();

            UserSecurityPass securityPass = AuthLogic.GenerateUserSecurityPass(webpageRepo, usersRepo, tokenRepo, contextSecurityData);

            if ( securityPass.Role >= _role )
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult("Unauthorized!");//new EmptyResult();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
