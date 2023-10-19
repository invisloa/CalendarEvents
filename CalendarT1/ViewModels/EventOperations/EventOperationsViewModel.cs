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

		public RelayCommand IsCompleteFrameCommand { get; set; }

		#region Constructors
		// ctor for creating evnents create mode
		public EventOperationsViewModel(IEventRepository eventRepository, DateTime selectedDate)
			: base(eventRepository)
		{
			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
			IsCompleteFrameCommand = new RelayCommand(() => IsCompleted = !IsCompleted);

		}
		// ctor for editing events edit mode
		public EventOperationsViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
		: base(eventRepository)
		{           
			// value measurementType cannot be changed 
			_submitEventCommand = new AsyncRelayCommand(EditEventAsync, CanExecuteSubmitCommand);
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
			SelectedMainEventType = _selectedCurrentEvent.EventType.MainEventType;
			SelectedEventType = _selectedCurrentEvent.EventType;
			StartExactTime = _selectedCurrentEvent.StartDateTime.TimeOfDay;
			EndExactTime = _selectedCurrentEvent.EndDateTime.TimeOfDay;
			IsCompleted = _selectedCurrentEvent.IsCompleted;

			FilterAllSubEventTypesOCByMainEventType(SelectedMainEventType);	// CANNOT CHANGE MAIN EVENT TYPE
			if (_selectedCurrentEvent.EventType.IsValueType)
			{
				SelectedMeasurementUnit = MeasurementUnitsOC.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == _selectedCurrentEvent.QuantityAmount.Unit);
				QuantityValue = _selectedCurrentEvent.QuantityAmount.Value;
			}
			// TODO ADD EVENT TYPE multiTasks
			IsCompleteFrameCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
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
			QuantityAmount = new QuantityModel(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue);
			_selectedCurrentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime.Date + StartExactTime, EndDateTime.Date + EndExactTime, SelectedEventType, QuantityAmount);
			await _eventRepository.AddEventAsync(_selectedCurrentEvent);

			ClearFields();
		}

		private async Task EditEventAsync()
		{
			QuantityValueText = "SET VALUE:";
			_selectedCurrentEvent.Title = Title;
			_selectedCurrentEvent.Description = Description;
			_selectedCurrentEvent.EventType = SelectedEventType;
			_selectedCurrentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_selectedCurrentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_selectedCurrentEvent.IsCompleted = IsCompleted;
			QuantityAmount = new QuantityModel(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue);
			_selectedCurrentEvent.QuantityAmount = QuantityAmount;
			await _eventRepository.UpdateEventAsync(_selectedCurrentEvent);
			await Shell.Current.GoToAsync("..");
		}
		private void FilterAllSubEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = AllSubEventTypesOC.ToList().FindAll(x => x.MainEventType == value);
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllSubEventTypesOC));
		}

		private async Task DeleteSelectedEvent()
		{
				var action = await App.Current.MainPage.DisplayActionSheet($"Delete event {_selectedCurrentEvent.Title}", "Cancel", null, "Delete");
				switch (action)
				{
					case "Delete":
						await _eventRepository.DeleteFromEventsListAsync(_selectedCurrentEvent);
						await Shell.Current.GoToAsync("..");
					break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;

		}

		private async Task ShareEvent()
		{
			var action = await App.Current.MainPage.DisplayActionSheet("Not working for now!!!", "Cancel", null, "XXXX");

			// await _shareEvents.ShareEventAsync(_selectedCurrentEvent);
		}

		#endregion


	}
}
