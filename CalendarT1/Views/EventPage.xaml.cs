using CalendarT1.Models.EventModels;
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
		public EventPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
		{
			BindingContext = new EventViewModel(eventRepository, eventToEdit: eventModel);
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			// nie wiem co naraazie
			await (BindingContext as EventViewModel).OnApearing();
		}
	}
}
