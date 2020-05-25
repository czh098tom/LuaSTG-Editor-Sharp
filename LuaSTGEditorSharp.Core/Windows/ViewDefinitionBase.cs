using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.Windows
{
    public class ViewDefinitionBase : Window, IViewDefinition
    {
        protected DocumentData data;

        protected ObservableCollection<MetaModel> Tree { get; set; } = new ObservableCollection<MetaModel>();

        public virtual void InitializeTree()
        {
            Tree.Add(GetUserDefinedMeta());
        }

        protected MetaModel GetUserDefinedMeta()
        {
            MetaModel userDefinedNode = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/userdefinednode.png",
                Text = "User Defined Nodes"
            };
            var a = data.Meta.aggregatableMetas[1].GetAllFullWithDifficulty("");
            foreach (MetaModel info in a)
            {
                userDefinedNode.Children.Add(info);
            }

            return userDefinedNode;
        }
    }
}
