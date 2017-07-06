using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Plugins
{
    [Serializable]
    public class PluginCompilationException : Exception
    {
        // constructors...
        #region PluginCompilationException()
        /// <summary>
        /// Constructs a new PluginCompilationException.
        /// </summary>
        public PluginCompilationException() { }
        #endregion
        #region PluginCompilationException(string message)
        /// <summary>
        /// Constructs a new PluginCompilationException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public PluginCompilationException(string message) : base(message) { }
        #endregion
        #region PluginCompilationException(string message, Exception innerException)
        /// <summary>
        /// Constructs a new PluginCompilationException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public PluginCompilationException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
        #region PluginCompilationException(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected PluginCompilationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
