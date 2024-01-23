using CommonAbstraction.Repository;
using CommonLogic.Helpers;
using DatabaseLayer.DatabaseConnection;
using DataModels.StorageModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IGenericRepo<User> _userRepo;
        private readonly IGenericRepo<AuthenticationToken> _authTokenRepo;

        public ManagementController(DataBaseContext dataBaseContext, IGenericRepo<User> userRepo, IGenericRepo<AuthenticationToken> authTokenRepo)
        {
            _dataBaseContext = dataBaseContext;
            _userRepo = userRepo;
            _authTokenRepo = authTokenRepo;
        }

        [HttpGet("Init")]
        public IActionResult Init()
        {
            string response = "";

            try
            {
                UpdateDatabase();
                response += "Database init success! \n";
            }
            catch (Exception ex)
            {
                response += "Error while database init! Error message: \n" + ex.Message;
                return BadRequest(response);
            }

            try
            {
                response += "Default user init success! Credentials: " + AddDefaultUser() + "\n";
            }
            catch (Exception ex)
            {
                response += "Error while default user init! Error message: \n" + ex.Message;
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("UpdateDatabase")]
        public IActionResult RunDatabaseUpdate()
        {
            try
            {
                UpdateDatabase();
                return Ok("Success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("InitDefaultUser")]
        public IActionResult InitDefaultUser()
        {
            try
            {
                return Ok(AddDefaultUser());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Cleanup")]
        public IActionResult CleanUp()
        {
            string response = "";
            try
            {
                response += "Authentication token cleanup success! Details: " + AuthenticationTokensCleanUp() + "\n";
            }
            catch (Exception err)
            {
                response += "Error while authentication token cleanup. Error message: \n" + err + "\n";

                return BadRequest(response);
            }

            return Ok(response);
        }

        private void UpdateDatabase()
        {
            _dataBaseContext.Database.Migrate();
        }

        private string AddDefaultUser()
        {
            string defaultUserEmail = "admin@aditenea.net";
            string defaultUserName = "admin";

            string randomGeneratedPassword = CryptographyHelper.GenerateRandomString(6);

            if (_userRepo.Get().Where(c => c.Email == defaultUserEmail).Any())
            {
                throw new Exception("User already initilized!");
            }

            User defaultUser = new User();
            defaultUser.Email = defaultUserEmail;
            defaultUser.DisplayedName = defaultUserName;
            defaultUser.Role = DataModels.StorageModels.Auth.User.UserRole.Admin;
            defaultUser.PasswordSalt = CryptographyHelper.GeneratePasswordSalt();
            defaultUser.PasswordHash = CryptographyHelper.HashPassword(randomGeneratedPassword, defaultUser.PasswordSalt);

            _userRepo.Add(defaultUser);

            return $"Username: {defaultUser.Email}; Password: {randomGeneratedPassword}";
        }
    
        private string AuthenticationTokensCleanUp()
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
                throw new Exception($"Operation finished with error! Records found: {totalCount}, Records deleted: {deletedCount}, Error message: {ex.Message}");
            }

            return $"Records found: {totalCount}, Records deleted: {deletedCount}";
        }
    }
}
