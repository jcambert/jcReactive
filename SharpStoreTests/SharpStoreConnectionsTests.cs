using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpStore;
using Ninject;
using jcReactive.Common;
using jcReactive.Common.Ninject;
using System.Reactive;
using System.Reactive.Linq;
using System.Linq;
using System.Diagnostics;

namespace SharpStoreTests
{
    [TestClass]
    public class SharpStoreConnectionsTests
    {
        static IKernel Kernel;
        static Action<IReactiveDbObjectEventArgs> onAdding;
        static Action<IReactiveDbObjectEventArgs> onAdded;
        private Action<IReactiveDbObjectEventArgs> onDeleted;

      /*  [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            
            
        }*/
        [TestInitialize]
        public void MyTestInitialize()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
            Kernel = new StandardKernel();
            Kernel.Bind<IReactiveDbContext>().To<StoreContext>().InAmbientScope();
            Kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InAmbientScopeAsTransient();
            Kernel.Bind<IArticle>().To<Article>();

            EffortProviderFactory.ResetDb();
            onAdding = e =>
            {
                (e.Sender as StoreModel).Key = Guid.NewGuid();
            };

            onAdded = e =>
             {
                 var s = e.Sender as StoreModel;
                 Trace.WriteLine($"{s.Key} was successfully added to the repo");
             };

            onDeleted = e =>
            {
                var s = e.Sender as StoreModel;
                Trace.WriteLine($"{s.Key} was successfully deleted to the repo");
            };
        }
        [TestMethod]
        public void TestConnectionSelfInstance()
        {
            var ctx = new StoreContext();
            Assert.IsNotNull(ctx);
        }

        [TestMethod]
        public void TestConnectionDI()
        {
            Action a = () =>
            {
                var ctx = Kernel.Get<IReactiveDbContext>();
                Assert.IsNotNull(ctx);
                Assert.IsInstanceOfType(ctx, typeof(StoreContext));
            };
            inAmbiant(a);
        }

        [TestMethod]
        public void TestCreateSimpleProduct()
        {
            Action a = () =>
             {
                 using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                 {
                     var product = Kernel.Get<IArticle>();
                     Assert.IsNotNull(product);
                     Assert.IsInstanceOfType(product, typeof(Article));
                 }
             };
            inAmbiant(a);
        }

        [TestMethod]
        public void TestAddSimpleProduct()
        {
            Action a = () =>
            {
                using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                {
                    var product = Kernel.Get<IArticle>();
                    Assert.IsNotNull(product);
                    Assert.IsInstanceOfType(product, typeof(Article));

                    ctx.Products.Add(product as Article);
                }
            };
            inAmbiant(a);
        }

        [TestMethod]
        public void TestSaveSimpleProduct()
        {
            Action a = () =>
            {
                using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                {
                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        var product = repo.Create();
                        product.Adding.Subscribe(onAdding, e => { });
                        product.Code = "456";
                        Assert.IsNotNull(product);
                        Assert.IsInstanceOfType(product, typeof(Article));
                        repo.Add(product as Article);
                    }
                    ctx.SaveChanges();

                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        var products = repo.GetAll();
                        Assert.AreEqual(products.Count(), 1);
                    }
                }
            };
            inAmbiant(a);
        }

        [TestMethod]
        public void TestContextScope()
        {

            Guid first_ctx = Guid.Empty;
            Guid second_ctx = Guid.Empty;
            Guid first_repo = Guid.Empty;
            Guid second_repo = Guid.Empty;
            Action a = () =>
            {
                using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                {
                    first_ctx = ctx.Guid;
                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        Assert.AreEqual(ctx.Guid, repo.Context.Guid);
                        first_repo = repo.Guid;
                    }
                }

                using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                {
                    second_ctx = ctx.Guid;
                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        Assert.AreEqual(ctx.Guid, repo.Context.Guid);
                        second_repo = repo.Guid;
                    }
                }
                Assert.AreEqual(first_ctx, second_ctx);
                Assert.AreNotEqual(first_repo, second_repo);
            };

            inAmbiant(a);

        }

        [TestMethod]
        public void TestAddSimpleProductToRepository()
        {
            Action a = () =>
            {
                using (var repo = Kernel.Get<IRepository<Article>>())
                {
                    Assert.IsNotNull(repo.Context);
                    Assert.IsInstanceOfType(repo.Context, typeof(IStoreContext));

                    var product = repo.Create();
                    Assert.IsNotNull(product);
                    Assert.IsInstanceOfType(product, typeof(Article));

                    repo.Add(product as Article);
                }
            };
            inAmbiant(a);
        }

        [TestMethod]
        public void TestSaveSimpleProductWithRepository()
        {
            Action a = () =>
            {
                using (var ctx = Kernel.Get<IReactiveDbContext>() as IStoreContext)
                {
                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        Assert.IsNotNull(repo.Context);
                        Assert.IsInstanceOfType(repo.Context, typeof(IStoreContext));

                        var product = repo.Create();
                        product.Added.Subscribe(onAdded, e => { });
                        product.Adding.Subscribe(onAdding, e => { });
                        Assert.IsNotNull(product);
                        Assert.IsInstanceOfType(product, typeof(Article));

                        repo.Add(product as Article);


                    }

                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        Assert.IsNotNull(repo.Context);
                        Assert.IsInstanceOfType(repo.Context, typeof(IStoreContext));

                        var product = repo.Create();
                        product.Added.Subscribe(onAdded, e => { });
                        product.Adding.Subscribe(onAdding, e => { });
                        Assert.IsNotNull(product);
                        Assert.IsInstanceOfType(product, typeof(Article));

                        repo.Add(product as Article);


                    }

                    var res = ctx.SaveChanges();
                    Assert.AreEqual(2, res);


                    using (var repo = Kernel.Get<IRepository<Article>>())
                    {
                        Assert.AreEqual(2, repo.GetAll().Count());

                        repo.GetAll().ToList().ForEach(p =>
                        {
                            Trace.WriteLine(p.Key);
                            p.Deleted.Subscribe(onDeleted, e => { });
                        });

                        repo.DeleteRange(repo.Set());
                        ctx.SaveChanges();
                    }
                }
            };
            inAmbiant(a);
        }

        private void inAmbiant(Action a)
        {
            using (var scope = new NinjectAmbientScope())
            {
                a();
            }
        }
    }
}
