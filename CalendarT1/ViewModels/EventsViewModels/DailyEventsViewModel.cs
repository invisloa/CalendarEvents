using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventFactories;
using CommunityToolkit.Mvvm.Input;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; }
		public DailyEventsViewModel(IEventRepository eventRepository, Dictionary<string, IBaseEventFactory> _eventFactories) : base(eventRepository, _eventFactories)
		{
			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent);
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents);
		}

		public async Task DeleteOneEvent()
		{
			if (AllEventsList.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsList[0];
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
