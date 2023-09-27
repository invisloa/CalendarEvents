using CalendarT1.Models.EventModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;
using CalendarT1.ViewModels.EventsViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CalendarT1.Views
{
    public partial class EventPage : ContentPage
	{
		// For adding events
		public EventPage(IEventRepository eventRepository, DateTime selcetedDate)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, selcetedDate);
		}

		// For editing events
		public EventPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, eventToEdit: eventModel);

		}

	}
}
