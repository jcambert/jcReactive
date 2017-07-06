using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    public interface IReactiveDbContext : IReactiveObject, IInitializable
    {
        Guid Guid { get; }

        bool TriggersEnabled { get; set; }

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IObservable<IReactiveDbObjectEventArgs> EntityAdding { get; }
        IObservable<IReactiveDbObjectEventArgs> EntityAdded { get; }
        void RaiseEntityAdding(IReactiveDbObjectEventArgs arg);
        void RaiseEntityAdded(IReactiveDbObjectEventArgs arg);

        IObservable<IReactiveDbObjectEventArgs> EntityDeleting { get; }
        IObservable<IReactiveDbObjectEventArgs> EntityDeleted { get; }
        void RaiseEntityDeleting(IReactiveDbObjectEventArgs arg);
        void RaiseEntityDeleted(IReactiveDbObjectEventArgs arg);

        IObservable<IReactiveDbObjectEventArgs> EntityUpdating { get; }
        IObservable<IReactiveDbObjectEventArgs> EntityUpdated { get; }
        void RaiseEntityUpdating(IReactiveDbObjectEventArgs arg);
        void RaiseEntityUpdated(IReactiveDbObjectEventArgs arg);

        #region ERRORS
        IObservable<IReactiveDbContextEventArgs> Error { get; }

        int ErrorCountSubscriber { get; }

        void RaiseError(Exception ex);
        void DetachEntity<TEntity>(TEntity x) where TEntity : IReactiveDbObject;

        #endregion

        #region Validation
        IObservable<ValidationEntitiesException> ValidationError { get; }
        int ValidationErrorCountSubscriber { get; }


        bool EnableValidation { get; set; }
        bool ForceNoTracking { get; set; }

        void RaiseValidationError(ValidationEntitiesException ex);
        void Validate();

        void Attach<TEntity>(TEntity x) where TEntity : IReactiveDbObject;
        void ChangeState<TEntity>(TEntity entity, EntityState unchanged) where TEntity : IReactiveDbObject;

        TEntity Create<TEntity>() where TEntity : IReactiveDbObject;
        #endregion

    }

    internal class EntityEntryComparer : IEqualityComparer<DbEntityEntry>
    {
        private EntityEntryComparer() { }
        public bool Equals(DbEntityEntry x, DbEntityEntry y) => ReferenceEquals(x.Entity, y.Entity);
        public int GetHashCode(DbEntityEntry obj) => obj.Entity.GetHashCode();
        public static readonly EntityEntryComparer Default = new EntityEntryComparer();
    }

    internal static class DbContextExtensions
    {


        public static int SaveChangesWithTriggers<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<bool, int> baseSaveChanges, bool acceptAllChangesOnSuccess = true) where TReactiveDbContext : IReactiveDbContext
        {



            try
            {
                dbContext.Validate();
                var afterEvents = dbContext.RaiseBeforeEvents();
                var result = baseSaveChanges(acceptAllChangesOnSuccess);
                afterEvents.RaiseAfterEvents();

                return result;
            }
            catch (ValidationEntitiesException ex)
                when (dbContext.RaiseValidationFailedEvents(ex))
            { }
            catch (Exception ex)
                when (dbContext.RaiseFailedEvents(ex))
            { }
            return 0;


        }

        private static List<Action> RaiseBeforeEvents<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : IReactiveDbContext
        {


            var entries = dbContext.GetReactiveDbObjectEntries();

            var triggeredEntries = new List<DbEntityEntry>(entries.Count);

            List<Action> afterEvents = new List<Action>(entries.Count);
            while (entries.Any())
            {
                foreach (var entry in entries)
                {
                    var events = entry.GetPairEvent();
                    events.Before();
                    afterEvents.Add(events.After);

                    triggeredEntries.Add(entry);
                }
                var newEntries = dbContext.GetReactiveDbObjectEntries().Except(triggeredEntries, EntityEntryComparer.Default);
                entries.Clear();
                entries.AddRange(newEntries);
            }
            return afterEvents;
        }

        private static void RaiseAfterEvents(this List<Action> events)
        {
            events.ForEach(
                @event =>
                {
                    @event();

                });
        }

        private static bool RaiseFailedEvents<TReactiveDbContext>(this TReactiveDbContext context, Exception ex) where TReactiveDbContext : IReactiveDbContext
        {

            var entityResult = false;
            var contextResult = false;
            contextResult = context.RaiseDbContextError(ex);
            context.GetReactiveDbObjectEntries().ForEach(entry =>
            {
                entityResult = ((IReactiveDbObject)entry.Entity).RaiseDbEntityError(ex) && true;
            });
            return contextResult || entityResult;
        }


        private static bool RaiseValidationFailedEvents<TReactiveDbcontext>(this TReactiveDbcontext context, ValidationEntitiesException ex) where TReactiveDbcontext : IReactiveDbContext
        {

            var entityResult = false;
            var contextResult = false;

            contextResult = context.RaiseDbValidationContextError(ex);
            ex.Errors.Errors.ForEach(entry =>
            {

                entityResult = ((IReactiveDbObject)entry.Entity).RaiseDbValidationEntityError(entry.Exception) && true;

            });
            return contextResult || entityResult;
        }

        public static List<DbEntityEntry> GetReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : IReactiveDbContext
             => (dbContext as DbContext)?.ChangeTracker.Entries().Where(e => { return typeof(IReactiveDbObject).IsAssignableFrom(e.Entity.GetType()); }).ToList();



        public static List<DbEntityEntry> GetValidatableReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : IReactiveDbContext
            => dbContext.GetReactiveDbObjectEntries().Where(e => { return e.State == EntityState.Added || e.State == EntityState.Modified; }).ToList();

        public static BeforeAfterDbEvent<TEntry> GetPairEvent<TEntry>(this TEntry entry) where TEntry : DbEntityEntry
           => new BeforeAfterDbEvent<TEntry>(entry);







        public static async Task<int> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
            where TReactiveDbContext : IReactiveDbContext
        {
            return await Task.Run(() =>
            {
                return 1;
            });
        }
        public static Task<Int32> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, CancellationToken cancellationToken = default(CancellationToken))
            where TReactiveDbContext : IReactiveDbContext
        {
            return dbContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);
        }


    }
    public class BeforeAfterDbEvent<TEntry> where TEntry : DbEntityEntry

    {
        public BeforeAfterDbEvent(TEntry entry)
        {
            this.Entry = entry;
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleting;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleted;
                    break;
                case EntityState.Modified:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdating;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdated;
                    break;
                case EntityState.Added:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityAdding;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityAdded;
                    break;
                default:
                    break;
            }
        }

        public Action Before { get; private set; } = () => { };
        public Action After { get; private set; } = () => { };
        public TEntry Entry { get; private set; }

    }
}
