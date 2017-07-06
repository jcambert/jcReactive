using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Ninject;
using SharpStoreWeb.App_Start;
using SharpStoreWeb.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Ninject;
using System.Web.Http;
using SharpStoreWeb.Controllers;
using Microsoft.Owin.Cors;


[assembly: OwinStartup(typeof(SharpStoreWeb.Startup))]

namespace SharpStoreWeb
{
    public partial class Startup
    {
        public static IAppBuilder AppBuilder;
        public void Configuration(IAppBuilder app)
        {
         //   app.UseCors(CorsOptions.AllowAll);

            AppBuilder = app;
            ConfigureAuth(app);
            ConfigureSignalR(app);
            


        }

        public static void ConfigureSignalR(IAppBuilder app)
        {
            var kernel = NinjectWebCommon.Bootstrapper.Kernel;
            var signalRDependencyResolver = new NInjectSignalRDependencyResolver(kernel);
            // Register hub connection context
            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).
                  ToMethod(context =>
                  signalRDependencyResolver.Resolve<IConnectionManager>().
                  GetHubContext<StoreHub>().Clients).
                  WhenInjectedInto<IBaseController>();


            GlobalConfiguration.Configuration.DependencyResolver = new NInjectDependencyResolver(kernel);

  

            app.MapSignalR("/signalr", new HubConfiguration()
            {
                EnableDetailedErrors = true,
                Resolver = signalRDependencyResolver,
                EnableJavaScriptProxies = true
            });

        }
    }
}
