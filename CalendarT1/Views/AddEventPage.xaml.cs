using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AddEventPage : ContentPage
{
	public AddEventPage()
	{
		InitializeComponent();
		BindingContext = new AddEventCVViewModel();

	}
}