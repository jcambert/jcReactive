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
    /// Represent Menu for front-end application
    /// </summary>
    [DataContract]
    public class Menu:StoreModel, IMenu
    {
        /// <summary>
        /// The code of the product
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
       // [MinLength(5, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Localizations))]
       /// [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 2)]
        [Column("MEKTCODMEN")]
        public string Code { get; set; }

        /// <summary>
        /// Tolltip for this item menu
        /// </summary>
        [DataMember]
        public string Tooltip { get; set; }

        /// <summary>
        /// Tooltip placement
        /// <see cref="https://angular-ui.github.io/bootstrap/#!#tooltip"/>
        /// </summary>
        [DataMember]
        public string TooltipPlacement { get; set; }

        /// <summary>
        /// The Font awesome icon for item menu
        /// <see cref="http://fontawesome.io/icons/"/>
        /// </summary>
        [DataMember]
        public string Icon { get; set; }

        /// <summary>
        /// The animation for item menu
        /// <see cref="http://l-lin.github.io/font-awesome-animation/"/>
        /// </summary>
        [DataMember]
        public string Animation { get; set; }

        /// <summary>
        /// Name of action 
        /// <see cref="https://ui-router.github.io/ng1/"/>
        /// </summary>
        [DataMember]
        public string Action { get; set; }
    }
}
