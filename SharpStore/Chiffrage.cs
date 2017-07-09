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
    /// Represents a quotation head Model
    /// </summary>
    [DataContract]
    public class Chiffrage:StoreModel
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [Range(0,int.MaxValue,ErrorMessageResourceName ="MinValue", ErrorMessageResourceType =typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 2)]
        [Column("CHKTNUM")]
        public int Numero { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "MinValue", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 3)]
        [Column("CHKTAVEN")]
        public int  Avenant { get; set; }

        /// <summary>
        /// Articles presents dans ce chiffrages
        /// </summary>
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
