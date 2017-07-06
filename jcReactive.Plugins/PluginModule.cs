using Ninject;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Ninject.Extensions.Conventions.Syntax;
using Ninject.Extensions.Conventions.BindingGenerators;
using Ninject.Extensions.Conventions.BindingBuilder;
using Ninject.Syntax;
using Ninject.Activation;

namespace jcReactive.Plugins
{
    public class PluginModule<T> : NinjectModule where T: class,IPlugin,new()
    {
        private T CreateGenerator(IContext context)
        {
            PluginLocator<T> pluginLocator = context.Kernel.Get<PluginLocator<T>>();
            Type roslynPluginType = pluginLocator.Locate();
            return (T)context.Kernel.Get(roslynPluginType ?? typeof(T));
        }

        private readonly Assembly[] assemblies;

        public PluginModule(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }
        public override void Load()
        {
            Bind<T>().ToMethod(context => CreateGenerator(context));
            //Kernel.Bind(x => x.From(assemblies).SelectAllClasses().InheritedFrom<T>().BindToPluginOrDefaultInterfaces<T>());
            Bind<IPluginProvider>().To<PluginProvider<T>>();
            Bind<PluginAssemblyCache<T>>().ToSelf().InSingletonScope();
            Bind<IAssemblyReferenceCollector>().To<AssemblyReferenceCollector>();
           
        }
    }

    public static class ConventionSyntaxExtensions
    {
        public static IConfigureSyntax BindToPluginOrDefaultInterfaces<T>(this IJoinFilterWhereExcludeIncludeBindSyntax syntax) where T:class,IPlugin,new()
        {
            return syntax.BindWith(new DefaultInterfacesBindingGenerator(new BindableTypeSelector(), new PluginOrDefaultBindingCreator<T>()));
        }
    }

    ///// <summary>
    ///// Returns a Ninject binding to a method which returns the plugin type if one exists, otherwise returns the default type.
    ///// </summary>
    public class PluginOrDefaultBindingCreator<T> : IBindingCreator where T:class,IPlugin,new()
    {
        public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(IBindingRoot bindingRoot, IEnumerable<Type> serviceTypes, Type implementationType)
        {
            if (bindingRoot == null)
            {
                throw new ArgumentNullException("bindingRoot");
            }
            

            return !serviceTypes.Any()
             ? Enumerable.Empty<IBindingWhenInNamedWithOrOnSyntax<object>>()
             : new[] { bindingRoot.Bind(serviceTypes.ToArray()).ToMethod(context => context.Kernel.Get(context.Kernel.Get<PluginLocator<T>>().Locate(serviceTypes) ?? implementationType)) };
        }
    }
}
