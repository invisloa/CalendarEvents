using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventOperations
{
	class EventViewModel : EventOperationsBaseViewModel
	{
		//add observable collection for all MeasurementUnit types
		// there has to be some picker for a user to select measurement unit so i need an observable collectiion so a user could select one
		#region Fields

		private IShareEvents _shareEvents;
		private AsyncRelayCommand _deleteEventCommand;
		private AsyncRelayCommand _shareEventCommand;
		#endregion

		#region Properties
		public string PageTitle => IsEdit ? "Edit Event" : "Add Event";
		public string HeaderText => IsEdit ? $"Edit event of Title: {Title}" : "Add New Event";
		public bool IsEdit => _currentEvent != null;

		public IShareEvents ShareEvents
		{
			get => _shareEvents;
			set => _shareEvents = value;
		}

		public AsyncRelayCommand DeleteEventCommand
		{
			get => _deleteEventCommand;
			set => _deleteEventCommand = value;
		}

		public AsyncRelayCommand ShareEventCommand
		{
			get => _shareEventCommand;
			set => _shareEventCommand = value;
		}
		public override string SubmitButtonText
		{
			get
			{
				if(IsEdit)
				{
					return "SUBMIT CHANGES";
				}
				else
				{
					return "ADD NEW EVENT";
				}
			}
			set
			{

				// for now set is not used maybe it will be implemented when Languages will be added
				_submitButtonText = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region Constructors
		// ctor for creating evnents
		public EventViewModel(IEventRepository eventRepository, DateTime selectedDate)
			: base(eventRepository)
		{
			// Create new Event mode
			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			EventType = AllEventTypesOC.First(); // Initialize to the first event type to ensure it's not null
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
		}
		// ctor for editing events
		public EventViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
		: base(eventRepository)
		{
			_submitEventCommand = new AsyncRelayCommand(EditEvent, CanExecuteSubmitCommand);
			DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
			ShareEvents = new ShareEventsJson(eventRepository); // Confirm this line if needed
			ShareEventCommand = new AsyncRelayCommand(ShareEvent);

			// Set properties based on eventToEdit
			_currentEvent = eventToEdit;
			Title = _currentEvent.Title;
			Description = _currentEvent.Description;
			EventType = _currentEvent.EventType;
			StartDateTime = _currentEvent.StartDateTime.Date;
			EndDateTime = _currentEvent.EndDateTime.Date;
			StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
			IsCompleted = _currentEvent.IsCompleted;
		}
		#endregion

		#region Command Execution Methods

		private bool CanExecuteSubmitCommand() => !string.IsNullOrEmpty(Title);

		private async Task AddEventAsync()
		{
			// Create a new Event based on the selected EventType
			_currentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime + StartExactTime, EndDateTime + EndExactTime, EventType, QuantityAmount);
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

		#endregion
	}
}
