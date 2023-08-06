using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using Microsoft.Extensions.Logging;
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
		ObservableCollection<IGeneralEventModel> _allEventsListOC;
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}
		private ObservableCollection<IUserEventTypeModel> _allEventTypesOC;
		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _allEventTypesOC;
			set
			{
				if (_allEventTypesOC == value)
				{
					return;
				}
				_allEventTypesOC = value;
				OnPropertyChanged();
			}
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
		#endregion
		public event Action OnEventsToShowListUpdated;
		protected virtual void OnOnEventsToShowListUpdated()
		{
			OnEventsToShowListUpdated?.Invoke();
		}
		public AbstractEventViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_allEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			_allEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserTypeListChanged += UpdateAllEventTypesList;
		}
		public void UpdateAllEventList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}
		public void UpdateAllEventTypesList()
		{
			AllEventTypesOC= new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
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
			var selectedEventTypes = AllEventTypesOC.Where(x => x.IsSelectedToFilter).Select(x => x.EventTypeName).ToList();
			List<IGeneralEventModel> filteredEvents = new List<IGeneralEventModel>();
			foreach (var eventModel in AllEventsListOC)
			{
				if (eventModel.StartDateTime.Date >= startDate && eventModel.EndDateTime.Date <= endDate)
				{
					if (selectedEventTypes.Contains(eventModel.EventType.EventTypeName))
					{
						filteredEvents.Add(eventModel);
					}
				}
			}
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
		}
	}
}
