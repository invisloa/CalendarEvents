using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views;

public partial class EditEventPage : ContentPage
{
	public EditEventPage(IEventRepository eventRepository, EventModel eventModel)
	{
		BindingContext = new EditEventViewModel(eventRepository, eventModel);
		InitializeComponent();
	}
}