using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SharpStore;
using Ninject;
using jcReactive.Common;
using System.ComponentModel.DataAnnotations;
using SharpStoreWeb.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SharpStoreWeb.Filters;
using SharpStoreWeb.Exceptions;
using SharpStoreWeb.Models;
using System.Security.Principal;
using System.Web.Http.Cors;

namespace SharpStoreWeb.Controllers
{
    public static class ModelExtension
    {
        internal static Task<TModel> ThrowIfNull<TModel>(this Task<TModel> model)
        {
            if (model.Result.IsNull()) throw new ModelNullException(Localizations.ModelIsNull.Fmt(typeof(TModel).Name));
            return model;
        }


        internal static void CheckModelCanBeLock<TModel>(this TModel model) where TModel : class, IStoreModel, new()
        {
            if (!model.CanLock())
                throw new ModelCanOnlyUnlockByException(new List<string>() { "ModelCanOnlyUnlockBy" }, typeof(TModel).Name, model.LockBy);
        }

        internal static void CheckModelCanBeUnlock<TModel>(this TModel model,IIdentity identity) where TModel : class, IStoreModel, new()
        {
            if (!model.CanUnlock(identity))
                throw new ModelCanOnlyUnlockByException(new List<string>() { "ModelCanOnlyUnlockBy" }, typeof(TModel).Name, model.LockBy);
        }


        internal static void CheckModelIsLockByMe<TModel>(this TModel model, IIdentity identity) where TModel : class, IStoreModel, new()
        {
            if (!model.IsLockByMe(identity))
                throw new ModelMustBeLockException(new List<string>() { "ModelMustBeLock" }, typeof(TModel).Name, model.Key.ToString());
        }
    }
    /// <summary>
    /// Base Store WebApi controller
    /// </summary>
    /// <typeparam name="TModel">The database Model</typeparam>
    /// <typeparam name="TDto">The Dto Model</typeparam>
    [StoreControllerExceptionFilter]
   // [EnableCors("*", "*", "*")]
    public abstract class BaseStoreController<TModel, TDto> : ApiController, IBaseController
        where TModel : class, IStoreModel, new()
        where TDto : class, new()
    {


        [Inject]
        public IReactiveDbContext Context { get; set; }

        [Inject]
        public IRepository<TModel> Repository { get; set; }



        protected Expression<Func<TModel, bool>> GetByIdPredicate(Guid id) => PredicateBuilder.True<TModel>().And(m => m.Key == id);

        protected Expression<Func<TModel, bool>> GetByLockedPredicate() => PredicateBuilder.True<TModel>().And(m => m.IsLock());

        protected Expression<Func<TModel, bool>> GetByUnlockedPredicate() => PredicateBuilder.True<TModel>().And(m => !m.IsLock());

        protected DbSet<TModel> All() => Repository.DbContext.Set<TModel>();

        protected TModel ById(Guid id) => All().Where(GetByIdPredicate(id)).FirstOrDefault();

        protected TModel CheckModelExistWithId(Guid id)
        {
            if (id.IsNull()) throw new IdNotExistException(new List<string>() { "The", "KeyNotNull" }, typeof(TModel).Name);
            var model = ById(id);
            if (model.IsNull()) throw new IdNotExistException(new List<string>() { "The", "KeyNotExist" }, typeof(TModel).Name, id.ToString());
            return model;

        }


        protected async Task<TModel> ByIdAsync(Guid id) => await All().Where(GetByIdPredicate(id)).FirstOrDefaultAsync();

        protected void CheckIfIsNull(TDto dto)
        {
            if (dto.IsNull()) throw new ModelNullException(new List<string>() { "The", "ModelIsNull" },typeof(TModel).Name);
        }

       

       
        /// <summary>
        /// Return one TDto by its key
        /// </summary>
        /// <param name="id">Guid Key</param>
        /// <returns>the finded dto</returns>
        [ActionName("get")]
        public IHttpActionResult Get(Guid id)
        {
            var model = CheckModelExistWithId(id);
           // TModel model = await ByIdAsync(id).ThrowIfNull();

            return Ok(Mapper.Map<TDto>(model));
        }

        /// <summary>
        /// Return all  Models
        /// </summary>
        /// <returns></returns>
        [ActionName("all")]
        public virtual IQueryable<TDto> Get() => All().ProjectTo<TDto>();


        // POST: api/Base
        /// <summary>
        ///  Add a new  model
        /// </summary>
        /// <param name="dto">the new modem</param>
        /// <returns>IHttpActionResult</returns>
        [ActionName("add")]
        [HttpPost]
        public virtual IHttpActionResult Post([FromBody]TDto dto)
        {
            CheckIfIsNull(dto);

            IHttpActionResult result;

            TModel model = Repository.Create();
            Mapper.Map(dto, model, typeof(TDto), typeof(TModel));
            model.SetUser(this.User.Identity);

            Repository.Add(model);

            if (Context.SaveFromController(Ok, BadRequest, out result))
            {
                //       Hub.NotifyAdded(value);

            }

            return result;
        }

        // PUT: api/Base/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ActionName("modify")]
        public virtual IHttpActionResult Put(Guid id, [FromBody] TDto dto)
        {
            CheckIfIsNull(dto);

            var model = CheckModelExistWithId(id);

            model.CheckModelIsLockByMe(User.Identity);

            IHttpActionResult result;


            //if (!CanUnlock(id, out model, out result)) return result;

            Mapper.Map(dto, model, typeof(TDto), typeof(TModel));
            model.Key = id;
            model.SetUser(this.User.Identity);

            // value.Key = id;
            Repository.Update(model);
            if (Context.SaveFromController(Ok, BadRequest, out result))
            { }// Hub?.NotifyAdded<T>(value);
            return result;
        }

        // DELETE: api/Base/5
        /// <summary>
        /// Delete a model 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IHttpActionResult Delete(Guid id)
        {
            var model = CheckModelExistWithId(id);

            IHttpActionResult result;
            Repository.Delete(id);
            if (Context.SaveFromController(Ok, BadRequest, out result))
            { }//  Hub?.NotifyDeleted<T>(entity);
            return result;
        }

        /// <summary>
        /// Lock a model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// [ResponseType(typeof(bool))]
        [HttpPost]
        [ActionName("lock")]
        public virtual HttpResponseMessage PostLock(Guid id)
        {
            var model = CheckModelExistWithId(id);
            model.CheckModelCanBeLock();
            model.Lock(this.User.Identity);
            Context.SaveChanges();
            return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
        }

        [ResponseType(typeof(bool))]
        [HttpPost]
        [ActionName("unlock")]
        public virtual HttpResponseMessage PostUnlock(Guid id)
        {
            var model = CheckModelExistWithId(id);
            model.CheckModelCanBeUnlock(User.Identity);
            model.Unlock(this.User.Identity);
            Context.SaveChanges();
            return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        [ResponseType(typeof(LockByDto))]
        [HttpGet]
        [ActionName("islocked")]
        public HttpResponseMessage GetIslocked(Guid id)
        {
            var model = CheckModelExistWithId(id);
            return Request.CreateResponse<LockByDto>(HttpStatusCode.OK, new LockByDto() {LockAt=model.LockAt,LockBy=model.LockBy });
        }

        /// <summary>
        /// Get information of Creation and modification for a model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Information</returns>
        [ResponseType(typeof(InformationDto))]
        [HttpGet]
        [ActionName("info")]
        public HttpResponseMessage GetInformation(Guid id)
        {
            var model = CheckModelExistWithId(id);
            return Request.CreateResponse<InformationDto>(HttpStatusCode.OK, new InformationDto() { CreatedAt=model.CreatedAt,CreatedBy=model.CreatedBy,ModifiedAt=model.ModifiedAt,ModifiedBy=model.ModifiedBy});
        }
    }
}