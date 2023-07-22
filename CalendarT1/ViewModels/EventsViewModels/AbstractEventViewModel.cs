using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using CommunityToolkit.Mvvm.Input;
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

		private ObservableCollection<EventTypeModel> _eventTypesOC;
		public ObservableCollection<EventTypeModel> EventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				if(_eventTypesOC == value)
				{
					return;
				}
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<EventModel> _eventsToShowList = new ObservableCollection<EventModel>();
		public ObservableCollection<EventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				_eventsToShowList = value;
				OnPropertyChanged();
			}
		}

		private List<EventModel> _allEventsList;
		public List<EventModel> AllEventsList
		{
			get => _allEventsList;
			set
			{
				_allEventsList = value;
				OnPropertyChanged();
			}
		}

		#endregion
		public event Action OnEventsToShowListUpdated;

		protected virtual void OnOnEventsToShowListUpdated()
		{
			OnEventsToShowListUpdated?.Invoke();
		}
		public AbstractEventViewModel(IEventRepository eventRepository)
		{

			
			EventTypesOC = new ObservableCollection<EventTypeModel>();
			EventTypesOC.Add(new EventTypeModel("BasicEvent", Color.FromHex("#FF0000"), false));
			EventTypesOC.Add(new EventTypeModel("BasicTask", Color.FromHex("#00FFFF"), true));
			var eventTypes = EventTypesOC;
			var json = JsonConvert.SerializeObject(eventTypes);
			Preferences.Set("event_types", json);

			 json = Preferences.Get("event_types", "");
			EventTypesOC = JsonConvert.DeserializeObject<ObservableCollection<EventTypeModel>>(json);
/*			if (EventTypesOC == null)
			{
				EventTypesOC.Add(new EventTypeModel("BasicEvent", Color.FromHex("#FF0000"), false));
				EventTypesOC.Add(new EventTypeModel("BasicTask", Color.FromHex("#00FFFF"), true));
				var eventTypes = EventTypesOC;
				json = JsonConvert.SerializeObject(eventTypes);
				Preferences.Set("event_types", json);
			}
*/
			_eventRepository = eventRepository;

		}

		#region Commands

		private RelayCommand<DateTime> _datePickerDateSelectedCommand;
		public RelayCommand<DateTime> DatePickerDateSelectedCommand =>
			_datePickerDateSelectedCommand ?? (_datePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected));

		private RelayCommand<EventTypeModel> _selectEventPriorityCommand;
		public RelayCommand<EventTypeModel> SelectEventPriorityCommand =>
			_selectEventPriorityCommand ?? (_selectEventPriorityCommand = new RelayCommand<EventTypeModel>(SelectEventPriority));

		private RelayCommand _goToAddEventPageCommand;
		public RelayCommand GoToAddEventPageCommand =>
			_goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));

		public RelayCommand<EventModel> _selectEventCommand;
		public RelayCommand<EventModel> SelectEventCommand =>
			_selectEventCommand ?? (_selectEventCommand = new RelayCommand<EventModel>(SelectEvent));

		#endregion

		#region Methods

		private void SelectEventPriority(EventTypeModel eventPriority)
		{
			eventPriority.IsSelectedToFilter = !eventPriority.IsSelectedToFilter;
			BindDataToScheduleList();
		}

		private void DatePickerDateSelected(DateTime newDate)
		{
			CurrentSelectedDate = newDate;
			BindDataToScheduleList();
		}

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddEventPage(_eventRepository));
		}

		private void SelectEvent(EventModel selectedEvent)
		{
			Debug.WriteLine($"Selected event: {selectedEvent.Title}");
			Application.Current.MainPage.Navigation.PushAsync(new EditEventPage(_eventRepository, selectedEvent));
		}
		#endregion

		#region Abstract Methods

		public abstract Task BindDataToScheduleList();
		public async Task LoadAndBindDataToScheduleListAsync()
		{
			AllEventsList = await _eventRepository.GetEventsListAsync();
			await BindDataToScheduleList();
		}

		#endregion


		protected async Task ApplyEventFilter(DateTime startDate, DateTime endDate)
		{
			var selectedEventTypes = EventTypesOC.Where(x => x.IsSelectedToFilter).Select(x => x.EventTypeName).ToList();

			var allEvents = await EventRepository.GetEventsListAsync();

			List<EventModel> filteredEvents = new List<EventModel>();

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
			EventsToShowList = new ObservableCollection<EventModel>(filteredEvents);
		}
	}
}
