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
		public bool IsDefaultEventTimespanSelected
		{
			get
			{
				return UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected;
			}
			set
			{
				UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected = value;
				OnPropertyChanged();
			}
		}
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
		public RelayCommand IsDefaultTimespanSelectedCommand
		{
			get
			{
				return UserTypeExtraOptionsHelper.IsDefaultTimespanSelectedCommand;
			}
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
		// ctor for creating evnents create event mode
		public EventOperationsViewModel(IEventRepository eventRepository, DateTime selectedDate)
			: base(eventRepository)
		{
			StartDateTime = selectedDate;
			EndDateTime = selectedDate;
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
			IsCompleteFrameCommand = new RelayCommand(() => IsCompleted = !IsCompleted);

		}
		public EventOperationsViewModel(IEventRepository eventRepository)
			: base(eventRepository)
		{
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now;
			_submitEventCommand = new AsyncRelayCommand(AddEventAsync, CanExecuteSubmitCommand);
			IsCompleteFrameCommand = new RelayCommand(() => IsCompleted = !IsCompleted);

		}
		// ctor for editing events edit event mode
		public EventOperationsViewModel(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
		: base(eventRepository)
		{           
			// value measurementType cannot be changed 
			_submitEventCommand = new AsyncRelayCommand(EditEventAsync, CanExecuteSubmitCommand);
			DeleteEventCommand = new AsyncRelayCommand(DeleteSelectedEvent);
			ShareEvents = new ShareEventsJson(eventRepository); // Confirm this line if needed
			ShareEventCommand = new AsyncRelayCommand(ShareEvent);
			SelectUserEventTypeCommand = null;

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
			
			// ADD measurements if IsMeasurementType
			if (_selectedCurrentEvent.EventType.IsValueType)
			{
				_measurementSelectorHelperClass.SelectedMeasurementUnit = _measurementSelectorHelperClass.MeasurementUnitsOC.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == _selectedCurrentEvent.QuantityAmount.Unit);
				_measurementSelectorHelperClass.QuantityValue = _selectedCurrentEvent.QuantityAmount.Value;
			}


			// ADD EVENT MICROTASKS if IsMicroTaskType
			if (_selectedCurrentEvent.EventType.IsMicroTaskType)
			{
				MicroTasksCCAdapter.MicroTasksOC = new ObservableCollection<MicroTaskModel>(_selectedCurrentEvent.MicroTasksList);
			}
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
			_selectedCurrentEvent = Factory.CreatePropperEvent(Title, Description, StartDateTime.Date + StartExactTime, EndDateTime.Date + EndExactTime, SelectedEventType, _measurementSelectorHelperClass.QuantityAmount, MicroTasksCCAdapter.MicroTasksOC); // TODO !!!!!add microtasks
			await _eventRepository.AddEventAsync(_selectedCurrentEvent);

			ClearFields();
		}

		private async Task EditEventAsync()
		{
			_selectedCurrentEvent.Title = Title;
			_selectedCurrentEvent.Description = Description;
			_selectedCurrentEvent.EventType = SelectedEventType;
			_selectedCurrentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
			_selectedCurrentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
			_selectedCurrentEvent.IsCompleted = IsCompleted;
			_measurementSelectorHelperClass.QuantityAmount = new QuantityModel(_measurementSelectorHelperClass.SelectedMeasurementUnit.TypeOfMeasurementUnit, _measurementSelectorHelperClass.QuantityValue);
			_selectedCurrentEvent.QuantityAmount = _measurementSelectorHelperClass.QuantityAmount;
			await _eventRepository.UpdateEventAsync(_selectedCurrentEvent);
			await Shell.Current.GoToAsync("..");
		}
		private void FilterAllSubEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = AllSubEventTypesOC.ToList().FindAll(x => x.MainEventType.Equals(value));
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
