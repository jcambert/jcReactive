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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Chiffrage>().HasMany(c => c.Articles).WithMany(a => a.Chiffrages);
        }
        public DbSet<UIPage> Pages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Article> Products { get ; set ; }
        public DbSet<Parametre> Parametres { get; set; }
        public DbSet<Chiffrage> Chiffrages { get; set; }

    }
    
}
