using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.Repositories
{

    /// <summary>
    /// Use this repository if your entity framework objects are your business/service objects but they are 
    /// traversing a serialization boundary (such as web service or file serialization). Changes to the graph will
    /// automatically be applied without the need for proxies or self tracking entities.
    /// </summary>
    /// <typeparam name="TEntity">A type that represents the corresponding entity framework root object.</typeparam>
    /// <typeparam name="TDbContext">The database context. Note that the 'TEntity' type must be defined in this context.</typeparam>
    /// <typeparam name="Key">The key of the entity</typeparam>
    public class EntityFrameworkSelfRepository<TEntity, TDbContext, Key> : EntityFrameworkCrudRepository<TEntity, TEntity, TDbContext, Key>
        where TDbContext : DbContext, new()
        where TEntity : class,new()
    {

    }

}
