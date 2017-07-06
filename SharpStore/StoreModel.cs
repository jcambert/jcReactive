using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jcReactive.Common;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpStore
{
    public class StoreModel : ReactiveDbObject, IStoreModel
    {

        public StoreModel()
        {
            
        }
        /// <summary>
        /// Unique Guid Key for ths StoreModel Object
        /// </summary>
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public Guid Key { get ; set ; }

        /// <summary>
        /// The Code of societe where are working about
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        //[RegularExpression(@"^[0 - 9]{3}",ErrorMessageResourceName = "Societe", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 1)]
        public string Societe { get; set; }

        /// <summary>
        /// Indicate if a store model entity is lock by an user
        /// Only this user can Unlock or Save this Store Model
        /// </summary>
        [DataMember]
        public string LockBy { get ; set; }

        /// <summary>
        /// the date time when this object is locked
        /// </summary>
        [DataMember]
        public DateTime? LockAt { get ; set; }

        /// <summary>
        /// The name of creator
        /// </summary>
        [DataMember]
        public string CreatedBy { get ; set ; }

        /// <summary>
        ///  the date time when store model is created
        /// </summary>
        [DataMember]
        public DateTime? CreatedAt { get ; set ; }
        /// <summary>
        /// The name of last modifier
        /// </summary>
        [DataMember]
        public string ModifiedBy { get ; set; }
        /// <summary>
        ///  the date time when store model is modified for the last time
        /// </summary>
        [DataMember]
        public DateTime? ModifiedAt { get ; set ; }
    }
}
