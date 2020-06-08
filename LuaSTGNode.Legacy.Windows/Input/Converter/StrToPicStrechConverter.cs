using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace LuaSTGEditorSharp.Windows.Input.Converter
{
    class StrToPicStrechConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            switch (s)
            {
                case "Fill":
                    return Stretch.Uniform;
                case "Original":
                default:
                    return Stretch.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
