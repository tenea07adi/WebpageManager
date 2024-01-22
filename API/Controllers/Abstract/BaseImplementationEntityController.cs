using API.ActionFilters;
using CommonAbstraction.DataModels;
using CommonAbstraction.Repository;
using CommonLogic.BusinessLogic;
using CommonLogic.DataModelsMapper;
using DataModels.DatabaseModels.Webpage;
using DataModels.StorageModels.Auth;
using DataModels.UtilityModels.Security;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Abstract
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseImplementationEntityController<TAddDTO, TUpdateDTO, TGetDTO, TDatabaseModel> : BaseEntityController<TAddDTO, TUpdateDTO, TGetDTO, TDatabaseModel> 
        where TAddDTO : class, new()
        where TUpdateDTO : class, new()
        where TGetDTO : class, new()
        where TDatabaseModel : BaseStorageModel, new()
    {
        protected enum Endpoint { Add, Update, Delete, Get, GetAll};

        private readonly IGenericProtectedRepo<TDatabaseModel, UserSecurityPass> _genericRepo;

        private Dictionary<Endpoint, Func<TDatabaseModel, bool>> _additionalChecks = new Dictionary<Endpoint, Func<TDatabaseModel, bool>>();

        public BaseImplementationEntityController(IGenericProtectedRepo<TDatabaseModel, UserSecurityPass> genericRepo)
        {
            this._genericRepo = genericRepo;
        }

        [AuthActionFilter(UserSecurityPass.PassRole.User)]
        public override IActionResult Add(TAddDTO record)
        {

            TDatabaseModel mappedRecord = new TDatabaseModel();

            DataModelsMapper.Mapp(record, mappedRecord);

            if (_additionalChecks.ContainsKey(Endpoint.Add) && !_additionalChecks[Endpoint.Add].Invoke(mappedRecord))
            {
                return BadRequest("Invalid record!");
            }

            _genericRepo.Add(mappedRecord, GenerateSecurityPass());

            TGetDTO resp = new TGetDTO();

            DataModelsMapper.Mapp(mappedRecord, resp);

            return Ok(resp);
        }

        [AuthActionFilter(UserSecurityPass.PassRole.User)]
        public override IActionResult Delete(int id)
        {
            TDatabaseModel record = _genericRepo.Get(id, GenerateSecurityPass());

            if(record == null)
            {
                return NotFound();
            }

            if (_additionalChecks.ContainsKey(Endpoint.Add) && !_additionalChecks[Endpoint.Add].Invoke(record))
            {
                return BadRequest("Invalid record!");
            }

            _genericRepo.Delete(id, GenerateSecurityPass());

            return Ok();
        }

        [AuthActionFilter(UserSecurityPass.PassRole.Webpage)]
        public override IActionResult Get()
        {
            List<TDatabaseModel> databaseModels = new List<TDatabaseModel>();
            List<TGetDTO> getDTOs = new List<TGetDTO>();

            databaseModels = _genericRepo.Get(GenerateSecurityPass());

            for(int i = 0; i <databaseModels.Count; i ++)
            {
                getDTOs.Add( new TGetDTO());
                DataModelsMapper.Mapp(databaseModels[i], getDTOs[i]);
            }

            return Ok(getDTOs);
        }

        [AuthActionFilter(UserSecurityPass.PassRole.Webpage)]
        public override IActionResult Get(int id)
        {
            TGetDTO getDTO = new TGetDTO();
            TDatabaseModel databaseModel = null;

            databaseModel = _genericRepo.Get(id, GenerateSecurityPass());

            if(databaseModel == null)
            {
                return NotFound("Not fount or not authorized!");
            }

            DataModelsMapper.Mapp(databaseModel, getDTO);

            return Ok(getDTO);
        }

        [AuthActionFilter(UserSecurityPass.PassRole.User)]
        public override IActionResult Update(int id, TUpdateDTO record)
        {
            TDatabaseModel mappedRecord = null;

            mappedRecord = _genericRepo.Get(id, GenerateSecurityPass());

            if(mappedRecord == null)
            {
                return NotFound();
            }

            DataModelsMapper.Mapp(record, mappedRecord);

            if (_additionalChecks.ContainsKey(Endpoint.Add) && !_additionalChecks[Endpoint.Add].Invoke(_genericRepo.Get(id)))
            {
                return BadRequest("Invalid record!");
            }

            _genericRepo.Update(mappedRecord, GenerateSecurityPass());

            return Ok(record);
        }

        protected void AddAdditionalCheck(Endpoint endpoint, Func<TDatabaseModel, bool> func)
        {
            if(_additionalChecks.ContainsKey(endpoint))
            {
                _additionalChecks[endpoint] = func;
            }
            else
            {
                _additionalChecks.Add(endpoint, func);
            }
        }

        protected UserSecurityPass GenerateSecurityPass()
        {
            IGenericRepo<User> usersRepo = HttpContext.RequestServices.GetService<IGenericRepo<User>>();
            IGenericRepo<Webpage> webpageRepo = HttpContext.RequestServices.GetService<IGenericRepo<Webpage>>();
            IGenericRepo<AuthenticationToken> tokenRepo = HttpContext.RequestServices.GetService<IGenericRepo<AuthenticationToken>>();

            return AuthLogic.GenerateUserSecurityPass(webpageRepo, usersRepo, tokenRepo, AuthLogic.GetContextSecurityData(HttpContext));
        }
    }
}
