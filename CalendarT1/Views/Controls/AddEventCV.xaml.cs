using CalendarT1.ViewModels;

namespace CalendarT1.Views.Controls;

public partial class AddEventCV : ContentView
{
	public AddEventCV()
	{
		InitializeComponent();
		BindingContext = new AddEventViewModel();
	}
}