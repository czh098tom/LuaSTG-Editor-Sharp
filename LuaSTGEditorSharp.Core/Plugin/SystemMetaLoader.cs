using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.Plugin
{
    public class SystemMetaLoader
    {
        public static MetaModel[] FromResource(string path)
        {
            MetaModel[] mm = new MetaModel[1];
            StreamReader sr = null;
            try
            {
                Uri uri = new Uri(path, UriKind.Absolute);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                sr = new StreamReader(info.Stream);
                string s = sr.ReadToEnd();
                mm = JsonConvert.DeserializeObject<MetaModel[]>(s);
                //Console.WriteLine(s);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            return mm;
        }
    }
}
