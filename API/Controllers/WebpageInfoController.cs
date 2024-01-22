using API.ActionFilters;
using API.Controllers.Abstract;
using CommonAbstraction.Repository;
using CommonLogic.DataModelsMapper;
using DataModels.DatabaseModels.Webpage;
using DataModels.DatabaseModels.WebpageSimpleInfo;
using DataModels.DataTransferObjects.WebpageSimpleInfo;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebpageInfoController : BaseImplementationEntityController<WebpageInfoUpsertDTO, WebpageInfoUpsertDTO, WebpageInfoRespDTO, WebpageInfo>
    {
        private readonly IGenericProtectedRepo<Webpage, UserSecurityPass> _webpageRepo;
        private readonly IGenericProtectedRepo<WebpageInfo, UserSecurityPass> _webpageInfoRepo;
        private readonly IGenericProtectedRepo<WebpageInfoCollection, UserSecurityPass> _webpageInfoCollectionRepo;

        public WebpageInfoController(
            IGenericProtectedRepo<Webpage, UserSecurityPass> webpageRepo,
            IGenericProtectedRepo<WebpageInfo, UserSecurityPass> webpageInfoRepo,
            IGenericProtectedRepo<WebpageInfoCollection, UserSecurityPass> webpageInfoCollectionRepo) : base(webpageInfoRepo)
        {
            _webpageRepo = webpageRepo;
            _webpageInfoRepo = webpageInfoRepo;
            _webpageInfoCollectionRepo= webpageInfoCollectionRepo;

            AdditionalChecks();
        }

        [HttpGet("active")]
        [AuthActionFilter(UserSecurityPass.PassRole.Webpage)]
        public IActionResult GetActiveInfo()
        {
            List<WebpageInfo> dbInfos =  _webpageInfoRepo.Get(GenerateSecurityPass()).Where(info => (info.HaveExpirationMoment == false || info.ExpirationMoment > DateTime.Now) && (info.HaveStartMoment == false || info.StartMoment < DateTime.Now)).ToList();

            List<WebpageInfoRespDTO> resInfos = new List<WebpageInfoRespDTO>();

            for (int i = 0; i < dbInfos.Count; i++)
            {
                resInfos.Add(new WebpageInfoRespDTO());
                DataModelsMapper.Mapp(dbInfos[i], resInfos[i]);
            }

            return Ok(resInfos);
        }

        private void AdditionalChecks()
        {
            this.AddAdditionalCheck(Endpoint.Add, HaveCorrectRelations);
            this.AddAdditionalCheck(Endpoint.Update, HaveCorrectRelations);
        }

        private bool HaveCorrectRelations(WebpageInfo webpageInfo)
        {
            if(webpageInfo == null)
            {
                return false;
            }

            if(webpageInfo.WebpageId == 0 || webpageInfo.CollectionId == 0) 
            {
                return false;
            }

            Webpage webpage = _webpageRepo.Get(webpageInfo.WebpageId, GenerateSecurityPass());
            WebpageInfoCollection webpageInfoCollection = _webpageInfoCollectionRepo.Get(webpageInfo.CollectionId, GenerateSecurityPass());

            if (webpage == null || webpageInfoCollection == null)
            {
                return false;
            }

            if (webpage.Id != webpageInfo.WebpageId)
            {
                return false;
            }

            return true;
        }
    
    }
}
