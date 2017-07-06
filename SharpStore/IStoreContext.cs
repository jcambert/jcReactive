using jcReactive.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    public interface IStoreContext : IReactiveDbContext, IDisposable
    {
        DbSet<Product> Products { get; set; }

        DbSet<Parametre> Parametres { get; set; }

    }
}
