using System;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Plugin
{
    public static class PluginHandler
    {
        public static AbstractPluginEntry Plugin { get; private set; } = new DefaultNullPlugin.DefaultPluginEntry();

        public static bool LoadPlugin(string PluginPath)
        {
            Assembly pluginAssembly = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginPath));
                pluginAssembly = Assembly.LoadFile(path);
                Plugin = (AbstractPluginEntry)pluginAssembly.CreateInstance("LuaSTGEditorSharp.PluginEntry");
            }
            catch { }
            if (pluginAssembly == null)
            {
                if (Plugin == null) Plugin = new DefaultNullPlugin.DefaultPluginEntry();
                Plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly());
                return false;
            }
            else
            {
                Plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly(), pluginAssembly);
                return true;
            }
        }
    }
}
