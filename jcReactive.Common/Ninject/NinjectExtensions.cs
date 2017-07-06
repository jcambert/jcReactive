using Ninject.Activation;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common.Ninject
{
    /// <summary>
    /// Defines ambient scope extension methods.
    /// </summary>
    public static class NinjectExtensions
    {
        #region Methods

        /// <summary>
        /// Sets the scope to ambient scope.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>The syntax to define more information.</returns>
        public static IBindingNamedWithOrOnSyntax<T> InAmbientScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(GetAmbientScope);
        }

        private static object GetAmbientScope(IContext ctx)
        {
            var scope = NinjectAmbientScope.Current;
            if (scope != null)
            {
                return scope;
            }
            throw new ApplicationException("No ambient scope defined");
        }

        /// <summary>
        /// Sets the scope to ambient scope and inject as transient within the current ambient scope.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <returns>The syntax to define more information.</returns>
        public static IBindingNamedWithOrOnSyntax<T> InAmbientScopeAsTransient<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(GetAmbientScopeAsTransient);
        }

        private static object GetAmbientScopeAsTransient(IContext ctx)
        {
            var scope = NinjectAmbientScope.Current.CreateTransientScope();
            if (scope != null)
            {
                return scope;
            }
            throw new ApplicationException("No ambient scope defined");
        }

        #endregion Methods
    }
}
