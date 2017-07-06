using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace SharpStoreWeb.Hubs
{
    public interface IStoreHub
    {
        void NotifyModified<T>(T value);
        void NotifyAdded<T>(T value);
        void NotifyDeleted<T>(T entity);

        void LockEntity(string type, Guid id);
        void UnlockEntity(string type, Guid id);


        void OnMail(int mailCount);
    }
}