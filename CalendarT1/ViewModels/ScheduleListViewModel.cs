using CalendarT1.Models;
using CalendarT1.Models.Enums;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
	public class ScheduleListViewModel : BaseViewModel
	{

		// region for Properties
		#region Properties
		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate
		{
			get => _currentDate;
		}
		private DateTime _currentSelectedDate = DateTime.Now;
		public DateTime CurrentSelectedDate
		{
			get => _currentSelectedDate;
			set
			{
				if (_currentSelectedDate != value)
				{
					_currentSelectedDate = value;
					OnPropertyChanged();
					DatePickerDateSelectedCommand.Execute(_currentSelectedDate);
				}
			}
		}
		public ObservableCollection<EventPriority> _eventPriorities;
		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;
			set
			{
				_eventPriorities = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<EventModel> EventsToShowList { get; set; } = new ObservableCollection<EventModel>();
		private List<EventModel> _allEventsList;
		public List<EventModel> AllEventsList
		{
			get => _allEventsList;
			set
			{
				_allEventsList = value;
				BindDataToScheduleList();
				OnPropertyChanged();
			}
		}
		#endregion
		#region Commands
		public ICommand DatePickerDateSelectedCommand { get; set; }
		public ICommand SelectEventPriorityCommand { get; set; }
		public ICommand AddEventCommand { get; set; }
		public ICommand SelectEventCommand { get; set; }
		#endregion
		//Services
		#region Services
		IEventRepository _eventRepository;
		#endregion
		public ScheduleListViewModel()
		{
			DatePickerDateSelectedCommand = new Command<DateTime>(DatePickerDateSelected);
			SelectEventPriorityCommand = new Command<EventPriority>(SelectEventPriority);
			AddEventCommand = new Command(AddEventOnlyForTesting);
			SelectEventCommand = new Command<EventModel>(ExecuteSelectEventCommand);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			AllEventsList = _eventRepository.LoadEventsList();
		}
		#region Methods
		private void SelectEventPriority(EventPriority eventPriority)
		{
			eventPriority.IsSelected = !eventPriority.IsSelected;
			BindDataToScheduleList();

		}
		public void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var filteredScheduleList = _allEventsList
				.Where(x => (x.StartDateTime.Date == _currentSelectedDate.Date ||
							 x.EndDateTime.Date == _currentSelectedDate.Date) &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			EventsToShowList.Clear();
			foreach (var item in filteredScheduleList)
			{
				EventsToShowList.Add(item);
			}
		}
		private void DatePickerDateSelected(DateTime newDate)
		{
			_currentSelectedDate = newDate;
			BindDataToScheduleList();
		}
		private void AddEventOnlyForTesting()       // we will save directly to the database because it will be moved to a separate page
		{
			_eventRepository.AddEvent(new EventModel()
			{
				Title = "New Event",
				Description = "New Event Description",
				StartDateTime = DateTime.Now,
				EndDateTime = DateTime.Now.AddHours(2),
				PriorityLevel = EventPriorities[3]
			});
		}
		// TODO: make a new window for event details 
		private void ExecuteSelectEventCommand(EventModel selectedEvent)
		{
			Debug.WriteLine($"Selected event: {selectedEvent.Title}");
		}
		#endregion
	}

}
