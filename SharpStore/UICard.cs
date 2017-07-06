using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    [ComplexType]
    public class UICards:UIComponent
    {
        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public UICardHeader Header { get; set; }

        [DataMember]
        public string RepeatOn { get; set; }
        
    }

    [ComplexType]
    public class UICardHeader
    {
        [DataMember]
        public string  Color { get; set; }

       // public string Text { get; set; }
    }

    [ComplexType]
    public class UICardContent
    {


    }
}
