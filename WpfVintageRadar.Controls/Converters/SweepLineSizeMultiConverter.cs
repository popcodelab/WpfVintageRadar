using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfVintageRadar.Controls.Converters
{
    [MarkupExtensionReturnType(typeof(SweepLineSizeMultiConverter))]
    internal class SweepLineSizeMultiConverter : IMultiValueConverter
    {


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parentWidth = (double)values[0];
            var edgeThickness = (int)values[1];
            var result =  (parentWidth/ 2) - (double)edgeThickness;
            //Debug.WriteLine($"====>parentWidth : {parentWidth}  edgeThickness : {edgeThickness} result = {result}");
            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null ;
        }
    }
}
