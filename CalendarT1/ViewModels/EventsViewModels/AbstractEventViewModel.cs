using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public abstract class AbstractEventViewModel : BaseViewModel
	{

		// region for Properties
		#region Properties
		protected DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate
		{
			get => _currentDate;
		}
		protected DateTime _currentSelectedDate = DateTime.Now;
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
		//public ObservableCollection<EventModel> EventsToShowList { get; set; } = new ObservableCollection<EventModel>();

		protected ObservableCollection<EventModel> _eventsToShowList = new ObservableCollection<EventModel>();
		public ObservableCollection<EventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				_eventsToShowList = value;
				OnPropertyChanged();
			}
		}

		protected List<EventModel> _allEventsList;
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

		public RelayCommand<DateTime> DatePickerDateSelectedCommand { get => new RelayCommand<DateTime>(DatePickerDateSelected); }
		public RelayCommand<EventPriority> SelectEventPriorityCommand { get => new RelayCommand<EventPriority>(SelectEventPriority); }
		public RelayCommand AddEventCommand { get => new RelayCommand(GoToAddEventPage);
		public RelayCommand<EventModel> SelectEventCommand { get => new RelayCommand<EventModel>(ExecuteSelectEventCommand); }



		DeleteOneEventCommand = new RelayCommand(DeleteOneEvent);
		DeleteAllEventsCommand = new RelayCommand(DeleteAllEvents);
		SelectEventCommand =
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			AllEventsList = _eventRepository.LoadEventsList();

		#endregion
		//Services
		#region Services
		protected IEventRepository _eventRepository;
		#endregion
		#region Methods
		protected void SelectEventPriority(EventPriority eventPriority)
		{
			eventPriority.IsSelected = !eventPriority.IsSelected;
			BindDataToScheduleList();
		}

		protected void DatePickerDateSelected(DateTime newDate)
		{
			_currentSelectedDate = newDate;
			BindDataToScheduleList();
		}
		protected void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddEventPage());
		}

		protected void ExecuteSelectEventCommand(EventModel selectedEvent)
		{
			Debug.WriteLine($"Selected event: {selectedEvent.Title}");
			Application.Current.MainPage.Navigation.PushAsync(new EditEventPage(selectedEvent));
		}

		public void OnAppearing()
		{
			BindDataToScheduleList();
		}
		#endregion

		#region Abstract Methods
		public abstract void BindDataToScheduleList();
		#endregion
	}
}
