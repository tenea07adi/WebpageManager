using API.Controllers.Abstract;
using CommonAbstraction.Repository;
using DataModels.DatabaseModels.Webpage;
using DataModels.DataTransferObjects.Webpage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebpageController : BaseImplementationEntityController<WebpageUpsertDTO, WebpageUpsertDTO, WebpageRespDTO, Webpage>
    {
        private readonly IGenericRepo<Webpage> _webpageRepo;
        public WebpageController(IGenericRepo<Webpage> webpageRepo) : base(webpageRepo)
        {
            _webpageRepo = webpageRepo;
        }
    }
}
