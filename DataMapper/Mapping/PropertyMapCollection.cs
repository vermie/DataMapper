using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class PropertyMapCollection : Collection<PropertyMap>
    {
        public Object[] GetSourceKeyValues(Object source)
        {
            return this.GetKeyValues(source, true);
        }
        public Object[] GetTargetKeyValues(Object target)
        {
            return this.GetKeyValues(target, false);
        }
        private Object[] GetKeyValues(Object theObject, Boolean readFromSource)
        {
            List<Object> listy = new List<object>();

            foreach (var item in this.KeyProperties())
            {
                var propInfo = readFromSource ? item.SourcePropertyInfo : item.TargetPropertyInfo;

                listy.Add(propInfo.GetValue(theObject, null));
            }

            return listy.ToArray();
        }

        protected override void InsertItem(int index, PropertyMap item)
        {
            if (this.Contains(item))
            {
                throw new DataMapperException("Cannot add PropertyMap {0} to collection because the property pair has already been mapped.".FormatString(item));
            }

            base.InsertItem(index, item);
        }

        //override the default behavior here
        public new bool Contains(PropertyMap proposed)
        {
            if (proposed == null)
            {
                throw new ArgumentNullException("proposed");
            }

            return
                this.Where(a => a.ContainsPropertyInfoSourceOrTarget(proposed.SourcePropertyInfo, proposed.TargetPropertyInfo)).Any();
        }

        public PropertyMap TryFind(PropertyInfo sourcePropertyInfo, PropertyInfo targetPropertyInfo)
        {
            return this.Where(a => a.SourcePropertyInfo == sourcePropertyInfo && a.TargetPropertyInfo == targetPropertyInfo).FirstOrDefault();
        }

        public PropertyMapCollection GetNonCollectionPropertyMaps()
        {
            PropertyMapCollection listy = new PropertyMapCollection();

            this.Where(a => a.IsCollection == false).ToList().ForEach(b => listy.Add(b));

            return listy;
        }

        public void MapNonCollection(Object source, Object target, MappingDirection changeDirection)
        {
            this.GetNonCollectionPropertyMaps()
                .ForEach(a => a.Copy(source, target, changeDirection));
        }
    }
}
