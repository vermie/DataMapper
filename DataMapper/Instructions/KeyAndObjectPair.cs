using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper.Instructions
{
    [Serializable()]
    internal class KeyAndObjectPair<TKey>
    {
        public TKey Key
        {
            get;
            private set;
        }
        public Object Value
        {
            get;
            private set;
        }

        public KeyAndObjectPair(TKey key, Object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    [Serializable()]
    public class CompositeKey
    {
        private List<Tuple<Type, Object>> KeyValues
        {
            get;
            set;
        }

        public CompositeKey()
        {
            this.KeyValues = new List<Tuple<Type, object>>();
        }

        public void AddKey(Type type, Object key)
        {
            this.KeyValues.Add(new Tuple<Type, object>(type, key));
        }

        public override int GetHashCode()
        {
            foreach(var item in this.KeyValues)
            {
                if (item.Item2 != null)
                {
                    return item.Item2.GetHashCode();
                }
            }

            return 0;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj as CompositeKey == null))
            {
                return false;
            }

            return this.Equals(obj as CompositeKey);
        }
        private Boolean Equals(CompositeKey otherKey)
        {
            if (this.KeyValues.Count != otherKey.KeyValues.Count)
            {
                throw new DataMapperException("Cannot compare composite keys because the keys are of different lengths.");
            }

            //now check each key
            for (int i = 0; i < this.KeyValues.Count; i++)
            {
                if (this.SingleKeyMatches(this.KeyValues[i], otherKey.KeyValues[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
        private Boolean SingleKeyMatches(Tuple<Type,Object> firstItem, Tuple<Type,Object> secondItem)
        {
            //if (firstItem.Item1 != secondItem.Item1)
            //{
            //    throw new DataMapperException("Cannot compare items because their ");
            //}

            //value type equality check
            if (firstItem.Item1.IsValueType)
            {
                return firstItem.Item2.Equals(secondItem.Item2);
            }

            //null equality check
            if (firstItem.Item2 == null)
            {
                return secondItem.Item2 == null;
            }

            return firstItem.Item2.Equals(secondItem.Item2);
        }
    }

}
