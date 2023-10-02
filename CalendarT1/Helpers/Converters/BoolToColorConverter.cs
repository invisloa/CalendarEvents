using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CalendarT1.Helpers.Converters
{
	public class BoolToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isCompleted && isCompleted)
			{
				return Application.Current.Resources.ContainsKey("MainSubTaskBackgroundColor");
			}
			return Application.Current.Resources.ContainsKey("DeselectedBackgroundColor");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}