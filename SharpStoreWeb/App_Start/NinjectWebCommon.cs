[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SharpStoreWeb.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SharpStoreWeb.App_Start.NinjectWebCommon), "Stop")]

namespace SharpStoreWeb.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using jcReactive.Common;
    using SharpStore;
    using SharpStoreWeb.Hubs;
    using System.Web.Http;
    using Microsoft.AspNet.SignalR;
    using Ninject.Activation;
    using AutoMapper;
    using SharpStoreWeb.Models;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static Bootstrapper Bootstrapper => bootstrapper;

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
            MapperConfig.RegisterMappings(Bootstrapper.Kernel);

        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            NinjectSettings settings = new NinjectSettings()
            {
                InjectNonPublic = true
            };
            var kernel = new StandardKernel(settings);
            try
            {


                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => Bootstrapper.Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();



                RegisterServices(kernel);
                


                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //  kernel.Bind<IReactiveDbContext>().ToProvider<DbContextProvider>().InRequestScope();
            kernel.Bind<IReactiveDbContext>().To<StoreContext>().InRequestScope().OnActivation(ActivateDbContext);
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind<IUIPage>().To<UIPage>();
            kernel.Bind<IMenu>().To<Menu>();
            kernel.Bind<IProduct>().To<Product>().OnActivation(ActivateProduct);
            kernel.Bind<IParametre>().To<Parametre>();
            kernel.Bind<IHubContext<IStoreHub>>().ToMethod(ctx => GlobalHost.ConnectionManager.GetHubContext<StoreHub, IStoreHub>()).InSingletonScope();
            kernel.Bind<StoreHub>().ToSelf().InSingletonScope().WithConstructorArgument("context", GlobalHost.ConnectionManager.GetHubContext<StoreHub, IStoreHub>()).WithConstructorArgument("kernel", kernel);


        }

        public static void ActivateProduct(Product product)
        {
            product.Adding.Subscribe(p =>
            {

            });
        }

        private static void ActivateDbContext(IContext context, StoreContext ctx)
        {
            var hub = context.Kernel.Get<StoreHub>();
            ctx.EntityAdding.Subscribe(e =>
            {
               //TODO Subscribe to business methods
            });

            ctx.EntityAdded.Subscribe(e =>
            {
                hub.NotifyAdded(e.Sender);
            });

            ctx.EntityDeleted.Subscribe(e =>
            {
                hub.NotifyDeleted(e.Sender);
            });

            ctx.EntityUpdated.Subscribe(e =>
            {
                hub.NotifyModified(e.Sender);
            });
        }


       

    }
}
