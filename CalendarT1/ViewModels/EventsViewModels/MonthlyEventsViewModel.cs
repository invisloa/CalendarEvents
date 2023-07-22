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
			// Start of the month
			var startOfMonth = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1);

			// End of the month
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			await ApplyEventFilter(startOfMonth, endOfMonth);

			OnOnEventsToShowListUpdated(); // TODO TO CHECK IF ITS NEEDED

		}
	}
}
