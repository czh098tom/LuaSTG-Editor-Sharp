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
        public static AbstractPluginEntry Plugin { get; private set; } = null;

        public static bool LoadPlugin(string PluginPath)
        {
            bool isSuccess;
            Assembly pluginAssembly = null;
            try
            {
                string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginPath));
                pluginAssembly = Assembly.LoadFile(path);
                Plugin = (AbstractPluginEntry)pluginAssembly.CreateInstance("LuaSTGEditorSharp.PluginEntry");
            }
            catch { }
            if (Plugin == null)
            {
                Plugin = new DefaultNullPlugin.DefaultPluginEntry();
                Plugin.NodeTypeCache.Initialize(Assembly.GetExecutingAssembly());
                isSuccess = false;
            }
            else
            {
                Plugin.NodeTypeCache.Initialize(AppDomain.CurrentDomain.GetAssemblies());
                isSuccess = true;
            }
            EditorData.InputWindowSelector.Register(Plugin.GetInputWindowSelectorRegister());
            EditorData.InputWindowSelector.AfterRegister();
            return isSuccess;
        }
    }
}
