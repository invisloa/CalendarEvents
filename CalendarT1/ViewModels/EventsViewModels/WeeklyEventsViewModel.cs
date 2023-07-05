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


		public override void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
			var endOfWeek = startOfWeek.AddDays(7);

			var filteredScheduleList = _allEventsList
				.Where(x => x.StartDateTime.Date >= startOfWeek.Date &&
							 x.EndDateTime.Date < endOfWeek.Date &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			// Divide events into days of the week
			MondayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Monday));
			TuesdayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Tuesday));
			WednesdayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Wednesday));
			ThursdayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Thursday));
			FridayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Friday));
			SaturdayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Saturday));
			SundayEvents = new ObservableCollection<EventModel>(filteredScheduleList.Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Sunday));

			// Make sure to trigger OnPropertyChanged for the new properties
			OnPropertyChanged(nameof(MondayEvents));
			OnPropertyChanged(nameof(TuesdayEvents));
			OnPropertyChanged(nameof(WednesdayEvents));
			OnPropertyChanged(nameof(ThursdayEvents));
			OnPropertyChanged(nameof(FridayEvents));
			OnPropertyChanged(nameof(SaturdayEvents));
			OnPropertyChanged(nameof(SundayEvents));
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
	}
}
