using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VersionController.Controls.Converter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool boolValue = false;

                if (value is bool)
                {
                    boolValue = (bool)value;
                }
                else if (value is bool?)
                {
                    boolValue = (bool?)value ?? false;
                }

                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Unable to convert at {nameof(VisibilityConverter)}: {ex.Message}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility ? (Visibility)value == Visibility.Visible : false;
        }
    }
}
