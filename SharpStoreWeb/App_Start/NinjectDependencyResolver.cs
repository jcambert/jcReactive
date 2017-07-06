
using Ninject;
using System.Web.Http.Dependencies;

namespace SharpStoreWeb
{

    /// <summary>
    /// NInject dependency resolver class 
    /// </summary>
    public class NInjectDependencyResolver : NInjectScope, IDependencyResolver
    {
        #region Private Variable

        private readonly IKernel _kernel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Get Kernel value 
        /// </summary>
        public IKernel Container
        {
            get { return _kernel; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// NInject dependency resolver class
        /// </summary>
        /// <param name="container">IKernel value</param>
        public NInjectDependencyResolver(IKernel container)
            : base(container)
        {
            _kernel = container;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Begin dependency scope
        /// </summary>
        /// <returns>IDependencyScope object</returns>
        public new IDependencyScope BeginScope()
        {
            return new NInjectScope(_kernel);
        }

        #endregion
    }
}