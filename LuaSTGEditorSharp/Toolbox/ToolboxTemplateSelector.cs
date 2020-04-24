using LuaSTGEditorSharp.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LuaSTGEditorSharp.Toolbox
{
    internal class ToolboxContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var obj = (ToolboxItemData)item;
            DataTemplate dt = null;
            if (container is FrameworkElement fe)
            {
                if (obj.IsSeperator)
                    dt = fe.FindResource("ToolboxSeperator") as DataTemplate;
                else
                    dt = fe.FindResource("ToolboxButton") as DataTemplate;

            }
            return dt;
        }
    }

    internal class ToolboxTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var obj = item;
            DataTemplate dt = null;
            if (container is FrameworkElement fe)
            {
                if (obj is ToolboxTab)
                    dt = fe.FindResource("ToolboxContent") as DataTemplate;
                else
                    dt = null;

            }
            return dt;
        }
    }
}
