using CommonAbstraction.Repository;
using CommonLogic.BusinessLogic;
using CommonLogic.DataModelsMapper;
using CommonLogic.Helpers;
using DataModels.DataTransferObjects.Auth;
using DataModels.StorageModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGenericRepo<User> _userRepo;
        private readonly IGenericRepo<AuthenticationToken> _authTokenRepo;

        public AuthController(IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> authTokenRepo)
        {
            _authTokenRepo = authTokenRepo;
            _userRepo = userRepo;
        }

        [HttpGet("Utility/Init")]
        public IActionResult InitDefaultUser()
        {
            string defaultUserEmail = "admin@aditenea.net";
            string defaultUserName = "admin";

            string randomGeneratedPassword = CryptographyHelper.GenerateRandomString(6);

            if (_userRepo.Get().Where(c => c.Email == defaultUserEmail).Any())
            {
                return BadRequest("User already initilized!");
            }

            User defaultUser = new User();
            defaultUser.Email = defaultUserEmail;
            defaultUser.DisplayedName = defaultUserName;
            defaultUser.PasswordSalt = CryptographyHelper.GeneratePasswordSalt();
            defaultUser.PasswordHash = CryptographyHelper.HashPassword(randomGeneratedPassword, defaultUser.PasswordSalt);

            _userRepo.Add(defaultUser);

            return Ok($"Success! Username: {defaultUser.Email}; Password: {randomGeneratedPassword}");
        }

        [HttpGet("Utility/Cleanup")]
        public IActionResult CleanUp()
        {
            int deletedCount = 0;
            int totalCount = 0;

            try
            {
                List<AuthenticationToken> authenticationTokens = _authTokenRepo.Get().Where(c => c.AccessTokenExpirationMoment < DateTime.Now).ToList();

                totalCount = authenticationTokens.Count;

                foreach (var authenticationToken in authenticationTokens)
                {
                    _authTokenRepo.Delete(authenticationToken.Id);

                    deletedCount++;
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Operation finished with error! Records found: {totalCount}, Records deleted: {deletedCount}, Error message: {ex.Message}");
            }

            return Ok($"Records found: {totalCount}, Records deleted: {deletedCount}");
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

        [HttpPost("User/Register")]
        public IActionResult Register(UserRegistrationDTO registrationData)
        {
            return BadRequest("Not implemented yet!");
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

    }
}
