using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; }
		public DailyEventsViewModel( IEventRepository eventRepository) : base( eventRepository)
		{
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
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevelEnums).ToList();

			var allEvents = await EventRepository.GetEventsListAsync();

			var filteredScheduleList = allEvents
				.Where(x => (x.StartDateTime.Date == CurrentSelectedDate.Date ||
							 x.EndDateTime.Date == CurrentSelectedDate.Date) &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevelEnums))
				.ToList();

			EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);
		}

	}
}
