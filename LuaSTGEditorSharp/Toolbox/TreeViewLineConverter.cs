using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Toolbox
{
    public class TreeViewLineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Rectangle rectangle && values[1] is TreeViewItem item)
            {
                ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
                if (ic != null && ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1)
                {
                    rectangle.VerticalAlignment = VerticalAlignment.Top;
                    return 9.0;
                }
                else
                {
                    rectangle.VerticalAlignment = VerticalAlignment.Stretch;
                    return double.NaN;
                }
            }
            return double.NaN;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
