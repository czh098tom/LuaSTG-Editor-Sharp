using System;
using System.Reflection;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Plugin
{
    public static class PluginHandler
    {
        private static AbstractPluginEntry plugin;

        public static AbstractPluginEntry Plugin { get => plugin; }

        public static bool LoadPlugin(string PluginPath)
        {
            if (!string.IsNullOrEmpty(PluginPath))
            {
                Assembly pluginAssembly = Assembly.LoadFile(PluginPath);
                plugin = (AbstractPluginEntry)pluginAssembly.CreateInstance("LuaSTGEditorSharp.PluginEntry");
                if (plugin == null)
                {
                    return false;
                }
                else
                {
                    plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly(), pluginAssembly);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
