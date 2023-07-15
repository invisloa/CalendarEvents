using CalendarT1.Models;
using CalendarT1.Services;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	class AddEventViewModel : EventOperationsBase
	{

		public AddEventViewModel()
		{

			_submitEventCommand = new RelayCommand(AddEventAsync, canAddEvent);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			SubmitButtonText = "Add Event";
			ClearFields();
		}
		private async Task AddEventAsync()
		{
			_currentEvent = new EventModel(Title, Description, EventPriority, StartDateTime + StartExactTime, EndDateTime + EndExactTime);
			await _eventRepository.AddEventAsync(_currentEvent);
			Title = "";
			Description = "";
			EventPriority = EventPriorities[0];
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now.AddHours(1);
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.AddHours(1).TimeOfDay;
			ClearFields();
		}
		private bool canAddEvent()
		{
			return !string.IsNullOrEmpty(Title);
		}
	}
}