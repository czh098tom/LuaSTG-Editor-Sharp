using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LuaSTGEditorSharp.EditorData
{
    public class EditorTypeBinder : ISerializationBinder
    {
        DefaultSerializationBinder @default = new DefaultSerializationBinder();

        private string asmFullName;
        public string AsmFullName
        {
            get
            {
                if (asmFullName == null)
                {
                    asmFullName = Plugin.PluginHandler.Plugin.GetType().Assembly.FullName;
                }
                return asmFullName;
            }
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (serializedType == typeof(DependencyAttrItem) || serializedType == typeof(AttrItem))
            {
                assemblyName = "";
                typeName = $".{serializedType.Name}";
            }
            else
            {
                @default.BindToName(serializedType, out assemblyName, out typeName);
                string sa = assemblyName, st = typeName;
                string asmName = serializedType.Assembly.GetName().Name;
                //App.Current.Dispatcher.Invoke(() => System.Windows.Forms.MessageBox.Show($"To Name: asm: {sa} type: {st}"));
                typeName = typeName.Substring(33);
                if (asmName == "LuaSTGEditorSharp")
                {
                    assemblyName = "LuaSTGEditorSharp";
                }
                else
                {
                    assemblyName = "";
                }
            }
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            if(typeName.EndsWith(".AttrItem"))
            {
                return typeof(AttrItem);
            }
            else if(typeName.EndsWith(".DependencyAttrItem"))
            {
                return typeof(DependencyAttrItem);
            }
            string s = assemblyName == "LuaSTGEditorSharp" ? assemblyName : AsmFullName;
            if (!typeName.StartsWith("LuaSTGEditorSharp.EditorData.Node.")) typeName = $"LuaSTGEditorSharp.EditorData.Node{typeName}";
            //App.Current.Dispatcher.Invoke(() => System.Windows.Forms.MessageBox.Show($"To Type: asm: {s} type: {typeName}"));
            Type t = @default.BindToType(s, typeName);
            //App.Current.Dispatcher.Invoke(() => System.Windows.Forms.MessageBox.Show($"Type: {t.FullName}"));
            return t;
        }
    }
}
