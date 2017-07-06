using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    /// <summary>
    /// IObservedChange is a generic interface that is returned from WhenAny()
    /// Note that it is used for both Changing (i.e.'before change')
    /// and Changed Observables.
    /// </summary>
    public interface IObservedChange<out TSender, out TValue>
    {
        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        TSender Sender { get; }

        /// <summary>
        /// The expression of the member that has changed on Sender.
        /// </summary>
        Expression Expression { get; }

        /// <summary>
        /// The value of the property that has changed. IMPORTANT NOTE: This
        /// property is often not set for performance reasons, unless you have
        /// explicitly requested an Observable for a property via a method such
        /// as ObservableForProperty. To retrieve the value for the property,
        /// use the GetValue() extension method.
        /// </summary>
        TValue Value { get; }
    }

    /// <summary>
    /// A data-only version of IObservedChange
    /// </summary>
    public class ObservedChange<TSender, TValue> : IObservedChange<TSender, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedChange{TSender, TValue}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="expression">Expression describing the member.</param>
        /// <param name="value">The value.</param>
        public ObservedChange(TSender sender, Expression expression, TValue value = default(TValue))
        {
            this.Sender = sender;
            this.Expression = expression;
            this.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public TValue Value { get; private set; }
    }

    /// <summary>
    /// IReactivePropertyChangedEventArgs is a generic interface that
    /// is used to wrap the NotifyPropertyChangedEventArgs and gives
    /// information about changed properties. It includes also
    /// the sender of the notification.
    /// Note that it is used for both Changing (i.e.'before change')
    /// and Changed Observables.
    /// </summary>
    public interface IReactivePropertyChangedEventArgs<out TSender>
    {
        /// <summary>
        /// The name of the property that has changed on Sender.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        TSender Sender { get; }
    }

    /// <summary>
    /// IReactiveNotifyPropertyChanged represents an extended version of
    /// INotifyPropertyChanged that also exposes typed Observables.
    /// </summary>
    public interface IReactiveNotifyPropertyChanged<out TSender>
    {
        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed. Note that this should not fire duplicate change notifications if a
        /// property is set to the same value multiple times.
        /// </summary>
        IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing { get; }

        /// <summary>
        /// Represents an Observable that fires *after* a property has changed.
        /// Note that this should not fire duplicate change notifications if a
        /// property is set to the same value multiple times.
        /// </summary>
        IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed { get; }

        /// <summary>
        /// When this method is called, an object will not fire change
        /// notifications (neither traditional nor Observable notifications)
        /// until the return value is disposed.
        /// </summary>
        /// <returns>An object that, when disposed, reenables change
        /// notifications.</returns>
        IDisposable SuppressChangeNotifications();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    public class ReactivePropertyChangingEventArgs<TSender> : PropertyChangingEventArgs, IReactivePropertyChangedEventArgs<TSender>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangingEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactivePropertyChangingEventArgs(TSender sender, string propertyName)
            : base(propertyName)
        {
            this.Sender = sender;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    public class ReactivePropertyChangedEventArgs<TSender> : PropertyChangedEventArgs, IReactivePropertyChangedEventArgs<TSender>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactivePropertyChangedEventArgs(TSender sender, string propertyName)
            : base(propertyName)
        {
            this.Sender = sender;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }
    }

    public interface IValidationEntityEventArg
    {
        IValidationEntityError Error { get; }
    }
    public interface IValidationEntitiesEventArg
    {

        IReactiveDbContext Context { get; }
        List<IValidationEntityError> Errors { get; }
    }
    public interface IReactiveDbObjectEventArgs
    {


        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        IReactiveDbObject Sender { get; }

        Exception Exception { get; }
    }
    public interface IReactiveDbContextEventArgs
    {


        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        IReactiveDbContext Sender { get; }

        Exception Exception { get; }
    }
    public class ReactiveDbObjectEventArgs : EventArgs, IReactiveDbObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ReactiveDbObjectEventArgs(IReactiveDbObject sender) : this(sender, null)
        {

        }

        public ReactiveDbObjectEventArgs(IReactiveDbObject sender, Exception ex)
        {
            this.Sender = sender;
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IReactiveDbObject Sender { get; private set; }
    }

    public class ReactiveDbContextEventArgs : EventArgs, IReactiveDbContextEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ReactiveDbContextEventArgs(IReactiveDbContext sender) : this(sender, null)
        {

        }

        public ReactiveDbContextEventArgs(IReactiveDbContext sender, Exception ex)
        {
            this.Sender = sender;
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IReactiveDbContext Sender { get; private set; }
    }

    public class ValidationEntitiesEventArg : EventArgs, IValidationEntitiesEventArg
    {


        public ValidationEntitiesEventArg(IReactiveDbContext context, List<IValidationEntityError> errors)
        {
            this.Context = context;
            this.Errors = errors;
        }

        public IReactiveDbContext Context { get; private set; }

        public List<IValidationEntityError> Errors { get; private set; }
    }

    public class ValidationEntitiesException : Exception
    {
        public ValidationEntitiesException(ValidationEntitiesEventArg args)
        {
            this.Errors = args;
        }

        public ValidationEntitiesEventArg Errors { get; private set; }
    }


    public class ValidationEntityEventArg : EventArgs, IValidationEntityEventArg
    {


        public ValidationEntityEventArg(IValidationEntityError error)
        {
            this.Error = error;
        }


        public IValidationEntityError Error { get; private set; }
    }

    public class ValidationEntityException : Exception
    {
        public ValidationEntityException(IReactiveDbObject sender, ValidationEntityEventArg args)
        {
            this.Errors = args;
            this.Sender = sender;
        }

        public ValidationEntityEventArg Errors { get; private set; }
        public IReactiveDbObject Sender { get; private set; }
    }

    public interface IValidationEntityError
    {
        IReactiveDbObject Entity { get; }
        ValidationException Exception { get; }
    }

    public class ValidationEntityError : IValidationEntityError
    {



        public ValidationEntityError(IReactiveDbObject entity, ValidationException ex)
        {

            this.Entity = entity;
            this.Exception = ex;
        }

        public IReactiveDbObject Entity { get; private set; }
        public ValidationException Exception { get; private set; }

    }
    public delegate void ReactiveDbEventHandler(IReactiveDbObject sender, IReactiveDbObjectEventArgs e);

    public delegate void ValidationEntityEventHandler(IValidationEntityEventArg e);

    public interface INotifyEntityAdded
    {
        event ReactiveDbEventHandler OnAdded;
    }
    public interface INotifyEntityAdding
    {
        event ReactiveDbEventHandler OnAdding;
    }
}
