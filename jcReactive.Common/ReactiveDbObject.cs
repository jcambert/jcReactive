using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    public abstract class ReactiveDbObject : ReactiveObject, IReactiveDbObject
    {
        #region events
        public event ReactiveDbEventHandler OnAdded = delegate { };
        public void RaiseEntityAdded(IReactiveDbObjectEventArgs args) => this.OnAdded?.Invoke(this, args);

        public event ReactiveDbEventHandler OnAdding = delegate { };
        public void RaiseEntityAdding(IReactiveDbObjectEventArgs args) => this.OnAdding?.Invoke(this, args);

        public event ReactiveDbEventHandler OnUpdated = delegate { };
        public void RaiseEntityUpdated(IReactiveDbObjectEventArgs args) => this.OnUpdated?.Invoke(this, args);

        public event ReactiveDbEventHandler OnUpdating = delegate { };
        public void RaiseEntityUpdating(IReactiveDbObjectEventArgs args) => this.OnUpdating?.Invoke(this, args);

        public event ReactiveDbEventHandler OnDeleted = delegate { };
        public void RaiseEntityDeleted(IReactiveDbObjectEventArgs args) => this.OnDeleted?.Invoke(this, args);

        public event ReactiveDbEventHandler OnDeleting = delegate { };
        public void RaiseEntityDeleting(IReactiveDbObjectEventArgs args) => this.OnDeleting?.Invoke(this, args);

        public event ReactiveDbEventHandler OnError = delegate { };
        public void RaiseEntityError(IReactiveDbObjectEventArgs args) => this.OnError?.Invoke(this, args);


        public event ValidationEntityEventHandler OnValidationError=delegate { };
        public void RaiseEntityValidationError(IValidationEntityEventArg args) => this.OnValidationError?.Invoke(args);

        #endregion

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be adding.
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Adding => ((IReactiveDbObject)this).getAddingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is added
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Added => ((IReactiveDbObject)this).getAddedObservable();


        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be updating.
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Updating => ((IReactiveDbObject)this).getUpdatingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is updated
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Updated => ((IReactiveDbObject)this).getUpdatedObservable();

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be deleting.
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Deleting => ((IReactiveDbObject)this).getDeletingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is deleted
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Deleted => ((IReactiveDbObject)this).getDeletedObservable();

        /// <summary>
        /// Represents an Observable that fires *after* a property is on error
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<IReactiveDbObjectEventArgs> Error => ((IReactiveDbObject)this).getErrorObservable();

        /// <summary>
        /// Represents an Observable that fires *after* a property is on error
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public IObservable<ValidationEntityEventArg> ValidationError => ((IReactiveDbObject)this).getValidationErrorObservable();



    }

}
