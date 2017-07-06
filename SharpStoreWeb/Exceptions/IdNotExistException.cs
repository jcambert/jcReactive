using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Exceptions
{
    public class IdNotExistException:StoreException
    {

      
        public IdNotExistException(params string[] message):base(message)
        {

        }

        public IdNotExistException(List<string> messages, params string[] parameters) :base(messages,parameters)
        {

        }
    }
}