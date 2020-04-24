using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LuaSTGEditorSharp.EditorData
{
    public static class EditorSerializer
    {
        public static readonly JsonSerializerSettings TreeNodeSettings =
            new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                SerializationBinder = new EditorTypeBinder()
            };

        public static readonly JsonSerializerSettings MetaSettings =
            new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            };

        public static string SerializeTreeNode(object o)
        {
            return JsonConvert.SerializeObject(o, typeof(TreeNode), TreeNodeSettings);
        }

        public static object DeserializeTreeNode(string s)
        {
            return JsonConvert.DeserializeObject(s, typeof(TreeNode), TreeNodeSettings);
        }

        public static string SerializeMetaData(object o)
        {
            return JsonConvert.SerializeObject(o, typeof(AbstractMetaData), MetaSettings);
        }

        public static object DeserializeMetaData(string s)
        {
            return JsonConvert.DeserializeObject(s, typeof(AbstractMetaData), MetaSettings);
        }
    }
}
