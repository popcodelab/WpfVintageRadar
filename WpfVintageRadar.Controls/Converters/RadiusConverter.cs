using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfVintageRadar.Controls.Converters
{
    /// <summary>
    /// Convert the diameter value into radius
    /// </summary>
    [MarkupExtensionReturnType(typeof(RadiusConverter))]
    internal class RadiusConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return (int) value / 2;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

        
    }
}
