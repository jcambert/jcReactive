using jcReactive.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SharpStoreWeb.Helpers;
using SharpStore;
using SharpStoreWeb.Hubs;
using SharpStoreWeb.Attributes;
using PropertyValueMapType = System.Collections.Generic.Dictionary<string, object>;
using System.ComponentModel.DataAnnotations;

namespace SharpStoreWeb.Controllers
{
    //[Authorize]
    public abstract class BaseController<T> : ApiController, IBaseController where T : class, IStoreModel, new()
    {


        public BaseController()
        {

        }
        [Inject]
        public IReactiveDbContext Context { get; set; }

        [Inject]
        public IRepository<T> Repository { get; set; }


        /*  [Inject]
          public BaseHub Hub { get; set; }*/

        //[Inject]
        // public IBaseHub Hub { get; set; }

        // GET: api/Base
        public virtual IEnumerable<T> Get()
        {
            return Repository.GetAll().ToList();
        }

        // GET: api/Base/5
        public virtual T Get(Guid id)
        {
            return Repository.Get(id);
        }

        // POST: api/Base
        [StoreModel()]
        public virtual IHttpActionResult Post([FromBody, Required]T value)
        {
            if (value.IsNull()) return BadRequest("Object cannot be null. Check well formatted json object representation");
            IHttpActionResult result;

            value.SetUser(this.User.Identity);

            Repository.Add(value);

            if (Context.SaveFromController(Ok, BadRequest, out result))
            {
                //       Hub.NotifyAdded(value);

            }

            return result;
        }

        // PUT: api/Base/5
        public virtual IHttpActionResult Put(Guid id, PropertyValueMapType updatedPropertyValueMap/*[FromBody]T value*/)
        {
            IHttpActionResult result;
            if (id.IsNull()) return BadRequest("Id cannot be null. Check well formatted json object representation");
            if (updatedPropertyValueMap.IsNull()) return BadRequest("Object cannot be null. Check well formatted json object representation");
            T value;
            if (!CanUnlock(id,out value, out result)) return result;
            /* var original = Repository.Get(id);
             if (original.IsNull()) return BadRequest($"There is no {nameof(T)} whith these id {id}");
             if (!original.CanUnlock(User.Identity)) return BadRequest($"only {original.LockBy} can save this {nameof(T)}");
             */

            var propertyInfos = typeof(T).GetProperties();
            foreach (var propertyValuePair in updatedPropertyValueMap) {
                propertyInfos.Single(x => x.Name == propertyValuePair.Key).SetValue(value, propertyValuePair.Value);
            }


            value.SetUser(this.User.Identity);
                
           // value.Key = id;
            Repository.Update(value);
            if (Context.SaveFromController(Ok, BadRequest, out result))
            { }// Hub?.NotifyAdded<T>(value);
            return result;
        }

        // DELETE: api/Base/5
        public virtual IHttpActionResult Delete(Guid id)
        {
            if (id.IsNull()) return BadRequest("Id cannot be null. Check well formatted json object representation");
            IHttpActionResult result;
            var entity = Get(id);
            Repository.Delete(id);
            if (Context.SaveFromController(Ok, BadRequest, out result))
            { }//  Hub?.NotifyDeleted<T>(entity);
            return result;
        }

        protected bool CanUnlock(Guid id,out T original,out IHttpActionResult result)
        {
            result = null;
            original = Repository.Get(id);
            if (original.IsNull()) result= BadRequest($"There is no {nameof(T)} whith these id {id}");
            if (!original.CanUnlock(User.Identity)) result= BadRequest($"only {original.LockBy} can save this {nameof(T)}");
           
            return result != null;
        }
    }
}
