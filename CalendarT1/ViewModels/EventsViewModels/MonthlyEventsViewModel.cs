using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class MonthlyEventsViewModel : AbstractEventViewModel
	{

		public MonthlyEventsViewModel
						(IEventRepository eventRepository)
						: base(eventRepository) { }
		public async override Task BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();

			// Start of the month
			var startOfMonth = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1);

			// End of the month
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			var filteredScheduleList = AllEventsList
				.Where(x => x.StartDateTime.Date >= startOfMonth.Date &&
							x.EndDateTime.Date <= endOfMonth.Date &&
							selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			// Initialize MonthlyEvents
			EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);

			OnOnEventsToShowListUpdated();
		}
	}
}
