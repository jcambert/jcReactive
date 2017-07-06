using jcReactive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Ninject;

namespace SharpStore
{
    public class StoreContext : ReactiveDbContext, IStoreContext
    {

        public StoreContext()
        {
            
        }

        public DbSet<UIPage> Pages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Product> Products { get ; set ; }
        public DbSet<Parametre> Parametres { get; set; }

    }
}
