using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
    class EventViewModel : EventOperationsBase
	{
		// dictionary factory DI
		private IShareEvents _shareEvents;

		public string PageTitle => IsEdit ? "Edit Event" : "Add Event";
		public string HeaderText => IsEdit ? $"Edit event of Title: {Title}" : "Add New Event";
		public string SubmitButtonText => IsEdit ? "Submit Changes" : "Add Event";
		public bool IsEdit => _currentEvent != null;
		public IShareEvents ShareEvents { get => _shareEvents; set { _shareEvents = value; } }
		private AsyncRelayCommand _deleteEventCommand;
		public AsyncRelayCommand DeleteEventCommand { get => _deleteEventCommand; set { _deleteEventCommand = value; } }
		private AsyncRelayCommand _shareEventCommand;
		public AsyncRelayCommand ShareEventCommand { get => _shareEventCommand; set { _shareEventCommand = value; } }
		public async Task OnApearing()
		{

		}
		public EventViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit = null) : base(eventRepository)
		{

			if (eventToEdit == null)
			{
				// Create new Event mode
				EventType = AllEventTypesOC.First(); // Initialize to first event type so it wont be null
				_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand); // ????????????
			}
			else
			{
				// Edit Event mode
				_submitEventCommand = new AsyncRelayCommand(EditEvent, CanExecuteSubmitCommand);
				DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
				ShareEvents = new ShareEventsJson(eventRepository);									// to check???
				ShareEventCommand = new AsyncRelayCommand(ShareEvent);

				// Set properties based on eventToEdit
				_currentEvent = eventToEdit;
				Title = _currentEvent.Title;
				Description = _currentEvent.Description;
				EventType = _currentEvent.EventType; //EventTypesOC.First(ep => ep.EventTypeName == _currentEvent.EventType.EventTypeName);
				StartDateTime = _currentEvent.StartDateTime.Date;
				EndDateTime = _currentEvent.EndDateTime.Date;
				StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
				EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
				IsCompleted = _currentEvent.IsCompleted;
			}
		}

		private bool CanExecuteSubmitCommand() => !string.IsNullOrEmpty(Title);

		private async Task AddEventAsync()
		{
			// Create new Event according to a selected EventType
			_currentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime+StartExactTime, EndDateTime+EndExactTime, EventType, SpendingAmount);
			await _eventRepository.AddEventAsync(_currentEvent);
			ClearFields();
		}

		private async Task EditEvent()
		{
			_currentEvent.Title = Title;
			_currentEvent.Description = Description;
			_currentEvent.EventType = EventType;
			_currentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_currentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_currentEvent.IsCompleted = IsCompleted;
			await _eventRepository.UpdateEventsAsync(_currentEvent);
			await Shell.Current.GoToAsync("..");
		}

		private async Task DeleteSelectedEvent()
		{
			await _eventRepository.DeleteFromEventsListAsync(_currentEvent);
			await Shell.Current.GoToAsync("..");
		}

		private async Task ShareEvent()
		{
			await _shareEvents.ShareEventAsync(_currentEvent);
		}
	}
}
