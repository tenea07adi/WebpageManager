using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Abstract
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseEntityController<TAddDTO, TUpdateDTO, TGetDTO, TDatabaseModel> : ControllerBase 
        where TAddDTO: class, new()
        where TUpdateDTO : class, new()
        where TGetDTO : class, new()
        where TDatabaseModel : class, new()
    {
        [HttpGet]
        public abstract IActionResult Get();

        [HttpGet("{id}")]
        public abstract IActionResult Get(int id);

        [HttpPost]
        public abstract IActionResult Add(TAddDTO record);

        [HttpPut("{id}")]
        public abstract IActionResult Update(int id, TUpdateDTO record);

        [HttpDelete("{id}")]
        public abstract IActionResult Delete(int id);
    }
}
