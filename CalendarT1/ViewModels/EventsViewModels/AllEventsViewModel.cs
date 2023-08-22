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
		private DateTime _filterDateFrom;
		private DateTime _filterDateTo;
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
				BindDataToScheduleList();
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDateTo;
			set
			{
				_filterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();
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
			SetFilterDatesValues();
		}
		public AllEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			var allTempTypes = new List<IUserEventTypeModel>();
			foreach (var item in AllEventTypesOC)
			{
				allTempTypes.Add(item);
			}

			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent);
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents);
			SetFilterDatesValues();
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
				ApplyEventsDatesFilter(FilterDateFrom, FilterDateTo);
			}
			else 
			{
				// TODO Change to also visually select proper event type
				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => x.EventType.EventTypeName == _eventType.EventTypeName)
					.ToList();

				EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
			}

		}
		#endregion

		public void SetFilterDatesValues()
		{
			if (AllEventsListOC.Count != 0)
			{
				FilterDateFrom = AllEventsListOC
					.OrderBy(e => e.StartDateTime)
					.FirstOrDefault()
					.StartDateTime;
			}
            else
            {
				FilterDateFrom = DateTime.Today;
            }
            FilterDateTo = DateTime.Today;
		}
	}
}
