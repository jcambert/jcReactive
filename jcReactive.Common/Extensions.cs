using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    public static class Extensions
    {
        public static bool IsNull(this object o) => o == null;

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static bool Exists<TContext, TEntity>(this TContext context, TEntity entity)
            where TContext : ReactiveDbContext
            where TEntity : ReactiveDbObject
        {
            return context.Set<TEntity>().Local.Any(e => e == entity);
        }

        public static string Fmt(this string s, params string[] parameters)
        {
            try
            {
                return string.Format(s, parameters);
            }
            catch (Exception)
            {
                return $"Warning: The number of parameter are not equal\n{parameters.Length} are passed \nCheck below: {s}";
            }
        }
        
    }
}
