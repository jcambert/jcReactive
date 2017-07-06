using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using jcReactive.Common;
using System.Reactive;
namespace SharpStore
{
    /// <summary>
    /// Represent a eShop Product
    /// </summary>
    [DataContract]
    public class Product:StoreModel,IProduct
    {
        public Product()
        {
            this.Adding.Subscribe(p =>
            {
                if (Complementaire.IsNull()) Complementaire = string.Empty;
                if (Libelle.IsNull()) Libelle = string.Empty;
            });
        }
      
        /// <summary>
        /// The code of the product
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [MinLength(5,ErrorMessageResourceName ="MinLength",ErrorMessageResourceType =typeof(Localizations))]
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique =true,Order =2)]
        [Column("ARKTCODART")]
        public string Code { get; set; }


        /// <summary>
        /// The complementary Key of the product
        /// </summary
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [DefaultValue("")]
        [Index(name: "IX_KEY", IsUnique = true, Order = 3)]
        [Column("ARKTCOMART")]
        public string Complementaire { get; set; }

        /// <summary>
        /// The libelle of the product
        /// </summary>
        [MaxLength(30, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [DefaultValue("")]
        [Column("ARCTLIB01")]
        public string Libelle { get; set; }

        /// <summary>
        /// Unite de commande
        /// </summary>
        public string UniteDeCommande  { get; set; }

        /// <summary>
        /// Unite de Stockage
        /// </summary>
        public string UniteDeStockage { get; set; }


        /// <summary>
        /// Unite de Facturation
        /// </summary>
        public string UniteDeFacturation { get; set; }

    }
}
