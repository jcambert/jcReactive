using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace jcReactive.Common
{
    public class Repository<T> : IRepository<T>, IInitializable where T : class, IReactiveDbObject,new()
    {
        static Repository()
        {
            Key = ObjectMixins.GetKey<T>();
            Indexes = ObjectMixins.GetIndexes<T>();

        }

        public static PropertyInfo Key { get; private set; }
        public static Dictionary<string, Indexes> Indexes { get; private set; }

        private IReactiveDbContext _context;
        private Guid _guid;

        public IQueryable<T> GetAllUntracked => throw new NotImplementedException();

        public ICollection<T> Local => throw new NotImplementedException();

        [Inject]
        public IKernel Kernel { get; set; }

        public DbContext DbContext => _context as DbContext;

        public DbSet<T> Set() => DbContext.Set<T>();

        public IReactiveDbContext Context => _context;

        public bool? AutoCommitEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Guid => _guid;

        public void Add(T entity)
        {
            //if((Key.GetGetMethod().Invoke(entity,null) as Guid?) ==Guid.Empty)
                Set().Add(entity);
            
        }

        public void Update(T entity)
        {
            //DbContext.Entry(entity).State = EntityState.Detached;
              //Set().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public T Create() => Kernel.Get<T>();


        public void Delete(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(Guid key)
        {
            DbContext.Entry(Get(key)).State = EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e =>
            {
                DbContext.Entry(e).State = EntityState.Deleted;
            });
        }

        public T Get(Guid id)
        {
            return Set().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            Contract.Ensures(Contract.Result<IQueryable<T>>() != null);
            return Set();
        }

        public Task<T> GetAsync(Guid key)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> GetModifiedProperties(T entity)
        {
            throw new NotImplementedException();
        }



        public void Initialize()
        {
            _guid = Guid.NewGuid();
            this._context = Kernel.Get<IReactiveDbContext>();
            if (!(Context is DbContext)) throw new ApplicationException("IReactiveDbContext must be a DbContext");
        }

        public bool IsModified(T entity)
        {
            return DbContext.Entry(entity).State == EntityState.Modified;
        }


        public Task<int> UpdateAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }



        public void Dispose()
        {

        }
    }
}
