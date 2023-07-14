using CalendarT1.Models;
using CalendarT1.Services;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		public RelayCommand DeleteOneEventCommand { get; set; }
		public RelayCommand DeleteAllEventsCommand { get; set; }
		public DailyEventsViewModel()
		{

		}
		public void DeleteOneEvent()
		{
			if (_allEventsList.Count == 0)
			{
				return;
			}
			var firstEvent = _allEventsList[0];
			_eventRepository.DeleteFromEventsList(firstEvent);
		}
		public void DeleteAllEvents()
		{
			_eventRepository.ClearEventsList();
		}
		public override void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
			var x = _allEventsList;

			var filteredScheduleList = _allEventsList
				.Where(x => (x.StartDateTime.Date == _currentSelectedDate.Date ||
							 x.EndDateTime.Date == _currentSelectedDate.Date) &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);
		}

	}

}
