using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper;
using System.Data.Entity;
using System.Linq.Expressions;
using DataMapper.Building;
using DataMapper.Repositories;

namespace DataMapper.Repositories
{


    /// <summary>
    /// Provides an implementation for a repository that encapsulates the mapping of changes from a
    /// business objects aggregate to an entity framework aggregate. Includes exposed CRUD operations.
    /// </summary>
    /// <typeparam name="TAggregate">A type that represents the aggregate root - usually of a business or service layer.</typeparam>
    /// <typeparam name="TEntity">A type that represents the corresponding entity framework root object.</typeparam>
    /// <typeparam name="TDbContext">The database context. Note that the 'TEntity' type must be defined in this context.</typeparam>
    /// <typeparam name="Key">The key of the aggregate</typeparam>
    public class EntityFrameworkCrudRepository<TAggregate, TEntity, TDbContext, Key> : EntityFrameworkRepository<TAggregate, TEntity, TDbContext>
        where TDbContext : DbContext, new()
        where TEntity : class,new()
        where TAggregate : class,new()
    {

        //public TAggregate TryFind(TDbContext context, Key id)
        //{
        //    return (TAggregate)this.TryFindAggregate(context, id);
        //}
        public TAggregate TryFind(Key id)
        {
            return (TAggregate)this.TryFindAggregate(id);
        }

        //public void Add(TDbContext context, TAggregate item)
        //{
        //    this.AddAggregate(context, item);
        //}
        public void Add(TAggregate item)
        {
            this.AddAggregate(item);
        }

        //public void Update(TDbContext context, TAggregate item)
        //{
        //    this.UpdateAggregate(context, item);
        //}
        public void Update(TAggregate item)
        {
            this.UpdateAggregate(item);
        }

        //public void Delete(TDbContext context, TAggregate item)
        //{
        //    this.DeleteAggregate(context, item);
        //}
        public void Delete(TAggregate item)
        {
            this.DeleteAggregate(item);
        }

        //public Boolean Exists(TDbContext context, Key id)
        //{
        //    return this.EntityExists(context, id);
        //}
        public Boolean Exists(Key id)
        {
            return this.EntityExists(id);
        }

    }

}

