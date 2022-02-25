using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySave.view.wpf.core;

/**
 * Convert a progression (from 0 to 100%) to a visibility object. Used in WPF xaml.
 */
public class ProgressionToVisibilityConverter : IValueConverter
{
    public Visibility TrueValue { get; set; }
    public Visibility FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var progression = (int) value;

        return progression == 0 ? TrueValue : FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (Visibility) value == TrueValue ? 0 : 100;
    }
}