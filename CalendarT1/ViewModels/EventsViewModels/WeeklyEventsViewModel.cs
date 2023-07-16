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
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var startOfWeek = CurrentSelectedDate.AddDays(-(int)CurrentSelectedDate.DayOfWeek);
			var endOfWeek = startOfWeek.AddDays(7);
			var filteredScheduleList = AllEventsList
				.Where(x => x.StartDateTime.Date >= startOfWeek.Date ||
							 x.EndDateTime.Date < endOfWeek.Date &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();
			// Initialize WeeklyEvents
			EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);
		}
	}
}
