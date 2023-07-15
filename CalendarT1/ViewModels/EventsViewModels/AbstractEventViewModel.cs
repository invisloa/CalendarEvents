using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public abstract class AbstractEventViewModel : BaseViewModel
	{
		public AbstractEventViewModel(ObservableCollection<EventPriority> eventPriorities, IEventRepository eventRepository)
		{
			_eventPriorities = eventPriorities;
			_eventRepository = eventRepository;
			AllEventsList = _eventRepository.LoadEventsList();
			BindDataToScheduleList();
		}
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
					DatePickerDateSelectedCommand.Execute(_currentSelectedDate);
				}
			}
		}
		private ObservableCollection<EventPriority> _eventPriorities;
		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;

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
				BindDataToScheduleList();
				OnPropertyChanged();
			}
		}
		#endregion

		#region Commands
		public RelayCommand<DateTime> DatePickerDateSelectedCommand { get => new RelayCommand<DateTime>(DatePickerDateSelected); }
		public RelayCommand<EventPriority> SelectEventPriorityCommand { get => new RelayCommand<EventPriority>(SelectEventPriority); }
		public RelayCommand GoToAddEventPageCommand { get => new RelayCommand(GoToAddEventPage); }
		public RelayCommand<EventModel> SelectEventCommand { get => new RelayCommand<EventModel>(SelectEvent); }
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

		protected void SelectEvent(EventModel selectedEvent)
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
