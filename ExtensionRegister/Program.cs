using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ExtensionRegister
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

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

            // 使用 Win32 API 弹窗
            MessageBoxW(IntPtr.Zero,
                $"已成功注册扩展文件名(.lstges, .lstgproj)目标至\n{sharp_path}",
                "LuaSTG Sharp Editor",
                0x40); // 0x40 = MB_ICONINFORMATION
        }
    }
}
