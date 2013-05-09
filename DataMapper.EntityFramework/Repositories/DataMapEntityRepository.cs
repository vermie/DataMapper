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

    //not sure how I feel about these. Although I am not quite sick to my stomach....

    public class CrudEntityFrameworkRepository<TAggregate, TEntity, TDbContext, Key> : EntityFrameworkRepository<TAggregate, TEntity, TDbContext>
        where TDbContext : DbContext, new()
        where TEntity : class,new()
        where TAggregate : class,new()
    {

        public TAggregate TryFind(TDbContext context, Key id)
        {
            return (TAggregate)this.TryFindAggregate(context, id);
        }
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

