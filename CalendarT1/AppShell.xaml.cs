using CalendarT1.Views;
using System.Globalization;

namespace CalendarT1;

public partial class AppShell : Shell
{
	public AppShell()
	{

		// REGISTER ROUTING
		Routing.RegisterRoute(nameof(AddNewTypePage), typeof(AddNewTypePage));
		Routing.RegisterRoute(nameof(AllTypesPage), typeof(AllTypesPage));



		InitializeComponent();
	}
}
