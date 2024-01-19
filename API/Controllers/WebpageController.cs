using API.Controllers.Abstract;
using CommonAbstraction.Repository;
using DataModels.DatabaseModels.Webpage;
using DataModels.DataTransferObjects.Webpage;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebpageController : BaseImplementationEntityController<WebpageUpsertDTO, WebpageUpsertDTO, WebpageRespDTO, Webpage>
    {
        private readonly IGenericProtectedRepo<Webpage, UserSecurityPass> _webpageRepo;
        public WebpageController(IGenericProtectedRepo<Webpage, UserSecurityPass> webpageRepo) : base(webpageRepo)
        {
            _webpageRepo = webpageRepo;
        }
    }
}
