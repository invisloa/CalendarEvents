using CalendarT1.Models;
using CalendarT1.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class WeeklyEventsViewModel : AbstractEventViewModel
	{
		public ObservableCollection<HourlyEvents> _weeklyEvents { get; set; }

		public ObservableCollection<HourlyEvents> WeeklyEvents
		{
			get => _weeklyEvents;
			set
			{
				if (_weeklyEvents == value) { return; }
				_weeklyEvents = value;
				OnPropertyChanged();
			}
		}
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
			WeeklyEvents = _tempWeeklyEvents;
		}
	}
}
