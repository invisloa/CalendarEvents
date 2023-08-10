using CalendarT1.Views;
using System.Globalization;

namespace CalendarT1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		CultureInfo.CurrentCulture = new CultureInfo("pl-PL", false);
		CultureInfo.CurrentUICulture = new CultureInfo("pl-PL", false);

		// REGISTER ROUTING

		// Change the first day of the week to Monday
		CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
		InitializeComponent();
	}
}
