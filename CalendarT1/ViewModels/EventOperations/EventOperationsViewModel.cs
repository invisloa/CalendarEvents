using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CalendarT1.Views.CustomControls;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventOperations
{
	class EventOperationsViewModel : EventOperationsBaseViewModel
	{

		#region Fields
		private IShareEvents _shareEvents;
		private AsyncRelayCommand _deleteEventCommand;
		private AsyncRelayCommand _shareEventCommand;
		#endregion

		#region Properties
		public string PageTitle => IsEdit ? "Edit Event" : "Add Event";
		public string HeaderText => IsEdit ? $"Edit event of Title: {Title}" : "Add New Event";
		public bool IsEdit => _selectedCurrentEvent != null;

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
		public EventOperationsViewModel(IEventRepository eventRepository, DateTime selectedDate)
			: base(eventRepository)
		{
			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
		}
		// ctor for editing events
		public EventOperationsViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
		: base(eventRepository)
		{
			_submitEventCommand = new AsyncRelayCommand(EditEvent, CanExecuteSubmitCommand);
			DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
			ShareEvents = new ShareEventsJson(eventRepository); // Confirm this line if needed
			ShareEventCommand = new AsyncRelayCommand(ShareEvent);
			// Set properties based on eventToEdit
			_selectedCurrentEvent = eventToEdit;
			Title = _selectedCurrentEvent.Title;
			Description = _selectedCurrentEvent.Description;
			StartDateTime = _selectedCurrentEvent.StartDateTime.Date;
			EndDateTime = _selectedCurrentEvent.EndDateTime.Date;
			StartExactTime = _selectedCurrentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _selectedCurrentEvent.EndDateTime.TimeOfDay;
			IsCompleted = _selectedCurrentEvent.IsCompleted;
			SelectedMainEventType = _selectedCurrentEvent.EventType.MainEventType;
			SelectedEventType = _selectedCurrentEvent.EventType;
			if(_selectedCurrentEvent.QuantityAmount != null)
			{
				SelectedMeasurementUnit = MeasurementUnitsOC.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == _selectedCurrentEvent.QuantityAmount.Unit);
				QuantityValueText = _selectedCurrentEvent.QuantityAmount.Value;

			}
			MainEventTypeSelectedCommand = new RelayCommand<MainEventVisualDetails>(noMatterWhat => { return; });
			
		}
		#endregion

		#region Command Execution Methods

		private bool CanExecuteSubmitCommand()
		{
			bool isValid = false;
			if(SelectedEventType!=null && SelectedEventType.MainEventType == MainEventTypes.Value)
			{
				if (!_isValueTypeTextOK)
				{
					return false; // EntryText is not valid
				}
			}
			return !string.IsNullOrWhiteSpace(Title) && SelectedEventType != null;

		}

		private async Task AddEventAsync()
		{
			// Create a new Event based on the selected EventType
			_selectedCurrentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime.Date + StartExactTime, EndDateTime.Date + EndExactTime, SelectedEventType, QuantityAmount);
			await _eventRepository.AddEventAsync(_selectedCurrentEvent);
			ClearFields();
		}

		private async Task EditEvent()
		{
			if(_selectedCurrentEvent.EventType.MainEventType == MainEventTypes.Value)
			{
				//if(!IsNumeric(EntryText))
				//{
				////TODO: Add error message
				//MessageProcessingHandler.ReferenceEquals("Quantity Amount must be a number", "Error");
				//return;
				//}
				//else
				//{
				//	//_selectedCurrentEvent.QuantityAmount = new Quantity((decimal)EntryText, SelectedMeasurementUnit.TypeOfMeasurementUnit);
				//}

			}
			
			_selectedCurrentEvent.Title = Title;
			_selectedCurrentEvent.Description = Description;
			_selectedCurrentEvent.EventType = SelectedEventType;
			_selectedCurrentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_selectedCurrentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_selectedCurrentEvent.IsCompleted = IsCompleted;
			_selectedCurrentEvent.QuantityAmount = QuantityAmount;
			await _eventRepository.UpdateEventsAsync(_selectedCurrentEvent);
			await Shell.Current.GoToAsync("..");
		}

		private async Task DeleteSelectedEvent()
		{
			await _eventRepository.DeleteFromEventsListAsync(_selectedCurrentEvent);
			await Shell.Current.GoToAsync("..");
		}

		private async Task ShareEvent()
		{
			await _shareEvents.ShareEventAsync(_selectedCurrentEvent);
		}

		#endregion
	}
}
