using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace LuaSTGEditorSharp.Lua
{
    public static class SyntaxHighlightLoader
    {
        //public static IHighlightingDefinition LuaSyntax { get; private set; }
        public static void LoadLuaDef()
        {
            IHighlightingDefinition LuaSyntax;
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("LuaSTGEditorSharp.Lua.LuaSyntax.xml"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    LuaSyntax = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("Lua", new string[] { ".lua" }, LuaSyntax);
        }
    }
}
