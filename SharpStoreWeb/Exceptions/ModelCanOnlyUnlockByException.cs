using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Exceptions
{
    public class ModelCanOnlyUnlockByException:StoreException
    {
        public ModelCanOnlyUnlockByException(List<string> messages, params string[] parameters) : base(messages, parameters)
        {

        }
        public ModelCanOnlyUnlockByException(params string[] message):base(message)
        {

        }
    }
}