using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jcReactive.Common.Ninject
{
    /// <summary>
    /// Ninject ambient scope.
    /// </summary>
    /// <remarks>
    /// Nested scopes are supported, but multi-threading in nested scopes is not supported.
    /// </remarks>
    public class NinjectAmbientScope : NinjectDisposableScope
    {
        #region Fields

        private static readonly AsyncLocal<NinjectAmbientScope> scopeHolder = new AsyncLocal<NinjectAmbientScope>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectAmbientScope" /> class.
        /// </summary>
        public NinjectAmbientScope()
        {
            TransientScopes = new ConcurrentQueue<NinjectDisposableScope>();
            scopeHolder.Value = this;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current ambient scope.
        /// </summary>
        public static NinjectAmbientScope Current
        {
            get { return scopeHolder.Value; }
        }

        /// <summary>
        /// Gets the DisposableScope queue used for transient injection during the ambient scope lifetime.
        /// </summary>
        private ConcurrentQueue<NinjectDisposableScope> TransientScopes { get; set; }

        #endregion Properties

        #region Methods

        public NinjectDisposableScope CreateTransientScope()
        {
            if (IsDisposed)
            {
                throw new ApplicationException("Ambient scope is disposed");
            }
            var scope = new NinjectDisposableScope();
            TransientScopes.Enqueue(scope);
            return scope;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            foreach (var transientScope in this.TransientScopes)
            {
                transientScope.Dispose();
            }
        }

        #endregion IDisposable Members
    }
}
