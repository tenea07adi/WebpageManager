using API.ActionFilters;
using CommonAbstraction.Repository;
using CommonLogic.BusinessLogic;
using CommonLogic.DataModelsMapper;
using CommonLogic.Helpers;
using DataModels.DatabaseModels.Webpage;
using DataModels.DataTransferObjects.Auth;
using DataModels.StorageModels.Auth;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGenericRepo<User> _userRepo;
        private readonly IGenericRepo<AuthenticationToken> _authTokenRepo;
        private readonly IGenericRepo<RegistrationInvite> _registrationInviteRepo;


        public AuthController(IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> authTokenRepo, IGenericRepo<RegistrationInvite> registrationInviteRepo)
        {
            _authTokenRepo = authTokenRepo;
            _userRepo = userRepo;
            _registrationInviteRepo = registrationInviteRepo;
        }

        [AuthActionFilter(UserSecurityPass.PassRole.Admin)]
        [HttpPost("User/Invite")]
        public IActionResult CreateUserInvite(RegistrationInviteUpsertDTO inviteRequest)
        {
            if (_userRepo.Get().Where(c => c.Email == inviteRequest.Email).FirstOrDefault() != null)
            {
                return BadRequest("User already exist!");
            }

            UserSecurityPass securityPass = GenerateUserSecurityPass();

            RegistrationInvite registrationInvite = new RegistrationInvite();

            registrationInvite.Email = inviteRequest.Email;
            registrationInvite.Message = inviteRequest.Message;
            registrationInvite.SenderId = securityPass.User.Id;
            registrationInvite.ExpirationMoment = DateTime.Now.AddDays(7);
            registrationInvite.RegistrationKey = "";

            // generate keys until it is uniqe
            while (registrationInvite.RegistrationKey == "")
            {
                registrationInvite.RegistrationKey = CryptographyHelper.GenerateRandomString(6);

                if(_registrationInviteRepo.Get().Where(c => c.RegistrationKey == registrationInvite.RegistrationKey).ToList().Count() > 0)
                {
                    registrationInvite.RegistrationKey = "";
                }
            }

            _registrationInviteRepo.Add(registrationInvite);

            RegistrationInviteResponseDTO responseDTO = new RegistrationInviteResponseDTO();

            DataModelsMapper.Mapp(registrationInvite, responseDTO);

            return Ok(responseDTO);
        }

        [HttpPost("User/Register")]
        public IActionResult Register(UserRegistrationDTO registrationData)
        {
            RegistrationInvite dbInvite = _registrationInviteRepo.Get().Where(c => c.RegistrationKey == registrationData.RegistrationKey).FirstOrDefault();
            
            if (dbInvite == null)
            {
                return BadRequest("Invite not found!");
            }

            if(dbInvite.ExpirationMoment < DateTime.Now)
            {
                return BadRequest("Invite Expired!");
            }

            if (_userRepo.Get().Where(c => c.Email == dbInvite.Email).FirstOrDefault() != null)
            {
                return BadRequest("User already exist!");
            }

            User newUser = new User();

            newUser.Email = dbInvite.Email;
            newUser.DisplayedName = registrationData.DisplayedName;
            newUser.Role = DataModels.StorageModels.Auth.User.UserRole.User;
            newUser.PasswordSalt = CryptographyHelper.GeneratePasswordSalt();
            newUser.PasswordHash = CryptographyHelper.HashPassword(registrationData.Password, newUser.PasswordSalt);

            _userRepo.Add(newUser);

            return Ok("User registered!");
        }

        [HttpPost("User/Login")]
        public IActionResult LogIn(LogInDTO logInData)
        {
            User user = _userRepo.Get().Where(c=>c.Email == logInData.Email).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }

            if (CryptographyHelper.IsCorrectPassword(logInData.Password, user.PasswordHash, user.PasswordSalt))
            {
                AuthenticationToken newAuthenticationToken = AuthLogic.GenerateAuthenticationToken(user.Id);

                _authTokenRepo.Add(newAuthenticationToken);

                LogInResponseDTO resp = new LogInResponseDTO();

                DataModelsMapper.Mapp(newAuthenticationToken, resp);

                return Ok(resp);
            }

            return BadRequest();
        }

        [HttpPost("AuthenticationToken/Renew")]
        public IActionResult AuthenticationTokenRenew(string authenticationToken)
        {
            return BadRequest("Not implemented yet!");
        }

        [HttpPost("AuthenticationToken/Check")]
        public IActionResult AuthenticationTokenCheck(string authenticationToken)
        {
            return BadRequest("Not implemented yet!");
        }

        private UserSecurityPass GenerateUserSecurityPass()
        {
            IGenericRepo<Webpage> webpageRepo = HttpContext.RequestServices.GetService<IGenericRepo<Webpage>>();

            return AuthLogic.GenerateUserSecurityPass(webpageRepo, _userRepo, _authTokenRepo, AuthLogic.GetContextSecurityData(HttpContext));
        }
    }
}
