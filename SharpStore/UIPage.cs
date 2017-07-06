using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpStore
{

    [DataContract]
    public class UIPage : StoreModel, IUIPage
    {



        protected override void Initialize()
        {
            base.Initialize();

            this.Added.Subscribe(page =>
            {
                if (Navbar == null)
                    Navbar = new UINavbar();
            });
        }
        /// <summary>
        ///  Unique code of this page
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [DataMember]
        [Index(name: "IX_KEY", IsUnique = true, Order = 2)]
        public string Code { get; set; }

        /// <summary>
        /// The title of this page
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Component in this page
        /// </summary>
        [DataMember]
        public virtual PersistableUICollection Childs { get; set; } = new PersistableUICollection();


        /// <summary>
        /// Views inside this Page
        /// </summary>
       // [DataMember]
       // public List<UIView> Views { get; set; }

        /// <summary>
        /// Navbar of this page
        /// </summary>
        [DataMember]
        public UINavbar Navbar { get; set; }
    }
}
