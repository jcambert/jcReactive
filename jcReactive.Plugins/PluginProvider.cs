using jcReactive.Common;
using jcReactive.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Plugins
{
    public interface IPluginProvider
    {
        IEnumerable<IPluginSnippetContainer> GetPlugins();
    }

    public class PluginProvider<T> : IPluginProvider where T:class,IPlugin,new()
    {
        public PluginProvider(IPluginSource source)
        {
            Contract.Requires(source != null);
            this.source = source;
        }
        
        private readonly IPluginSource source;

        public IEnumerable<IPluginSnippetContainer> GetPlugins()
        {
            var currentVersion = typeof(T).Assembly.GetName().Version;
           
            /// only consider plugins with version numbers greater than the 
            /// currently shipped version.
            return source.Plugins
                .Where(p => new Version(p.Version) > currentVersion)
                .Select(p => p.ToPluginScriptContainer())
                .ToArray();
        }
    }
}
