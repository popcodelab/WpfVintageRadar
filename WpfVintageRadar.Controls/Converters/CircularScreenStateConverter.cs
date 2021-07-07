using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfVintageRadar.Controls.Converters
{
    internal class CircularScreenStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case bool isOn when isOn:
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#000000"));
                case bool isOn:
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9C9C9C"));
                default:
                    return  (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9C9C9C"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
