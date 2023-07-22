using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class WeeklyEventsViewModel : AbstractEventViewModel
	{

		public WeeklyEventsViewModel
						(IEventRepository eventRepository) 
						: base(eventRepository) { }

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
