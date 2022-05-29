using System;
using System.Diagnostics;
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
            var sharp_path = Path.Combine(path, "LuaSTGEditorSharp.exe");
            key.SetValue("", $"\"{sharp_path}\" \"%1\"");

            key = Registry.ClassesRoot.CreateSubKey(".lstges", true);
            key.SetValue("", "LuaSTG.File");
            key = Registry.ClassesRoot.CreateSubKey(".lstgproj", true);
            key.SetValue("", "LuaSTG.Project");
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "mshta.exe",
                    Arguments = $"vbscript:msgbox(\"已成功注册扩展文件名(.lstges, .lstgproj)目标至{Environment.NewLine}{sharp_path}\",64,\"LuaSTG Sharp Editor\")(window.close)",
                    CreateNoWindow = true
                }
            };
            p.Start();
        }
    }
}
