using SharpStore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Models
{
    public class ParametreDto
    {
        [ReadOnly(true)]
        public Guid Key { get; set; }

        [ReadOnly(true)]
        public string Societe { get; set; }

        [ReadOnly(true)]
        public int Numero { get; set; }

        [ReadOnly(true)]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Localizations))]
        [MinLength(1, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Localizations))]
        [MaxLength(24, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Localizations))]
        public string Description { get; set; }
    }
}