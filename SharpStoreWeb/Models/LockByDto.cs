using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Models
{
    public class LockByDto
    {

        public bool IsLocked => LockBy != null;

        public string LockBy { get; set; }

        public DateTime? LockAt { get; set; }
    }
}