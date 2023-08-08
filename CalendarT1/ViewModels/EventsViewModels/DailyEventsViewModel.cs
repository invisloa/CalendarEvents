using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }                    // for testing purposes
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; }                   // for testing purposes
		private IUserEventTypeModel _eventType;
		public bool IsDailyView { get; set; } = true;
		public bool IsAllEventsView => !IsDailyView;
		public string AboveEventsListText
		{
			get
			{
				if (IsDailyView)
				{
					return "EVENTS LIST";
				}
				else
				{
					return $"ALL EVENTS OF TYPE {_eventType.EventTypeName}";
				}
			}
		}
		public DailyEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			IsDailyView = true;
		}
		public DailyEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			IsDailyView = false;
			_eventType = eventType;
			var allTempTypes = new List<IUserEventTypeModel>();
			foreach (var item in AllEventTypesOC)
			{
				allTempTypes.Add(item);
			}
			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent);              // for testing purposes
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents);            // for testing purposes
		}
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
			await EventRepository.ClearEventsListAsync();
		}

		public override void BindDataToScheduleList()
		{
				 ApplyEventsDatesFilter(CurrentSelectedDate.Date, CurrentSelectedDate.AddDays(1));
		}

	}
}
