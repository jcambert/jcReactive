﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Plugins.Models
{
    public interface IPluginSource
    {
        List<Plugin> Plugins { get; }
    }
}
