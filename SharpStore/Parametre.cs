using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{
    /// <summary>
    /// Represent a eShop Parameter
    /// </summary>
    [DataContract]
    public class Parametre : StoreModel, IParametre
    {
        /// <summary>
        /// Numero du parametre
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [Range(1,999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 2)]
        [Column("PAKTNOPAR")]
        public int Numero { get ; set ; }

        /// <summary>
        /// Code du parametre
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [MinLength(1, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Localizations))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 3)]
        [Column("PAKTCODE")]
        public string Code { get ; set ; }

        /// <summary>
        /// Description du parametre
        /// </summary>
        [DataMember]
        [Column("PACTDESC")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [MinLength(1, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Localizations))]
        [MaxLength(24, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        public string Description { get ; set ; }
    }
}
