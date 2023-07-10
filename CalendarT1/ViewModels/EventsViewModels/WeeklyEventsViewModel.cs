using CalendarT1.Models;
using CalendarT1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class WeeklyEventsViewModel : AbstractEventViewModel
	{
		public ObservableCollection<EventModel> MondayEvents { get; set; }
		public ObservableCollection<EventModel> TuesdayEvents { get; set; }
		public ObservableCollection<EventModel> WednesdayEvents { get; set; }
		public ObservableCollection<EventModel> ThursdayEvents { get; set; }
		public ObservableCollection<EventModel> FridayEvents { get; set; }
		public ObservableCollection<EventModel> SaturdayEvents { get; set; }
		public ObservableCollection<EventModel> SundayEvents { get; set; }
		public ObservableCollection<HourlyEvents> WeeklyEvents { get; set; }



		public WeeklyEventsViewModel()
		{
			DatePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected);
			SelectEventPriorityCommand = new RelayCommand<EventPriority>(SelectEventPriority);
			AddEventCommand = new RelayCommand(GoToAddEventPage);
			SelectEventCommand = new RelayCommand<EventModel>(ExecuteSelectEventCommand);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			AllEventsList = _eventRepository.LoadEventsList();
		}


		public override void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
			var endOfWeek = startOfWeek.AddDays(7);
			var x = _allEventsList;
			var filteredScheduleList = _allEventsList
				.Where(x => x.StartDateTime.Date >= startOfWeek.Date ||
							 x.EndDateTime.Date < endOfWeek.Date &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			// Initialize WeeklyEvents
			WeeklyEvents = new ObservableCollection<HourlyEvents>();
			foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
			{
				for (int hour = 0; hour < 24; hour++)
				{
					WeeklyEvents.Add(new HourlyEvents
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
				var hourlyEvents = WeeklyEvents.First(x => x.Day == eventModel.StartDateTime.DayOfWeek && x.Hour == eventModel.StartDateTime.Hour);
				hourlyEvents.Events.Add(eventModel);
			}

			// Make sure to trigger OnPropertyChanged for the new property
			OnPropertyChanged(nameof(WeeklyEvents));
		}
	}
}
