using CalendarT1.Models;
using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class EditEventPage : ContentPage
{
	public EditEventPage(EventModel eventModel)
	{
		BindingContext = new EditEventViewModel(eventModel);
		InitializeComponent();
	}
}