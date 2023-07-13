using CalendarT1.Models;
using CalendarT1.Services;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{

	public class EditEventViewModel : EventOperationsBase
	{
		public string EditTitleText => $"Edit event of Title: {Title}";
		private RelayCommand _deleteEventCommand;
		public RelayCommand DeleteEventCommand { get => _deleteEventCommand; set { _deleteEventCommand = value; } }

		public EditEventViewModel(EventModel eventToEdit)
		{
			_submitEventCommand = new RelayCommand(EditEvent,CanEditEvent);
			DeleteEventCommand = new RelayCommand(DeleteSelectedEvent);
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			_currentEvent = eventToEdit;
			Title = _currentEvent.Title;
			Description = _currentEvent.Description;
			EventPriority = EventPriorities.First(ep => ep.PriorityLevel == _currentEvent.PriorityLevel.PriorityLevel);
			StartDateTime = _currentEvent.StartDateTime.Date;
			EndDateTime = _currentEvent.EndDateTime.Date;
			StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
			SubmitButtonText = "Submit Changes";
		}
		private void EditEvent()
		{
			_currentEvent.Title = Title;
			_currentEvent.Description = Description;
			_currentEvent.PriorityLevel = EventPriority;
			_currentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_currentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_currentEvent.IsCompleted = false;
			_eventRepository.UpdateEvent(_currentEvent);
			Shell.Current.GoToAsync("..");
		}
		private void DeleteSelectedEvent()
		{
			_eventRepository.DeleteFromEventsList(_currentEvent);
			_currentEvent = null;
			Shell.Current.GoToAsync("..");
		}
		private bool CanEditEvent()
		{
			return !string.IsNullOrEmpty(Title);
		}

	}
}