using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SharpStore
{
    [ComplexType]
    public class PersistableUICollection : PersistableScalarCollection<UIComponent>
    {
        protected override string ConvertSingleValueToPersistable(UIComponent value)
        {
            return JsonConvert.SerializeObject(value);
        }

        protected override UIComponent ConvertSingleValueToRuntime(string rawValue)
        {
            string pattern= "(\"TypeName\":\")+(\\w*.\\w*)(\")";
            var s = Regex.Split(rawValue, pattern);
            var type = s[2];
            var meth = typeof(JsonConvert).GetMethods().Where(m=>m.IsGenericMethod && m.IsPublic && m.IsStatic).FirstOrDefault();
            var genmeth=meth.MakeGenericMethod(new Type[] { Type.GetType(type) });


            var result = genmeth.Invoke(null, new string[] { rawValue });
            return result as UIComponent;
        //    return JsonConvert.DeserializeObject<UIComponent>(rawValue);
        }

        
    }
}
