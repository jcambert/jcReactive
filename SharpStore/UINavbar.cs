using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    public class UINavbar:UIComponent
    {

        /// <summary>
        /// The Brand of this application
        /// </summary>
        [DataMember]
        public string Brand { get; set; }

        /// <summary>
        /// The icon of this application
        /// </summary>
        [DataMember]
        public string Icon { get; set; }
    }
}
