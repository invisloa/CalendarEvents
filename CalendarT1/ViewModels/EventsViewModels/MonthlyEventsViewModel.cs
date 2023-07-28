using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventFactories;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class MonthlyEventsViewModel : AbstractEventViewModel
	{

		public MonthlyEventsViewModel
						(IEventRepository eventRepository, Dictionary<string, IBaseEventFactory> _eventFactories)
						: base(eventRepository, _eventFactories) { }
		public async override Task BindDataToScheduleList()
		{
			// Start of the month
			var startOfMonth = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1);

			// End of the month
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			await ApplyEventFilter(startOfMonth, endOfMonth);

			OnOnEventsToShowListUpdated(); // TODO TO CHECK IF ITS NEEDED

		}
	}
}
