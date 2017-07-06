using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Models
{
    public class InformationDto
    {
        /// <summary>
        /// The name of creator
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        ///  the date time when store model is created
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// The name of last modifier
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        ///  the date time when store model is modified for the last time
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
    }
}