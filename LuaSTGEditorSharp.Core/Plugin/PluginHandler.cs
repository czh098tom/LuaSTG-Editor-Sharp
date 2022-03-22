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
        public static AbstractPluginEntry DefaultPlugin { get; set; }
        public static AbstractPluginEntry Plugin { get; private set; } = null;

        public static Exception LoadPlugin(string PluginPath)
        {
            Exception isSuccess = null;
            Assembly pluginAssembly = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginPath));
                pluginAssembly = Assembly.LoadFrom(path);
                Plugin = (AbstractPluginEntry)pluginAssembly.CreateInstance("LuaSTGEditorSharp.PluginEntry");
            }
            catch (Exception ex) 
            {
                isSuccess = ex;
            }
            if (Plugin == null)
            {
                Plugin = DefaultPlugin;
                Plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly());
            }
            else
            {
                Plugin.NodeTypeCache.Initialize(AppDomain.CurrentDomain.GetAssemblies());
            }
            return isSuccess;
        }
    }
}
