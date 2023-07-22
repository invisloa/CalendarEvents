using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views
{
	public partial class EventPage : ContentPage
	{
		// For adding events
		public EventPage(IEventRepository eventRepository)
		{
			BindingContext = new EventViewModel(eventRepository);
			InitializeComponent();
		}

		// For editing events
		public EventPage(IEventRepository eventRepository, EventModel eventModel)
		{
			BindingContext = new EventViewModel(eventRepository, eventModel);
			InitializeComponent();
		}
	}
}
