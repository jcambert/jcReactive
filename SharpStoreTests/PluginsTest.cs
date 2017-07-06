using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using jcReactive.Plugins;
using jcReactive.Plugins.Models;
using System.Collections.Generic;

namespace SharpStoreTests
{
    [TestClass]
    public class PluginsTest
    {
        static IKernel Kernel;

        [TestInitialize]
        public void MyTestInitialize()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
            Kernel = new StandardKernel(new PluginModule<TestPlugin>());
            Kernel.Bind<IPluginSource>().To<PluginsTestSource>();

            EffortProviderFactory.ResetDb();
        }

        [TestMethod]
        public void TestMethod1()
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
