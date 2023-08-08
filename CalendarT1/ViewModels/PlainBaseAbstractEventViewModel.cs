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

namespace CalendarT1.ViewModels
{
	public abstract class PlainBaseAbstractEventViewModel : BaseViewModel
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
				OnOnEventsToShowListUpdated();
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
				OnOnEventsToShowListUpdated();

			}
		}

		private ObservableCollection<IGeneralEventModel> _eventsToShowList = new ObservableCollection<IGeneralEventModel>();
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => _eventsToShowList;
			set
			{
				if (_eventsToShowList == value)
				{
					return;
				}
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
		public PlainBaseAbstractEventViewModel(IEventRepository eventRepository)
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
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}
		private RelayCommand<UserEventTypeModel> _selectEventPriorityCommand;
		public RelayCommand<UserEventTypeModel> SelectEventPriorityCommand =>
			_selectEventPriorityCommand ?? (_selectEventPriorityCommand = new RelayCommand<UserEventTypeModel>(SelectEventType));

		private RelayCommand<IGeneralEventModel> _selectEventCommand;
		public RelayCommand<IGeneralEventModel> SelectEventCommand =>
			_selectEventCommand ?? (_selectEventCommand = new RelayCommand<IGeneralEventModel>(SelectEvent));
		private void SelectEventType(IUserEventTypeModel eventSubType)
		{
			eventSubType.IsSelectedToFilter = !eventSubType.IsSelectedToFilter;
			BindDataToScheduleList();
		}
		private void SelectEvent(IGeneralEventModel selectedEvent)
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_eventRepository, selectedEvent));              // TO DO CONSIDER CHANGE to await Shell.Current.GoToAsync($"AllEventsPageList?{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}");
		}
		public abstract void BindDataToScheduleList();

	}
}
