using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Common
{
    /// <summary>
    /// Mark for attribute that is reactive 
    /// and will be transformed by fody
    /// to add Property Changing and Changed event
    /// availables
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReactiveAttribute : Attribute
    {
    }
}
