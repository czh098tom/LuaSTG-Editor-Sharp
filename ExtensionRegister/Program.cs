using System;
using System.IO;
using Microsoft.Win32;

namespace ExtensionRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            RegistryKey key = Registry.ClassesRoot.CreateSubKey("LuaSTG.File", true);
            key.CreateSubKey("DefaultIcon").SetValue("", Path.Combine(path, "File.ico"));
            key = key.CreateSubKey("shell", true);
            key.SetValue("", "open");
            key = key.CreateSubKey("open", true);
            key.SetValue("", "&Open");
            key = key.CreateSubKey("command", true);
            key.SetValue("", $"\"{Path.Combine(path, "LuaSTGEditorSharp.exe")}\" \"%1\"");

            key = Registry.ClassesRoot.CreateSubKey("LuaSTG.Project");
            key.CreateSubKey("DefaultIcon").SetValue("", Path.Combine(path, "File.ico"));
            key = key.CreateSubKey("shell", true);
            key.SetValue("", "open");
            key = key.CreateSubKey("open", true);
            key.SetValue("", "&Open");
            key = key.CreateSubKey("command", true);
            key.SetValue("", $"\"{Path.Combine(path, "LuaSTGEditorSharp.exe")}\" \"%1\"");

            key = Registry.ClassesRoot.CreateSubKey(".lstges", true);
            key.SetValue("", "LuaSTG.File");
            key = Registry.ClassesRoot.CreateSubKey(".lstgproj", true);
            key.SetValue("", "LuaSTG.Project");
        }
    }
}
