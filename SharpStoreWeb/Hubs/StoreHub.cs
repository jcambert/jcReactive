using jcReactive.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ninject;
using SharpStore;
using SharpStoreWeb.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpStoreWeb.Helpers;
namespace SharpStoreWeb.Hubs
{
    //  [AuthorizeClaims]
    public class StoreHub : Hub<IStoreHub>
    {
        protected readonly IHubContext<IStoreHub> HubContext;
        protected readonly IKernel Kernel;
        public StoreHub()
        {

        }

        public StoreHub(IHubContext<IStoreHub> ctx, IKernel kernel)
        {
            this.HubContext = ctx;
            this.Kernel = kernel;
        }

        public void NotifyAdded<T>(T value)
        {
            //   Clients.All.NotifyAdded(nameof(T), value);
            HubContext.Clients.All.NotifyAdded(value);
            Clients.All.NotifyAdded(value);
        }


        //public IHubConnectionContext<IBaseHub> Clients => HubContext.Clients;

        public void NotifyDeleted<T>(T value)
        {
            //   Clients.All.NotifyDeleted(nameof(T), value);
            HubContext.Clients.All.NotifyDeleted(value);
            Clients.All.NotifyDeleted(value);
        }

        public void NotifyModified<T>(T value)
        {
            //    Clients.All.NotifyModified(nameof(T), value);
            HubContext.Clients.All.NotifyModified(value);
            Clients.All.NotifyModified(value);

        }

        /// <summary>
        /// Lock an entity for modify it
        /// </summary>
        /// <param name="type">the StoreModel Type to lock</param>
        /// <param name="id">the entity's id</param>
        public void LockEntity(string type, Guid id)
        {


            dynamic repo = getRepository(type);
            StoreModel entity = (StoreModel)repo.Get(id);
            if (entity != null && entity.Lock(this.Context.User.Identity))
                repo.DbContext.SaveChanges();

        }

        /// <summary>
        /// Lock an entity for modify it
        /// </summary>
        /// <param name="type">the StoreModel Type to unlock</param>
        /// <param name="id">the entity's id</param>
        public void UnlockEntity(string type, Guid id)
        {
            dynamic repo = getRepository(type);
            StoreModel entity = (StoreModel)repo.Get(id);
            if (entity != null && entity.Unlock(this.Context.User.Identity))
                repo.DbContext.SaveChanges();

        }

        private dynamic getRepository(string type)
        {
            var repoType = typeof(IRepository<>);
            var repoGenType = repoType.MakeGenericType(Type.GetType($"SharpStore.{type},SharpStore"));
            return Kernel.Get(repoGenType);
        }

        /// <summary>
        /// Send to User that email arrived
        /// </summary>
        public void NotifyMail(int mailCount)
        {
            Clients.Caller.OnMail(mailCount);
        }
    }
}