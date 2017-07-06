using SharpStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using jcReactive.Common;
namespace SharpStoreWeb.Helpers
{
    public static class StoreModelExtensions
    {
        public const string ANONYMOUS = "Anonymous";

        public static string GetUserName(this IIdentity identity) => (identity.IsAuthenticated ? identity.Name : ANONYMOUS);

        public static bool IsLock(this IStoreModel model) => !model.LockBy.IsNullOrEmpty() && !model.LockAt.IsNull();
        

        public static bool IsLockByMe(this IStoreModel model, IIdentity identity)
        {
            if (!model.IsLock()) return false;
            return model.LockBy == identity.GetUserName();
        }

        public static bool CanUnlock(this IStoreModel model,IIdentity identity)
        {
            if (model.IsLock()) return true;

            return model.LockBy == identity.GetUserName();
        }

        public static bool CanLock(this IStoreModel model) => model.LockBy.IsNull();
        

        public static bool Lock(this IStoreModel model,IIdentity identity)
        {
            if (!model.CanLock()) return false;
            model.LockBy = identity.GetUserName();
            model.LockAt = DateTime.Now;
            return true;
        }

        public static bool Unlock(this IStoreModel model,IIdentity identity)
        {
            if (!model.CanUnlock(identity)) return false;
            model.LockBy = null;
            model.LockAt = null;
            return true;
        }

        public static bool IsNew(this IStoreModel model) => model.Key.IsNull();

        public static void SetUser(this IStoreModel model,IIdentity identity)
        {
            if (model.IsNew())
            {
                model.CreatedBy = identity.GetUserName();
                model.CreatedAt = DateTime.Now;
            }
            else
            {
                model.ModifiedBy = identity.GetUserName();
                model.ModifiedAt = DateTime.Now;
            }
        }

    }
}