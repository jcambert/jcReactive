using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    /// <summary>
    /// Represent a UI Web Component
    /// </summary>
    [ComplexType]
    [Serializable]
    public abstract class UIComponent
    {
        /// <summary>
        /// the order component
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// The Id of Component
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Classes that can be on Component
        /// </summary>
        [DataMember]
        public string  Classes { get; set; }

        /// <summary>
        /// Indicate if this component is a container
        /// </summary>
        [DataMember]
        public bool IsContainer { get; set; }

        /// <summary>
        /// UI Components Childs that can be hosted by parent (this)
        /// </summary>
        [DataMember]
        public virtual List<UIComponent> Childs { get; set; }

        [DataMember]
        public virtual string TypeName { get { return this.GetType().FullName; } set { } }
    }
}
