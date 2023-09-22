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
		//			QuantityValueText = "SET DEFAULT VALUE:"; ???????

		#region Fields
		private IShareEvents _shareEvents;
		private AsyncRelayCommand _deleteEventCommand;
		private AsyncRelayCommand _shareEventCommand;
		#endregion

		#region Properties
		public string PageTitle => IsEditMode ? "Edit Event" : "Add Event";
		public string HeaderText => IsEditMode ? $"EDIT EVENT" : "ADD NEW EVENT";
		protected override bool IsEditMode
		{
			get
			{
				return _selectedCurrentEvent != null;
			}
		}
		public bool EditModeVisibility => IsEditMode;

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
				if(IsEditMode)
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
		// ctor for creating evnents create mode
		public EventOperationsViewModel(IEventRepository eventRepository, DateTime selectedDate)
			: base(eventRepository)
		{
			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);

			// do not select any events by default
			//if(AllEventTypesOC != null && AllEventTypesOC.Count > 0)
			//{
			//	SelectedEventType = AllEventTypesOC[0];
			//	OnUserEventTypeSelected(SelectedEventType);
			//}
		}
		// ctor for editing events edit mode
		public EventOperationsViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
		: base(eventRepository)
		{           
			// value measurementType cannot be changed 
			IsValueTypeSelectionEnabled = false;
			_submitEventCommand = new AsyncRelayCommand(EditEvent, CanExecuteSubmitCommand);
			DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
			ShareEvents = new ShareEventsJson(eventRepository); // Confirm this line if needed
			ShareEventCommand = new AsyncRelayCommand(ShareEvent);

			// Set properties based on eventToEdit
			_selectedCurrentEvent = eventToEdit;
			OnUserEventTypeSelected(eventToEdit.EventType);
			Title = _selectedCurrentEvent.Title;
			Description = _selectedCurrentEvent.Description;
			StartDateTime = _selectedCurrentEvent.StartDateTime.Date;
			EndDateTime = _selectedCurrentEvent.EndDateTime.Date;
			StartExactTime = _selectedCurrentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _selectedCurrentEvent.EndDateTime.TimeOfDay;
			IsCompleted = _selectedCurrentEvent.IsCompleted;
			SelectedMainEventType = _selectedCurrentEvent.EventType.MainEventType;
			SelectedEventType = _selectedCurrentEvent.EventType;
			FilterAllEventTypesOCByMainEventType(SelectedMainEventType);	// CANNOT CHANGE MAIN EVENT TYPE
			if (_selectedCurrentEvent.QuantityAmount != null)
			{
				SelectedMeasurementUnit = MeasurementUnitsOC.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == _selectedCurrentEvent.QuantityAmount.Unit);
				QuantityValue = _selectedCurrentEvent.QuantityAmount.Value;
			}
			MainEventTypeSelectedCommand = null;
			
		}
		#endregion

		#region Command Execution Methods

		private bool CanExecuteSubmitCommand()
		{
			if (string.IsNullOrWhiteSpace(Title) || SelectedEventType == null)
			{
				return false;
			}
			return true;
		}

		private async Task AddEventAsync()
		{

			// Create a new Event based on the selected EventType
			QuantityAmount = new Quantity(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue);
			_selectedCurrentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime.Date + StartExactTime, EndDateTime.Date + EndExactTime, SelectedEventType, QuantityAmount);
			await _eventRepository.AddEventAsync(_selectedCurrentEvent);

			ClearFields();
		}

		private async Task EditEvent()
		{
			QuantityValueText = "SET VALUE:";
			_selectedCurrentEvent.Title = Title;
			_selectedCurrentEvent.Description = Description;
			_selectedCurrentEvent.EventType = SelectedEventType;
			_selectedCurrentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_selectedCurrentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_selectedCurrentEvent.IsCompleted = IsCompleted;
			QuantityAmount = new Quantity(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue);
			_selectedCurrentEvent.QuantityAmount = QuantityAmount;
			await _eventRepository.UpdateEventsAsync(_selectedCurrentEvent);
			await Shell.Current.GoToAsync("..");
		}
		private void FilterAllEventTypesOCByMainEventType(MainEventTypes value)
		{
			var tempFilteredEventTypes = AllEventTypesOC.ToList().FindAll(x => x.MainEventType == value);
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllEventTypesOC));
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
