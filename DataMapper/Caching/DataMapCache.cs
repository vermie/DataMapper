using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Mapping;

namespace DataMapper.Caching
{

    public class DataMapCache
    {
        private static DataMapCache Cache
        {
            get;
            set;
        }
        public static DataMapCache Instance
        {
            get
            {
                return Cache;
            }
        }

         static DataMapCache()
        {
            //forgot the singleton bs. This is safe because a type constructor is called once and is threadsafe.
            Cache = new DataMapCache();
        }

        private Object _synchronizingObject = new object();
        private Dictionary<String,DataMap> Dictionary
        {
            get;
            set;
        }

        private DataMapCache()
        {
            this.Dictionary = new Dictionary<string, DataMap>();
        }

        public void AddItem(String key, DataMap dataMap)
        {
            lock (this._synchronizingObject)
            {
                if (this.Dictionary.ContainsKey(key) == true)
                {
                    this.Dictionary.Remove(key);
                }

                this.Dictionary.Add(key, dataMap);
            }
        }
        public DataMap TryFind(String key)
        {
            lock (this._synchronizingObject)
            {
                DataMap dataMap;

                return this.Dictionary.TryGetValue(key, out dataMap) ? dataMap : null;
            }
        }
    }
}
