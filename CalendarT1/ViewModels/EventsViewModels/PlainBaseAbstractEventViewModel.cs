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
		private IEventRepository _eventRepository;
		private ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<IUserEventTypeModel> _allEventTypesOC;
		private ObservableCollection<IGeneralEventModel> _eventsToShowList = new ObservableCollection<IGeneralEventModel>();
		private RelayCommand<UserEventTypeModel> _selectUserEventTypeCommand;
		private RelayCommand<IGeneralEventModel> _selectEventCommand;
		private Color _deselectedUserEventTypeColor = Color.FromHex("#FFC0C0C0");
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
		public RelayCommand<UserEventTypeModel> SelectUserEventTypeCommand => _selectUserEventTypeCommand ?? (_selectUserEventTypeCommand = new RelayCommand<UserEventTypeModel>(OnUserEventTypeSelected));
		public RelayCommand<IGeneralEventModel> SelectEventCommand => _selectEventCommand ?? (_selectEventCommand = new RelayCommand<IGeneralEventModel>(SelectEvent));


		#endregion

		#region Constructor
		public PlainBaseAbstractEventViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_allEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			_allEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserTypeListChanged += UpdateAllEventTypesList;
		}
		#endregion

		#region Public Methods
		public void UpdateAllEventList()		// TO Check idk if it does anything OC updates automatically
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}

		public void UpdateAllEventTypesList()	// TO Check idk if it does anything OC updates automatically
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

			EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
		}

		#region Events
		public event Action OnEventsToShowListUpdated;
		#endregion
	}
}
