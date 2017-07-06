using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using System.Data.Entity.Infrastructure;

namespace jcReactive.Common
{
    public class ReactiveDbContext : DbContext, IReactiveDbContext
    {
        #region fields
        ISubject<IReactiveDbContextEventArgs> errorSubject;
        IObservable<IReactiveDbContextEventArgs> errorObservable;
        private Countable errorCountable;

        ISubject<ValidationEntitiesException> validationErrorSubject;
        IObservable<ValidationEntitiesException> validationErrorObservable;
        private Countable validationErrorCountable;
        private Guid _guid;


        ISubject<IReactiveDbObjectEventArgs> addedSubject;
        ISubject<IReactiveDbObjectEventArgs> updatedSubject;
        ISubject<IReactiveDbObjectEventArgs> deletedSubject;

        ISubject<IReactiveDbObjectEventArgs> addingSubject;
        ISubject<IReactiveDbObjectEventArgs> updatingSubject;
        ISubject<IReactiveDbObjectEventArgs> deletingSubject;
        // IObservable<IReactiveDbObjectEventArgs> addedObservable;


        #endregion

        #region Constructors
        public ReactiveDbContext() : base()
        { /*Initialize();*/
            internalInitialize();
        }

        public ReactiveDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            internalInitialize();
        }


        private void internalInitialize()
        {
            this.addingSubject = new Subject<IReactiveDbObjectEventArgs>();
            this.addedSubject = new Subject<IReactiveDbObjectEventArgs>();

            this.updatedSubject = new Subject<IReactiveDbObjectEventArgs>();
            this.updatingSubject = new Subject<IReactiveDbObjectEventArgs>();

            this.deletedSubject = new Subject<IReactiveDbObjectEventArgs>();
            this.deletingSubject = new Subject<IReactiveDbObjectEventArgs>();
        }

        public virtual void Initialize()
        {
            _guid = Guid.NewGuid();

            this.errorSubject = new Subject<IReactiveDbContextEventArgs>();
            this.errorObservable = errorSubject.Publish().RefCount();
            this.errorCountable = new Countable();
            this.errorObservable = this.errorCountable.GetCountable(this.errorObservable);

            this.validationErrorSubject = new Subject<ValidationEntitiesException>();
            this.validationErrorObservable = validationErrorSubject.Publish().RefCount();
            this.validationErrorCountable = new Countable();
            this.validationErrorObservable = this.validationErrorCountable.GetCountable(this.validationErrorObservable);


        }


        #endregion

        #region Properties
        [Reactive]
        public bool TriggersEnabled { get; set; } = true;
        #endregion

        #region SaveChanges


        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw HandleDbUpdateException(e);
            }
        }
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() => TriggersEnabled ? this.SaveChangesWithTriggers(SaveChanges, true) : SaveChanges(true);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => TriggersEnabled ? this.SaveChangesWithTriggersAsync(SaveChangesAsync, cancellationToken) : base.SaveChangesAsync(cancellationToken);



        public void DetachEntity<TEntity>(TEntity x) where TEntity : IReactiveDbObject
        {

            Entry(x as object).State = EntityState.Detached;
        }

        public void Attach<TEntity>(TEntity x) where TEntity : IReactiveDbObject
        {
            Set(typeof(TEntity)).Attach(x);

        }

        public void ChangeState<TEntity>(TEntity entity, EntityState state) where TEntity : IReactiveDbObject
        {
            this.Entry(entity as object).State = state;
        }

        public TEntity Create<TEntity>() where TEntity : IReactiveDbObject
            => (TEntity)Set(typeof(TEntity)).Create();

        #endregion

        #region IReactiveObject
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => this.PropertyChanging?.Invoke(this, args);

        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => this.PropertyChanged?.Invoke(this, args);

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => ((IReactiveObject)this).getChangingObservable();

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => ((IReactiveObject)this).getChangedObservable();

        public IObservable<IReactiveDbObjectEventArgs> EntityAdding => addingSubject;
        public IObservable<IReactiveDbObjectEventArgs> EntityAdded => addedSubject;

        public IObservable<IReactiveDbObjectEventArgs> EntityUpdating => updatingSubject;
        public IObservable<IReactiveDbObjectEventArgs> EntityUpdated => updatedSubject;

        public IObservable<IReactiveDbObjectEventArgs> EntityDeleting => deletingSubject;
        public IObservable<IReactiveDbObjectEventArgs> EntityDeleted => deletedSubject;

        #endregion

        #region ERROR
        public IObservable<IReactiveDbContextEventArgs> Error => this.errorObservable;
        public int ErrorCountSubscriber => errorCountable.Count;
        public void RaiseError(Exception ex)
        {
            errorSubject.OnNext(new ReactiveDbContextEventArgs(this, ex));
        }

        #endregion

        #region Validation
        public IObservable<ValidationEntitiesException> ValidationError => this.validationErrorObservable;
        public int ValidationErrorCountSubscriber => validationErrorCountable.Count;


        public bool EnableValidation { get; set; } = true;
        public bool ForceNoTracking { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Guid => _guid;

        public void RaiseValidationError(ValidationEntitiesException ex) => validationErrorSubject.OnNext(ex);
        public void Validate()
        {
            if (!EnableValidation) return;
            /* var entities = from e in this.ChangeTracker.Entries()
                            where e.State == EntityState.Added
                                || e.State == EntityState.Modified
                            select e.Entity;*/
            var entities = this.GetValidatableReactiveDbObjectEntries();
            List<IValidationEntityError> errors = new List<IValidationEntityError>();
            foreach (var entityEntry in entities)
            {
                var entity = (IReactiveDbObject)entityEntry.Entity;
                try
                {

                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, true);

                    AddEntryForNotification(entityEntry);
                }
                catch (ValidationException ex)
                {
                    errors.Add(new ValidationEntityError(entity, ex));
                    //throw new ValidationEntityException(new ValidationEntityEventArg(this, entity, ex));
                }
            }
            if (errors.Count > 0)
            {
                throw new ValidationEntitiesException(new ValidationEntitiesEventArg(this, errors));
            }
        }


        private void AddEntryForNotification(DbEntityEntry entityEntry)
        {
            if (EntriesForNotification.Keys.Contains(entityEntry.Entity as ReactiveDbObject)) return;
            EntriesForNotification[entityEntry.Entity as ReactiveDbObject] = new EntryNotification(this, entityEntry);
            /*
            switch (entityEntry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Added:
                    (entityEntry.Entity as ReactiveDbObject).Adding.Subscribe(e => this.RaiseEntityAdding(e));
                    (entityEntry.Entity as ReactiveDbObject).Added.Subscribe(e => this.RaiseEntityAdded(e));
                    break;
                case EntityState.Deleted:
                    (entityEntry.Entity as ReactiveDbObject).Deleting.Subscribe(e => this.RaiseEntityDeleting(e));
                    (entityEntry.Entity as ReactiveDbObject).Deleted.Subscribe(e => this.RaiseEntityDeleted(e));
                    break;
                case EntityState.Modified:
                    (entityEntry.Entity as ReactiveDbObject).Updating.Subscribe(e => this.RaiseEntityUpdating(e));
                    (entityEntry.Entity as ReactiveDbObject).Updated.Subscribe(e => this.RaiseEntityUpdated(e));
                    break;
                default:
                    break;
            }*/
        }

       readonly Dictionary<ReactiveDbObject, EntryNotification> EntriesForNotification = new Dictionary<ReactiveDbObject, EntryNotification>();

        private class EntryNotification : IDisposable
        {
            public EntryNotification(ReactiveDbContext ctx, DbEntityEntry entry)
            {
                addingSubscriber = (entry.Entity as ReactiveDbObject)?.Adding.Subscribe(e => ctx.RaiseEntityAdding(e));
                addedSubscriber = (entry.Entity as ReactiveDbObject)?.Added.Subscribe(e => ctx.RaiseEntityAdded(e));
                deletingSubscriber = (entry.Entity as ReactiveDbObject)?.Deleting.Subscribe(e => ctx.RaiseEntityDeleting(e));
                deletedSubscriber = (entry.Entity as ReactiveDbObject)?.Deleted.Subscribe(e => ctx.RaiseEntityDeleted(e));
                updatingSubscriber = (entry.Entity as ReactiveDbObject)?.Updating.Subscribe(e => ctx.RaiseEntityUpdating(e));
                updatedSubscriber = (entry.Entity as ReactiveDbObject)?.Updated.Subscribe(e => ctx.RaiseEntityUpdated(e));
            }

            #region IDisposable Support
            private bool disposedValue = false; // Pour détecter les appels redondants
            private readonly IDisposable addingSubscriber;
            private readonly IDisposable addedSubscriber;
            private readonly IDisposable deletingSubscriber;
            private readonly IDisposable deletedSubscriber;
            private readonly IDisposable updatingSubscriber;
            private readonly IDisposable updatedSubscriber;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: supprimer l'état managé (objets managés).
                        addedSubscriber.Dispose();
                        addingSubscriber.Dispose();
                        deletedSubscriber.Dispose();
                        deletingSubscriber.Dispose();
                        updatedSubscriber.Dispose();
                        updatingSubscriber.Dispose();
                    }

                    // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                    // TODO: définir les champs de grande taille avec la valeur Null.

                    disposedValue = true;
                }
            }

            // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
            // ~EntryNotification() {
            //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            //   Dispose(false);
            // }

            // Ce code est ajouté pour implémenter correctement le modèle supprimable.
            public void Dispose()
            {
                // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
                Dispose(true);
                // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }





        #endregion

        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new DbUpdateException(message, dbu);
        }

        public void RaiseEntityAdding(IReactiveDbObjectEventArgs arg)
        {
            addingSubject.OnNext(arg);
        }
        public void RaiseEntityAdded(IReactiveDbObjectEventArgs arg)
        {
            addedSubject.OnNext(arg);
        }
        public void RaiseEntityUpdating(IReactiveDbObjectEventArgs arge)
        {
            updatingSubject.OnNext(arge);
        }
        public void RaiseEntityUpdated(IReactiveDbObjectEventArgs arge)
        {
            updatedSubject.OnNext(arge);
        }
        public void RaiseEntityDeleting(IReactiveDbObjectEventArgs arg)
        {
            deletingSubject.OnNext(arg);
        }
        public void RaiseEntityDeleted(IReactiveDbObjectEventArgs arg)
        {
            deletedSubject.OnNext(arg);
        }
    }

}
