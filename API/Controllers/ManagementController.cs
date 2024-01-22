using DatabaseLayer.DatabaseConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly DataBaseContext _dataBaseContext;

        public ManagementController(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        [HttpGet("UpdateDatabase")]
        public IActionResult RunDatabaseUpdate()
        {
            try
            {
                _dataBaseContext.Database.Migrate();
                return Ok("Success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}
