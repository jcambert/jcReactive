using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace jcReactive.Common
{
    public class Index
    {
        public IndexAttribute Attribute { get; internal set; }
        public PropertyInfo Property { get; internal set; }
        public string Name => Attribute.Name;
    }
    public class Indexes : SortedList<int, Index> { }

    public static class ObjectMixins
    {
        internal static PropertyInfo GetKey<T>() where T : class, IReactiveDbObject, new()
        {
            if (!typeof(T).HasDataContractAttribute())
                throw ReactiveDbException.NoDataContractSpecified;

            var properties = typeof(T).GetProperties();

            var result = properties.Where(property =>
                       property.GetCustomAttributes(false)
                               .OfType<KeyAttribute>()
                               .Any()
                     ).ToList().FirstOrDefault();
            if (result == null)
                throw ReactiveDbException.NoKeySpecified;
            return result;
        }

        internal static Dictionary<string, Indexes> GetIndexes<T>() where T : class, IReactiveDbObject, new()
        {
            var res = new Dictionary<string, Indexes>();

            var properties = typeof(T).GetProperties();

            var result = properties.Where(property =>
                       property.GetCustomAttributes(true)
                               .OfType<IndexAttribute>()
                               .Any()
                     ).ToList();

            result.ForEach(prop =>
            {
                prop.GetCustomAttributes<IndexAttribute>().ToList().ForEach(attr =>
                {
                    try
                    {
                        if (!res.ContainsKey(attr.Name))
                            res[attr.Name] = new Indexes();
                        res[attr.Name].Add(attr.Order, new Index() { Attribute = attr, Property = prop });
                    }catch(Exception e)
                    {

                    }
                });
            });
            return res;
        }

        public static bool HasDataContractAttribute(this Type t)
        {
            return t.HasAttribute<DataContractAttribute>() != null;
        }

        public static TAttr HasAttribute<TAttr>(this Type t) where TAttr : Attribute
        {
            try
            {

                return Attribute.GetCustomAttribute(t, typeof(TAttr), true) as TAttr;

            }
            catch
            {
                return null;
            }

        }

    }
}