using Ninject.Infrastructure.Disposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common.Ninject
{
    /// <summary>
    /// Ninject disposable scope
    /// </summary>
    public class NinjectDisposableScope : IDisposable, INotifyWhenDisposed
    {
        #region Events

        public event EventHandler Disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDisposableScope" /> class.
        /// </summary>
        public NinjectDisposableScope()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the scope is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get;
            private set;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.IsDisposed = true;
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }

        #endregion IDisposable Members
    }
}
