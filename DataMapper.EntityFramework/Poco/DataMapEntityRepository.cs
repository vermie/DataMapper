using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMapper;
using System.Data.Entity;
using System.Linq.Expressions;
using DataMapper.Building;

namespace DataMapper.EntityFramework.Poco
{

    public abstract class DataMapEntityRepository<Context, Entity, EntityTarget> : DataMapEntityRepositoryGenericImplementation
        where Context : DbContext, new()
        where Entity : class,new()
        where EntityTarget : class,new()
    {

        #region Protected
        public DataMapValidation Validation
        {
            get;
            private set;
        }
        #endregion

        #region Abstract/virtual

        protected virtual void BuildDataMap(DataMapBuilder<Entity, EntityTarget> builder)
        {
            //by default we will map everything by convention
            builder.MapRemainingByConvention(PropertyMapUnresolvedBehavior.ThrowException);
        }

        #endregion

        #region Public

        public DataMapEntityRepository()
        {
            //create the builder
            var builder = new DataMapBuilder<Entity, EntityTarget>();

            //Call into the builder to allow user to build the mapping.
            this.BuildDataMap(builder);

            //a shame validate will be called twice...we could fix this but for now lets not worry about it.
            //performance not an issue unless it is an issue (id rather have the clarity than the performance)!

            //Get the finalized map from the databuilder
            this.EntityToEntityTargetDataMap = builder.FinalizeMap();

            //store the validation result
            this.Validation = builder.Validate();

            //set the context type
            this.DbContextType = typeof(Context);    
        }

        public void Add(Context context, EntityTarget item)
        {
            this.AddEntityTarget(context, item);
        }
        public void Add(EntityTarget item)
        {
            this.AddEntityTarget(item);
        }

        public void Update(Context context, EntityTarget item)
        {
            this.UpdateEntityTarget(context, item);
        }
        public void Update(EntityTarget item)
        {
            this.UpdateEntityTarget(item);
        }

        public void Delete(Context context, EntityTarget item)
        {
            this.DeleteEntityTarget(context, item);
        }
        public void Delete(EntityTarget item)
        {
            this.DeleteEntityTarget(item);
        }

        #endregion

    }


    //not sure how I feel about these. Although I am not quite sick to my stomach....

    public abstract class DataMapEntityRepository<Context, Entity, EntityTarget, EntityKey> : DataMapEntityRepository<Context, Entity, EntityTarget>
        where Context : DbContext, new()
        where Entity : class,new()
        where EntityTarget : class,new()
    {

        #region Public

        public EntityTarget TryFind(Context context, EntityKey id)
        {
            return (EntityTarget)this.TryFindEntityTarget(context, id);
        }
        public EntityTarget TryFind(EntityKey id)
        {
            return (EntityTarget)this.TryFindEntityTarget(id);
        }

        public Boolean Exists(Context context, EntityKey id)
        {
            return this.EntityExists(context, id);
        }
        public Boolean Exists(EntityKey id)
        {
            return this.EntityExists(id);
        }

        #endregion
    }

    public abstract class DataMapEntityRepository<Context, Entity, EntityTarget, EntityKey1, EntityKey2> : DataMapEntityRepository<Context, Entity, EntityTarget>
        where Context : DbContext, new()
        where Entity : class,new()
        where EntityTarget : class,new()
    {

        #region Public

        public EntityTarget TryFind(Context context, EntityKey1 idPart1, EntityKey2 idPart2)
        {
            return (EntityTarget)this.TryFindEntityTarget(context, new Object[] { idPart1, idPart2 });
        }
        public EntityTarget TryFind(EntityKey1 idPart1, EntityKey2 idPart2)
        {
            return (EntityTarget)this.TryFindEntityTarget(new Object[] {idPart1,idPart2});
        }

        public Boolean Exists(Context context, EntityKey1 idPart1, EntityKey2 idPart2)
        {
            return this.EntityExists(context, idPart1, idPart2);
        }
        public Boolean Exists(EntityKey1 idPart1, EntityKey2 idPart2)
        {
            return this.EntityExists(idPart1, idPart2);
        }

        #endregion
    }

}

