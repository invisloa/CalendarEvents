using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	class AddEventViewModel : EventOperationsBase
	{

		public AddEventViewModel(IEventRepository eventRepository)
		{

			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, canAddEvent);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = eventRepository;
			SubmitButtonText = "Add Event";
			ClearFields();
		}
		private async Task AddEventAsync()
		{
			_currentEvent = new EventModel(Title, Description, EventPriority, StartDateTime+StartExactTime, EndDateTime+EndExactTime);
			await _eventRepository.AddEventAsync(_currentEvent);

			ClearFields();
		}
		private bool canAddEvent()
		{
			return !string.IsNullOrEmpty(Title);
		}
	}
}