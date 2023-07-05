using CalendarT1.Models;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views;

public partial class AddEventPage : ContentPage
{
	public AddEventPage()
	{
		BindingContext = new AddEventViewModel();
		InitializeComponent();
	}
}