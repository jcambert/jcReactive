using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.Reflection;
using Ninject;
using jcReactive.Plugins;
using jcReactive.Plugins.Models;

namespace JcReactive.Plugins.Tests
{
    [TestClass]
    public class UnitTest1
    {
        static IKernel Kernel;

        [TestInitialize]
        public void MyTestInitialize()
        {
            Kernel = new StandardKernel(new PluginModule<TestPlugin>());
            Kernel.Bind<IPluginSource>().To<PluginsTestSource>();
            
        }


        [TestMethod]
        public void TestMethod1()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;
                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }");

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("RoslynCompileSample.Writer");
                    object obj = Activator.CreateInstance(type);
                    type.InvokeMember("Write",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        new object[] { "Hello World" });
                }
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var assembly= Assembly.GetCallingAssembly();
            var referencedAssemblyNames = assembly.GetReferencedAssemblies();

            var references = new List<MetadataReference>();
            foreach (AssemblyName an in referencedAssemblyNames)
            {
                var loadedAssembly = Assembly.Load(an);
                try
                {
                    references.Add(MetadataReference.CreateFromFile(loadedAssembly.Location));
                }
                catch (Exception e) {
                }
            }
        }

        [TestMethod]
        public void MyTestMethod2()
        {
            var plugin = Kernel.Get<TestPlugin>();
            Assert.IsNotNull(plugin);
        }
    }
    public class PluginsTestSource : IPluginSource
    {
        private readonly List<Plugin> _plugins;

        public PluginsTestSource()
        {
            _plugins = new List<Plugin> {
                new Plugin()
                {
                    ID=1,Name="TestPlugin",Version="2.0.0",Script=""
                }
            };
        }
        public List<Plugin> Plugins => _plugins;
    }
}
