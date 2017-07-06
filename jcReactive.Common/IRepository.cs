using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    
    public interface IRepository<T> :IDisposable where T:class,IReactiveDbObject 
    {
        Guid Guid { get; }

        /// <summary>
        /// Gets an entity by id from the database or the local change tracker.
        /// </summary>
        /// <param name="id">The id of the entity. This can also be a composite key.</param>
        /// <returns>The resolved entity</returns>
        T Get(Guid id);
        Task<T> GetAsync(Guid key);
        /// <summary>
        /// Returns the queryable entity set for the given type {T}.
        /// </summary>
        IQueryable<T> GetAll();


        /// <summary>
        /// Returns an untracked queryable entity set for the given type {T}.
        /// The entities returned will not be cached in the object context thus increasing performance.
        /// </summary>
        IQueryable<T> GetAllUntracked { get; }
        /// <summary>
        /// Provides access to the entities currently being tracked by the context and have not been marked as deleted
        /// </summary>
        ICollection<T> Local { get; }
        /// <summary>
        /// Creates a new instance of an entity of type {T}
        /// </summary>
        /// <returns>The new entity instance.</returns>
        T Create();
        /// <summary>
        /// Add Or insert an entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);
        /// <summary>
        /// Marks the changes of an existing entity to be saved to the store.
        /// </summary>
        /// <param name="entity">An instance that should be updated in the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void Update(T entity);
        /// <summary>
        /// Marks the changes of existing entities to be saved to the store.
        /// </summary>
        /// <param name="entities">A list of entity instances that should be updated in the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void UpdateRange(IEnumerable<T> entities);
        Task<int> UpdateAsync();
        void Delete(T entity);
        void Delete(Guid key);
        /// <summary>
        /// Marks existing entities to be deleted from the store.
        /// </summary>
        /// <param name="entities">A list of entity instances that should be deleted from the database.</param>
        /// <remarks>Implementors should delegate this to the current <see cref="IDbContext" /></remarks>
        void DeleteRange(IEnumerable<T> entities);
        /// <summary>
        /// Gets a value indicating whether the given entity was modified since it has been attached to the context
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns><c>true</c> if the entity was modified, <c>false</c> otherwise</returns>
        bool IsModified(T entity);

        /// <summary>
        /// Gets a list of modified properties for the specified entity
        /// </summary>
        /// <param name="entity">The entity instance for which to get modified properties for</param>
        /// <returns>
        /// A dictionary, where the key is the name of the modified property
        /// and the value is its ORIGINAL value (which was tracked when the entity
        /// was attached to the context the first time)
        /// Returns an empty dictionary if no modification could be detected.
        /// </returns>
        IDictionary<string, object> GetModifiedProperties(T entity);

        /// <summary>
        /// Returns the data context associated with the repository.
        /// </summary>
        /// <remarks>
        /// The context is likely shared among multiple repository types.
        /// So committing data or changing configuration also affects other repositories. 
        /// </remarks>
        IReactiveDbContext Context { get; }

        /// <summary>
        /// Gets or sets a value indicating whether database write operations
        /// such as insert, delete or update should be committed immediately.
        /// </summary>
		/// <remarks>
		/// Set this to <c>true</c> or <c>false</c> to supersede the global <c>AutoCommitEnabled</c>
		/// on <see cref="IDbContext"/> level for this repository instance only.
		/// </remarks>
        bool? AutoCommitEnabled { get; set; }

        DbContext DbContext { get; }

        DbSet<T> Set() ;


    }
}
