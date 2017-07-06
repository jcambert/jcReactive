using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Plugins.Models
{
    public class Plugin
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Script { get; set; }
    }

}
