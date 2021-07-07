using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfVintageRadar.Converters
{
    public class CircularScreenStateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (parameter != null)
                {
                    switch (parameter)
                    {
                        case "State":
                            switch (value)
                            {
                                case bool isOn when isOn:
                                    return "Switch Off";
                                case bool isOn:
                                    return "Switch On";
                                default:
                                    return "Switch On";
                            }

                        case "DetectionState":
                            switch (value)
                            {
                                case bool isOn when isOn:
                                    return "Detection Off";
                                case bool isOn:
                                    return "Detection On";
                                default:
                                    return "Detection On";
                            }
                    }
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
