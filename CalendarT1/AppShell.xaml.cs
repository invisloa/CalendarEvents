using CalendarT1.Views;
using System.Globalization;

namespace CalendarT1;

public partial class AppShell : Shell
{
	public AppShell()
	{

		// REGISTER ROUTING
		Routing.RegisterRoute(nameof(AddNewSubTypePage), typeof(AddNewSubTypePage));
		Routing.RegisterRoute(nameof(AllSubTypesPage), typeof(AllSubTypesPage));



		InitializeComponent();
	}
}
