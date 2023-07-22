using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	class AddEventViewModel : EventOperationsBase
	{

		public AddEventViewModel(IEventRepository eventRepository) : base()
		{
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, canAddEvent);
			_eventRepository = eventRepository;
			SubmitButtonText = "Add Event";
			ClearFields();
		}
		private async Task AddEventAsync()
		{
			_currentEvent = new EventModel(Title, Description, EventType, StartDateTime+StartExactTime, EndDateTime+EndExactTime);
			await _eventRepository.AddEventAsync(_currentEvent);

			ClearFields();
		}
		private bool canAddEvent()
		{
			return !string.IsNullOrEmpty(Title);
		}
	}
}