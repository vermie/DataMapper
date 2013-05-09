using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Concurrent;
using System.IO;
using System.Data.Entity;

namespace DataMapper
{
    public static partial class EntityFrameworkExtensions
    {

        public static Boolean IsType<T>(this Object item)
            where T:class
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.GetType() == typeof(T);
        }

        public static Boolean AsType<T>(this Object item)
            where T : class
        {
            if (item == null)
            {
                return false;
            }

            return (item as T) != null;
        }

        //public static Boolean ImplementsInterface<T>(this Type info)
        //    where T: class
        //{
        //    return info.GetInterfaces().Where(a => a.Equals(typeof(T))).Count() > 0;
        //}

        //public static Boolean IsOrInheritsFrom(this Type info,Type matchType)
        //{
        //    return true;
        //}

        public static Object CreateInstance(this Type info)
        {
            return info.Assembly.CreateInstance(info.FullName, false);
        }

        public static T CopyUsingBinarySerialization<T>(this Object obj)
        {
            var byteBuffer = obj.SerializeBinaryToByteStream();

            return DeserializeBinaryByteStream<T>(byteBuffer);
        }
        public static Byte[] SerializeBinaryToByteStream(this Object item)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binary =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            using (var ms = new System.IO.MemoryStream())
            {
                binary.Serialize(ms, item);

                ms.Position = 0;
                Byte[] buffer = new Byte[ms.Length];

                ms.Read(buffer, 0, Convert.ToInt32(ms.Length));

                return buffer;
            }
        }
        public static T DeserializeBinaryByteStream<T>(Byte[] buffer)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binary =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            using (var ms = new MemoryStream(buffer))
            {
                return (T)binary.Deserialize(ms);
            }
        }


        public static void RemoveAll<T>(this IDbSet<T> dbSet, IEnumerable<T> entitiesToRemove) where T : class
        {
            entitiesToRemove.ToList().ForEach(a => dbSet.Remove(a));
        }

        public static T Create<T>(this IDbSet<T> dbSet, StateToCreateObjectsIn state) where T : class
        {
            switch (state)
            {
                case StateToCreateObjectsIn.Add:
                    return dbSet.CreateAndAdd<T>();
                case StateToCreateObjectsIn.Attached:
                    return dbSet.CreateAndAttach<T>();
                case StateToCreateObjectsIn.Unattached:
                    return dbSet.Create<T>();
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
        public static T CreateAndAdd<T>(this IDbSet<T> dbSet) where T : class
        {
            var item = dbSet.Create();
            dbSet.Add(item);
            return item;
        }
        public static T CreateAndAttach<T>(this IDbSet<T> dbSet) where T : class
        {
            var item = dbSet.Create();
            dbSet.Attach(item);
            return item;
        }


        public static Object Create(this DbSet dbSet, Type entityType, StateToCreateObjectsIn state)
        {
            switch (state)
            {
                case StateToCreateObjectsIn.Add:
                    return dbSet.CreateAndAdd(entityType);
                case StateToCreateObjectsIn.Attached:
                    return dbSet.CreateAndAttach(entityType);
                case StateToCreateObjectsIn.Unattached:
                    return dbSet.Create(entityType);
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
        public static Object CreateAndAdd(this DbSet dbSet, Type entityType)
        {
            var item = dbSet.Create();
            dbSet.Add(item);
            return item;
        }
        public static Object CreateAndAttach(this DbSet dbSet, Type entityType) 
        {
            var item = dbSet.Create();
            dbSet.Attach(item);
            return item;
        }     
    }

    public enum StateToCreateObjectsIn
    {
        Unattached,
        Add,
        Attached
    }
}
