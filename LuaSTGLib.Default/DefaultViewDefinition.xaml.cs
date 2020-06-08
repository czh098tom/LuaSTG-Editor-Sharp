using System;
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
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.Plugin.Default
{
    /// <summary>
    /// DefaultViewDefinition.xaml 的交互逻辑
    /// </summary>
    public partial class DefaultViewDefinition : ViewDefinitionBase
    {
        public DefaultViewDefinition(DocumentData data)
        {
            this.data = data;
            InitializeComponent();
            InitializeTree();
            AllDef.ItemsSource = Tree;
        }
    }
}
