using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }                    // for testing purposes
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; }                   // for testing purposes
		public bool IsDailyView { get; set; } = true;
		public bool IsAllEventsView => !IsDailyView;
		public DailyEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			IsDailyView = true;
		}
		public DailyEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			IsDailyView = false;
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

		public override async Task BindDataToScheduleList()
		{
			await ApplyEventFilter(CurrentSelectedDate.Date, CurrentSelectedDate.AddDays(1));
		}

	}
}
