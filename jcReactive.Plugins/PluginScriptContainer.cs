﻿using jcReactive.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jcReactive.Plugins
{
    public static class PluginExtensions
    {
        public class PluginScriptContainer : IPluginSnippetContainer
        {
            public string Name { get; set; }
            public string Script { get; set; }
            public string Version { get; set; }
        }

        public static IPluginSnippetContainer ToPluginScriptContainer(this Plugin p)
        {
            return new PluginScriptContainer()
            {
                Name = p.Name,
                Script = p.Script,
                Version = p.Version
            };
        }
    }

    public interface IPluginSnippetContainer
    {
        string Name { get; }
        string Script { get; }
        string Version { get; }
    }
}
