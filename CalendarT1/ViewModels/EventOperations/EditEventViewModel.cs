using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{

	public class EditEventViewModel : EventOperationsBase
	{
		public string EditTitleText => $"Edit event of Title: {Title}";
		private AsyncRelayCommand _deleteEventCommand;
		public AsyncRelayCommand DeleteEventCommand { get => _deleteEventCommand; set { _deleteEventCommand = value; } }
		private IShareEvents _shareEvents;
		public IShareEvents ShareEvents { get => _shareEvents; set { _shareEvents = value; } }

		public EditEventViewModel(IEventRepository eventRepository, EventModel eventToEdit) : base()
		{
			_submitEventCommand = new AsyncRelayCommand(EditEvent,CanEditEvent);
			DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
			_eventRepository = eventRepository;
			_currentEvent = eventToEdit;
			Title = _currentEvent.Title;
			Description = _currentEvent.Description;
			EventType = EventTypesOC.First(ep => ep.EventTypeName == _currentEvent.EventType.EventTypeName);
			StartDateTime = _currentEvent.StartDateTime.Date;
			EndDateTime = _currentEvent.EndDateTime.Date;
			StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
			IsCompleted = _currentEvent.IsCompleted;
			SubmitButtonText = "Submit Changes";
			ShareEvents = new ShareEventsJson(_eventRepository);  // TODO TO CHANGE 
			ShareEventCommand = new RelayCommand(ShareEvent);
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
		public RelayCommand ShareEventCommand { get; set; }
		private void ShareEvent()
		{
			ShareEvents.ShareEventAsync(_currentEvent);
		}
		private bool CanEditEvent()
		{
			return !string.IsNullOrEmpty(Title);
		}

	}
}