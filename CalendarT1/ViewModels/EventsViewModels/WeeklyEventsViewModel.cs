using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class WeeklyEventsViewModel : AbstractEventViewModel
	{

		public WeeklyEventsViewModel
						(IEventRepository eventRepository)
						: base(eventRepository) { }

		public override void BindDataToScheduleList()
		{
			// Start of the Week
			var startOfWeek = CurrentSelectedDate.AddDays(-(int)CurrentSelectedDate.DayOfWeek);

			// End of the Week
			var endOfWeek = startOfWeek.AddDays(7);

			ApplyEventsDatesFilter(startOfWeek, endOfWeek);

			OnOnEventsToShowListUpdated(); // TODO TO CHECK IF ITS NEEDED

		}
	}
}
