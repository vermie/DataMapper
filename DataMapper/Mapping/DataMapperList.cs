using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DataMapper.Mapping
{
    [Serializable()]
    public class DataMapperList : IDataMapperList
    {
        private Object _underlyingListObject = null;

        public DataMapperList(Object underlyingListObject)
        {
            if (underlyingListObject == null)
                throw new ArgumentNullException("Unable to access underlying list object because it is null");

            this._underlyingListObject = underlyingListObject;
        }

        private Object UnderlyingListObject
        {
            get
            {
                return this._underlyingListObject;
            }
            set
            {
                this._underlyingListObject = value;
            }
        }

        public void Add(object item)
        {
            IList iList = this.UnderlyingListObject as IList;

            //this is shwag. Why on earth would MS use a hashset<t> and not implement IList
            //I think I know the answer. Performance. What a joke
            if (this.UnderlyingListObject as IList != null)
            {
                ((IList)this.UnderlyingListObject).Add(item);
            }
            else if ((this.UnderlyingListObject.GetType().IsGenericType) &&
                     (this.UnderlyingListObject.GetType().GetGenericTypeDefinition() == typeof(HashSet<>)))
            {
                this.UnderlyingListObject.GetType().GetMethod("Add").Invoke(this.UnderlyingListObject, new Object[] { item });
            }
            else
            {
                throw new DataMapperException("The collection class does not support any known collection types. Unable to invoke 'Add' method on list.");
            }
        }

        public void Remove(object item)
        {
            IList iList = this.UnderlyingListObject as IList;

            //this is shwag. Why on earth would MS use a hashset<t> and not implement IList
            //I think I know the answer. Performance. What a joke
            if (this.UnderlyingListObject as IList != null)
            {
                ((IList)this.UnderlyingListObject).Add(item);
            }
            else if ((this.UnderlyingListObject.GetType().IsGenericType) &&
                     (this.UnderlyingListObject.GetType().GetGenericTypeDefinition() == typeof(HashSet<>)))
            {
                this.UnderlyingListObject.GetType().GetMethod("Remove").Invoke(this.UnderlyingListObject, new Object[] { item });
            }
            else
            {
                throw new DataMapperException("The collection class does not support any known collection types. Unable to invoke 'Remove' method on list.");
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)this.UnderlyingListObject).GetEnumerator();
        }

        public bool IsNull()
        {
            return this.UnderlyingListObject == null;
        }
    }
}
