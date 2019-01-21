using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LuaSTGEditorSharp.Toolbox
{
    public class BannedToStrikeThrough : IValueConverter
    {
        //Bool2Strike
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(bool.TryParse(value.ToString(), out bool b))
            {
                if (b)
                {
                    return TextDecorations.Strikethrough;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == TextDecorations.Strikethrough;
        }
    }
}
