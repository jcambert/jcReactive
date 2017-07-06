using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    public class UIDropdown:UIComponent
    {
        /// <summary>
        /// Only UiDropdown Item can be added
        /// </summary>
        [DataMember]
        public new List<UIDropdownItem> Childs { get; set; }

        
    }
}
