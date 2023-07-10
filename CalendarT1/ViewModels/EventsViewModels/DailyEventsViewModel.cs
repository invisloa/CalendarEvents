using CalendarT1.Models;
using CalendarT1.Models.Enums;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;
using CalendarT1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels.EventsViewModels
{
    public class DailyEventsViewModel : AbstractEventViewModel
    {
		public RelayCommand DeleteOneEventCommand { get; set; }
		public RelayCommand DeleteAllEventsCommand { get; set; }
		public DailyEventsViewModel()
		{
			DatePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected);
			SelectEventPriorityCommand = new RelayCommand<EventPriority>(SelectEventPriority);
			AddEventCommand = new RelayCommand(GoToAddEventPage);
			DeleteOneEventCommand = new RelayCommand(DeleteOneEvent);
			DeleteAllEventsCommand = new RelayCommand(DeleteAllEvents);
			SelectEventCommand = new RelayCommand<EventModel>(ExecuteSelectEventCommand);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			AllEventsList = _eventRepository.LoadEventsList();
		}
		public void DeleteOneEvent()
		{
			if(_allEventsList.Count == 0)
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
