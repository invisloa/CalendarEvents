using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public RelayCommand DeleteOneEventCommand { get; set; }
		public RelayCommand DeleteAllEventsCommand { get; set; }
		public DailyEventsViewModel(ObservableCollection<EventPriority> eventPriorities, IEventRepository eventRepository) : base(eventPriorities, eventRepository)
		{
		}
		public void DeleteOneEvent()
		{
			if (AllEventsList.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsList[0];
			EventRepository.DeleteFromEventsList(firstEvent);
		}
		public void DeleteAllEvents()
		{
			EventRepository.ClearEventsList();
		}
		public override void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var x = AllEventsList;

			var filteredScheduleList = AllEventsList
				.Where(x => (x.StartDateTime.Date == CurrentSelectedDate.Date ||
							 x.EndDateTime.Date == CurrentSelectedDate.Date) &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);
		}

	}

}
