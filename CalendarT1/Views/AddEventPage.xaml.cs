using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views;

public partial class AddEventPage : ContentPage
{
	public AddEventPage(IEventRepository eventRepository)
	{
		BindingContext = new AddEventViewModel(eventRepository);
		InitializeComponent();
	}
}