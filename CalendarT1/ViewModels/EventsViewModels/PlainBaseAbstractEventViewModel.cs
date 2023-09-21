using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public abstract class PlainBaseAbstractEventViewModel : BaseViewModel
	{
		#region Fields
		protected IEventRepository _eventRepository;
		private ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<IUserEventTypeModel> _allEventTypesOC;
		private ObservableCollection<IGeneralEventModel> _eventsToShowList = new ObservableCollection<IGeneralEventModel>();
		private RelayCommand<IUserEventTypeModel> _selectUserEventTypeCommand;
		private RelayCommand<IGeneralEventModel> _selectEventCommand;
		private RelayCommand _goToAddNewTypePageCommand;

		protected Color _deselectedUserEventTypeColor = Color.FromArgb("#FFC0C0C0");
		#endregion

		#region Properties
		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
				OnOnEventsToShowListUpdated();
			}
		}

		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _allEventTypesOC;
			set
			{
				if (_allEventTypesOC == value) return;
				_allEventTypesOC = value;
				OnPropertyChanged();
				OnOnEventsToShowListUpdated();
			}
		}
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				if (_eventsToShowList == value) return;
				_eventsToShowList = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand<IUserEventTypeModel> SelectUserEventTypeCommand
		{
			get
			{
				return _selectUserEventTypeCommand ?? (_selectUserEventTypeCommand = new RelayCommand<IUserEventTypeModel>(OnUserEventTypeSelected));
			}
			set
			{
				if (_selectUserEventTypeCommand == value) return;
				_selectUserEventTypeCommand = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand GoToAddNewTypePageCommand => _goToAddNewTypePageCommand ?? (_goToAddNewTypePageCommand = new RelayCommand(GoToAddNewTypePage));

		public RelayCommand<IGeneralEventModel> SelectEventCommand => _selectEventCommand ?? (_selectEventCommand = new RelayCommand<IGeneralEventModel>(SelectEvent));

		#endregion

		#region Constructor
		public PlainBaseAbstractEventViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserTypeListChanged += UpdateAllEventTypesList;
		}
		#endregion

		#region Public Methods
		public void UpdateAllEventList()		// TO CHECK HOW TO CHANGE THIS
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}

		public void UpdateAllEventTypesList()   // TO CHECK HOW TO CHANGE THIS
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}

		public abstract void BindDataToScheduleList();
		#endregion

		#region Protected Methods
		protected virtual void OnOnEventsToShowListUpdated()
		{
			OnEventsToShowListUpdated?.Invoke();
		}
		#endregion

		#region Private Methods
		protected virtual void OnUserEventTypeSelected(IUserEventTypeModel eventSubType)
		{
			eventSubType.IsSelectedToFilter = !eventSubType.IsSelectedToFilter;
			if(eventSubType.IsSelectedToFilter)
			{
				eventSubType.BackgroundColor = eventSubType.EventTypeColor;
			}
			else
			{
				eventSubType.BackgroundColor = _deselectedUserEventTypeColor;
			}
			BindDataToScheduleList();
		}

		private void SelectEvent(IGeneralEventModel selectedEvent)
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_eventRepository, selectedEvent));
		}

		#endregion
		protected virtual void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{

			var selectedToFilterEventTypes = AllEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

			List<IGeneralEventModel> filteredEvents = AllEventsListOC
				.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
							x.StartDateTime.Date >= startDate.Date &&
							x.EndDateTime.Date <= endDate.Date)
				.ToList();

			// Clear existing items in the EventsToShowList
			EventsToShowList.Clear();

			// Add filtered items to the EventsToShowList
			foreach (var eventItem in filteredEvents)
			{
				EventsToShowList.Add(eventItem);
			}
		}
		private void GoToAddNewTypePage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewTypePage());
		}

		#region Events
		public event Action OnEventsToShowListUpdated;
		#endregion
	}
}
