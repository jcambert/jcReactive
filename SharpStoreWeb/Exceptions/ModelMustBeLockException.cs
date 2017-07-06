using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Exceptions
{
    public class ModelMustBeLockException:StoreException
    {
        public ModelMustBeLockException(List<string> messages, params string[] parameters) : base(messages, parameters)
        {

        }
        public ModelMustBeLockException(params string[] message):base(message)
        {

        }
    }
}