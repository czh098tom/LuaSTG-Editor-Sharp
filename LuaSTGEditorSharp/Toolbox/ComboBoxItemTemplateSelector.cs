using System.Windows;
using System.Windows.Controls;

namespace LuaSTGEditorSharp.Toolbox
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DropDownTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ComboBoxItem comboBoxItem = container.GetVisualParent<ComboBoxItem>();
            if (comboBoxItem == null)
            {
                return null;
            }
            return DropDownTemplate;
        }
    }
}
