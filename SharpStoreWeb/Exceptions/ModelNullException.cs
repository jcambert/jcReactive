﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb.Exceptions
{
    public class ModelNullException:StoreException
    {
        public ModelNullException(List<string> messages, params string[] parameters) : base(messages, parameters)
        {

        }
        public ModelNullException(params string[] messages):base(messages)
        {

        }
    }
}