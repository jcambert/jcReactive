using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.IO;
namespace jcReactive.Plugins
{
    public interface IAssemblyReferenceCollector
    {
        IEnumerable<MetadataReference> CollectMetadataReferences(Assembly assembly);
    }

    public class AssemblyReferenceCollector : IAssemblyReferenceCollector
    {
        public IEnumerable<MetadataReference> CollectMetadataReferences(Assembly assembly)
        {
            string assemblyName = Path.GetRandomFileName();
            var referencedAssemblyNames = assembly.GetReferencedAssemblies();

            var references = new List<MetadataReference>();
            foreach (AssemblyName an in referencedAssemblyNames)
            {
                var loadedAssembly = Assembly.Load(an);
                try
                {
                    references.Add(MetadataReference.CreateFromFile(loadedAssembly.Location));
                }
                catch (Exception e){ }
            }

            references
                .Add(MetadataReference.CreateFromFile(assembly.Location)); // add a reference to 'self', i.e., NetMWC

            return references;

        }
    }
}
