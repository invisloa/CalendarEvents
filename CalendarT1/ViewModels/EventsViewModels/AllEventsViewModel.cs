using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	internal class AllEventsViewModel : AbstractEventViewModel
	{
		#region Fields

		private IUserEventTypeModel _eventType;
		private DateTime _filterDateFrom = DateTime.MinValue;
		private DateTime _filterDateTo = DateTime.Today;

		#endregion

		#region Properties
		
		public string TextFilterDateFrom { get; set; } = "FILTER FROM:";
		public string TextFilterDateTo { get; set; } = "FILTER UP TO:";
		public DateTime FilterDateFrom
		{
			get => _filterDateFrom;
			set
			{
				_filterDateFrom = value;
				OnPropertyChanged();
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDateTo;
			set
			{
				_filterDateTo = value;
				OnPropertyChanged();
			}
		}
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; } // for testing purposes
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }  // for testing purposes
		public string AboveEventsListText
		{
			get
			{
				// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}

		#endregion

		#region Constructors

		public AllEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent);
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents);
			BindDataToScheduleList();
		}
		public AllEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			_eventType = eventType;

			var allTempTypes = new List<IUserEventTypeModel>();
			foreach (var item in AllEventTypesOC)
			{
				allTempTypes.Add(item);
			}

			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent);
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents);
			BindDataToScheduleList();
		}

		#endregion

		#region Commands

		public async Task DeleteOneEvent()
		{
			if (AllEventsListOC.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsListOC[0];
			await EventRepository.DeleteFromEventsListAsync(firstEvent);
		}

		public async Task DeleteAllEvents()
		{
			var eventsToNotDelete = AllEventsListOC.Where(x => EventsToShowList.Contains(x)).ToList();
			foreach (var item in eventsToNotDelete)
			{
				await EventRepository.DeleteFromEventsListAsync(item);
			}
		}

		#endregion

		#region Override Methods

		public override void BindDataToScheduleList()
		{
			if (_eventType == null)
			{
				var selectedToFilterEventTypes = AllEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()))
					.ToList();

				EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
			}
			else 
				if (_eventType != null)
			{
				// TODO Change to also visually select proper event type
				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => x.EventType.EventTypeName == _eventType.EventTypeName)
					.ToList();

				EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
			}

		}
		#endregion
	}
}
