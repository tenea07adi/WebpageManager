using API.Controllers.Abstract;
using CommonAbstraction.Repository;
using DataModels.DatabaseModels.Webpage;
using DataModels.DatabaseModels.WebpageSimpleInfo;
using DataModels.DataTransferObjects.WebpageSimpleInfo;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebpageInfoCollectionController : BaseImplementationEntityController<WebpageInfoCollectionUpsertDTO, WebpageInfoCollectionUpsertDTO, WebpageInfoCollectionRespDTO, WebpageInfoCollection>
    {
        private readonly IGenericProtectedRepo<Webpage, UserSecurityPass> _webpageRepo;
        private readonly IGenericProtectedRepo<WebpageInfoCollection, UserSecurityPass> _webpageInfoCollectionRepo;

        public WebpageInfoCollectionController(IGenericProtectedRepo<Webpage, UserSecurityPass> webpageRepo, IGenericProtectedRepo<WebpageInfoCollection, UserSecurityPass> webpageInfoCollectionRepo) : base(webpageInfoCollectionRepo)
        {
            _webpageRepo = webpageRepo;
            _webpageInfoCollectionRepo = webpageInfoCollectionRepo;

            AdditionalChecks();
        }

        private void AdditionalChecks()
        {
            this.AddAdditionalCheck(Endpoint.Add, IsWebpageRelated);
            this.AddAdditionalCheck(Endpoint.Update, IsWebpageRelated);
        }

        private bool IsWebpageRelated(WebpageInfoCollection webpageInfo)
        {
            if (webpageInfo == null)
            {
                return false;
            }

            if (webpageInfo.WebpageId == 0)
            {
                return false;
            }

            if (_webpageRepo.Get(webpageInfo.WebpageId, GenerateSecurityPass()) == null)
            {
                return false;
            }

            return true;
        }
    }
}
