using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Building;
using DataMapper.Commands;
using DataMapper.Mapping;
using DataMapper.TypeMapping;

namespace DataMapper
{
    public class TypeMapStore
    {
        private List<DataMapBuilderCore> _builderList = new List<DataMapBuilderCore>();
        private Boolean _finished = false;

        public Boolean Finished
        {
            get
            {
                return this._finished;
            }
        }

        public ITypeMapper<Source, Target> Define<Source, Target>()
        {
            return Define<Source, Target>(false);
        }
        public ITypeMapper<Source, Target> Define<Source, Target>(Boolean replaceExistingDefinitionIfDefined)
        {
            var builder = this.TryFindDataMapBuilderCore(typeof(Source), typeof(Target));

            if ((builder == null) || (replaceExistingDefinitionIfDefined))
            {
                if (replaceExistingDefinitionIfDefined)
                {
                    this._builderList.Remove(builder);
                }

                builder = new TypeMapper<Source, Target>();

                foreach (var prop in builder.SourcePropertyInfoHashSet)
                {
                    if (prop.IsPropertyTypeCollectionType())
                    {
                        builder.IgnoreProperty(prop);
                    }
                }

                this._builderList.Add(builder);
            }

            if (builder._state.IsBuilderFinished)
            {
                throw new TypeMapException("Unable to get type map definition because it has already been finalized.");
            }

            return builder as TypeMapper<Source, Target>;
        }

        public DataMapValidationList Validate()
        {
            if (this._finished)
            {
                throw new TypeMapException("The store has already been finalized.");
            }

            DataMapValidationList listy = new DataMapValidationList();

            foreach (var item in _builderList)
            {
                listy.Add(item.Validate());
            }

            return listy;
        }

        public void Finish()
        {
            if (this._finished)
            {
                throw new TypeMapException("The store has already been finalized.");
            }

            //map any remaining properties using convention
            this._builderList.ForEach(a => a.MapRemainingByConvention(PropertyMapUnresolvedBehavior.None));

            var validation = this.Validate();

            if (validation.IsValidList == false)
            {
                throw new TypeMapValidationException(validation);
            }
            //return builder as ITypeCopier<Source,Target>;

            this._finished = true;
            this._builderList.ForEach(a => a.FinalizeMap());
        }

        //public TypeMapStore MapToTarget<Source, Target>(Source source, Target target)
        //{
        //    return this.Map<Source, Target>(source, target, CommandChangeDirection.ApplyChangesFromSourceToTarget);
        //}
        //public TypeMapStore MapToSource<Source, Target>(Source source, Target target)
        //{
        //    return this.Map<Source, Target>(source, target, CommandChangeDirection.ApplyChangesFromTargetToSource);
        //}
        public TypeMapStore Map<Source, Target>(Source source, Target target)
        {
            if (this._finished == false)
            {
                throw new TypeMapException("Unable to perform mapping. You must Finish the store before you can use it.");
            }

            var command = this.TryResolveDataMapCommand<Source, Target>(source, target, CommandChangeDirection.ApplyChangesFromSourceToTarget);

            if (command == null)
            {
                //yes, yes. I know this is confusing as HELL. Sorry. I will try to wrap unit tests around this...
                command = this.TryResolveDataMapCommand<Target, Source>(target, source, CommandChangeDirection.ApplyChangesFromTargetToSource);
            }


            if (command == null)
            {
                throw new TypeMapException("Unable to map source to target. The type is not defined");
            }
            else
            {
                command.ApplyChanges();
            }

            return this;
        }

        private Command TryResolveDataMapCommand<Source, Target>(Source source, Target target, CommandChangeDirection changeDirection)
        {
            Command dataMapCommand = null;

            var dataMapCommandBuilder = new CommandBuilder();
            var dataMap = this.TryFindDataMap<Source, Target>();

            //we found a match
            if (dataMap != null)
            {
                dataMapCommand =
                   dataMapCommandBuilder.Build(
                   dataMap,
                   changeDirection,
                   source,
                   target);
            }

            return dataMapCommand;
        }
        private DataMap TryFindDataMap<Source, Target>()
        {
            return this.TryFindDataMap(typeof(Source), typeof(Target));
        }
        private DataMap TryFindDataMap(Type sourceType, Type targetType)
        {
            var builder = this.TryFindDataMapBuilderCore(sourceType, targetType);

            if (builder != null)
                return builder.FinalizeMap();

            return null;
        }
        private DataMapBuilderCore TryFindDataMapBuilderCore(Type sourceType, Type targetType)
        {
            return this._builderList.Where(a => a.SourceType == sourceType && a.TargetType == targetType).FirstOrDefault();
        }
    }
}
