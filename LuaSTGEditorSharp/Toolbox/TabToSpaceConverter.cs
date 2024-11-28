using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Toolbox
{
    public class TabToSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string text) return value;
            var tabSize = (Application.Current as App)?.TabDisplayWidth ?? 4;
            var result = text.Replace("\t", new(' ', tabSize));
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}