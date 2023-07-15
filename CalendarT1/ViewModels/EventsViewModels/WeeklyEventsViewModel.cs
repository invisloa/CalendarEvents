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
						(ObservableCollection<EventPriority> eventPriorities, IEventRepository eventRepository) 
						: base(eventPriorities, eventRepository) { }
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
			ObservableCollection<HourlyEvents>  _tempWeeklyEvents = new ObservableCollection<HourlyEvents>();
			foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
			{
				for (int hour = 0; hour < 24; hour++)
				{  
					_tempWeeklyEvents.Add(new HourlyEvents
					{
						Day = day,
						Hour = hour,
						Events = new ObservableCollection<EventModel>()
					});
				}
			}

			// Divide events into days of the week and hours of the day
			foreach (var eventModel in filteredScheduleList)
			{
				var hourlyEvents = _tempWeeklyEvents.First(x => x.Day == eventModel.StartDateTime.DayOfWeek && x.Hour == eventModel.StartDateTime.Hour);
				hourlyEvents.Events.Add(eventModel);
			}
		}
	}
}
