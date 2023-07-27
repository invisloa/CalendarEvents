using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CommunityToolkit.Mvvm.Input;

namespace CalendarT1.ViewModels.EventOperations
{
    class EventViewModel : EventOperationsBase
	{
		public string PageTitle => IsEdit ? "Edit Event" : "Add Event";
		public string HeaderText => IsEdit ? $"Edit event of Title: {Title}" : "Add New Event";
		public string SubmitButtonText => IsEdit ? "Submit Changes" : "Add Event";
		public bool IsEdit => _currentEvent != null;

		private IShareEvents _shareEvents;
		public IShareEvents ShareEvents { get => _shareEvents; set { _shareEvents = value; } }
		private AsyncRelayCommand _deleteEventCommand;
		public AsyncRelayCommand DeleteEventCommand { get => _deleteEventCommand; set { _deleteEventCommand = value; } }
		private AsyncRelayCommand _shareEventCommand;
		public AsyncRelayCommand ShareEventCommand { get => _shareEventCommand; set { _shareEventCommand = value; } }

		public EventViewModel(IEventRepository eventRepository, AbstractEventModel eventToEdit = null) : base()
		{
			_eventRepository = eventRepository;

			if (eventToEdit == null)
			{
				// Add mode
				_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecute);
			}
			else
			{
				// Edit mode
				_submitEventCommand = new AsyncRelayCommand(EditEvent, CanExecute);
				DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
				ShareEvents = new ShareEventsJson(eventRepository);
				ShareEventCommand = new AsyncRelayCommand(ShareEvent);

				// Set properties based on eventToEdit
				_currentEvent = eventToEdit;
				Title = _currentEvent.Title;
				Description = _currentEvent.Description;
				EventType = EventTypesOC.First(ep => ep.EventTypeName == _currentEvent.EventType.EventTypeName);
				StartDateTime = _currentEvent.StartDateTime.Date;
				EndDateTime = _currentEvent.EndDateTime.Date;
				StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
				EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
				IsCompleted = _currentEvent.IsCompleted;
			}
		}

		private bool CanExecute() => !string.IsNullOrEmpty(Title);

		private async Task AddEventAsync()
		{
			_currentEvent = new AbstractEventModel(Title, Description, EventType, StartDateTime + StartExactTime, EndDateTime + EndExactTime);
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
			await _eventRepository.UpdateEventAsync(_currentEvent);
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
