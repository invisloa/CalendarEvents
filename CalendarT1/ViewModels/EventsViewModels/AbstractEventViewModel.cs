using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CalendarT1.ViewModels.EventsViewModels
{
    public abstract class AbstractEventViewModel : BaseViewModel
	{
		#region Properties
		private IEventRepository _eventRepository;
		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}
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
					BindDataToScheduleList();
					//DatePickerDateSelectedCommand.Execute(_currentSelectedDate);		// TODO: check if this is the right way to do it
				}
			}
		}
		private ObservableCollection<IUserEventTypeModel> _eventTypesOC;
		public ObservableCollection<IUserEventTypeModel> EventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				if (_eventTypesOC == value)
				{
					return;
				}
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<IGeneralEventModel> _eventsToShowList = new ObservableCollection<IGeneralEventModel>();
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				_eventsToShowList = value;
				OnPropertyChanged();
			}
		}
		public List<IGeneralEventModel> AllEventsList
		{
			get => _eventRepository.AllEventsList;
		}
		#endregion
		public event Action OnEventsToShowListUpdated;
		protected virtual void OnOnEventsToShowListUpdated()
		{
			OnEventsToShowListUpdated?.Invoke();
		}
		public AbstractEventViewModel(IEventRepository eventRepository)
		{
	//		EventTypesOC = new ObservableCollection<IUserEventTypeModel>();      // load event types in onappearing method
			_eventRepository = eventRepository;
		}
		public async Task LoadEventTypes()
		{
			var eventTypes = await _eventRepository.GetUserEventTypesListAsync();
			EventTypesOC.Clear(); // Clear the collection before adding new items

			foreach (var eventType in eventTypes)
			{
				EventTypesOC.Add(eventType);
			}
		}

		#region Commands

		private RelayCommand<DateTime> _datePickerDateSelectedCommand;
		public RelayCommand<DateTime> DatePickerDateSelectedCommand =>
			_datePickerDateSelectedCommand ?? (_datePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected));

		private RelayCommand<UserEventTypeModel> _selectEventPriorityCommand;
		public RelayCommand<UserEventTypeModel> SelectEventPriorityCommand =>
			_selectEventPriorityCommand ?? (_selectEventPriorityCommand = new RelayCommand<UserEventTypeModel>(SelectEventType));

		private RelayCommand _goToAddEventPageCommand;
		public RelayCommand GoToAddEventPageCommand =>
			_goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));

		public RelayCommand<IGeneralEventModel> _selectEventCommand;
		public RelayCommand<IGeneralEventModel> SelectEventCommand =>
			_selectEventCommand ?? (_selectEventCommand = new RelayCommand<IGeneralEventModel>(SelectEvent));

		#endregion

		#region Methods

		private void SelectEventType(IUserEventTypeModel eventSubType)
		{
			eventSubType.IsSelectedToFilter = !eventSubType.IsSelectedToFilter;
			BindDataToScheduleList();
		}

		private void DatePickerDateSelected(DateTime newDate)
		{
			CurrentSelectedDate = newDate;
			BindDataToScheduleList();
		}

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_eventRepository));
		}

		private void SelectEvent(IGeneralEventModel selectedEvent)
		{
			Debug.WriteLine($"Selected event: {selectedEvent.Title}");
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_eventRepository, selectedEvent));
		}
		#endregion

		#region Abstract Methods

		public abstract Task BindDataToScheduleList();
		public async Task LoadAndBindDataToScheduleListAsync()
		{
			await BindDataToScheduleList();
		}
		#endregion

		protected async Task ApplyEventFilter(DateTime startDate, DateTime endDate)
		{
			var selectedEventTypes = EventTypesOC.Where(x => x.IsSelectedToFilter).Select(x => x.EventTypeName).ToList();

			var allEvents = await EventRepository.GetEventsListAsync();

			List<IGeneralEventModel> filteredEvents = new List<IGeneralEventModel>();

			// Step 1: Get events that fall within the specified date range
			foreach (var eventModel in allEvents)
			{
				if (eventModel.StartDateTime.Date >= startDate && eventModel.EndDateTime.Date <= endDate)
				{
					// Step 2: Get events of selected event types
					if (selectedEventTypes.Contains(eventModel.EventType.EventTypeName))
					{
						// Step 3: Add the filtered event to a list
						filteredEvents.Add(eventModel);
					}
				}
			}

			/*			var filteredEvents = allEvents
							.Where(x => x.StartDateTime.Date >= startDate &&
										x.EndDateTime.Date <= endDate &&
										selectedEventTypes.Contains(x.EventType.EventTypeName))
							.ToList();
			*/
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
		}
	}
}
