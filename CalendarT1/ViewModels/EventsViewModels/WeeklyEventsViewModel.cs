using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventFactories;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class WeeklyEventsViewModel : AbstractEventViewModel
	{

		public WeeklyEventsViewModel
						(IEventRepository eventRepository, Dictionary<string, IBaseEventFactory> _eventFactories)
						: base(eventRepository, _eventFactories) { }

		public async override Task BindDataToScheduleList()
		{
			// Start of the Week
			var startOfWeek = CurrentSelectedDate.AddDays(-(int)CurrentSelectedDate.DayOfWeek);

			// End of the Week
			var endOfWeek = startOfWeek.AddDays(7);

			await ApplyEventFilter(startOfWeek, endOfWeek);

			OnOnEventsToShowListUpdated(); // TODO TO CHECK IF ITS NEEDED

		}
	}
}
