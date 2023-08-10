using CalendarT1.Models.EventModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CalendarT1.Views
{
    public partial class EventPage : ContentPage
	{
		public EventPage(IEventRepository eventRepository)
		{
			BindingContext = new EventViewModel(eventRepository);
			InitializeComponent();
		}

		// For adding events
		public EventPage(IEventRepository eventRepository, DateTime selcetedDate)
		{
			BindingContext = new EventViewModel(eventRepository, selcetedDate);
			InitializeComponent();
		}

		// For editing events
		public EventPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
		{
			BindingContext = new EventViewModel(eventRepository, eventToEdit: eventModel);
			InitializeComponent();
		}

	}
}
