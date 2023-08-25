using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using CalendarT1.Views.CustomControls;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	/// <summary>
	/// Contains only must know data for events
	/// </summary>
    public abstract class EventOperationsBaseViewModel : BaseViewModel , IMainEventTypesCC
	{

		public EventOperationsBaseViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_allUserTypesForVisuals = new List<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList());
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList());
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			MainEventTypeSelectedCommand = new RelayCommand<EventVisualDetails>(OnMainEventTypeSelected);
			SelectUserEventTypeCommand = new RelayCommand<IUserEventTypeModel>(OnUserEventTypeSelected);
		}

		//Fields
		#region Fields
				// Language
		protected string _submitButtonText;
		private int _fontSize = 20;

		// normal fields
		public bool _isValueTypeTextOK = false;
		protected IMainEventTypesCC _mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass();
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _selectedCurrentEvent;
		protected bool _isCompleted;
		private bool _isValueTypeSelected;
		protected string _title;
		protected string _description;
		private decimal _entryText = 0;
		protected Quantity _quantityAmount;
		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		protected AsyncRelayCommand _submitEventCommand;
		protected Color _mainEventTypeButtonColor;
		private MeasurementUnitItem _selectedMeasurementUnit;
		protected List<IUserEventTypeModel> _allUserTypesForVisuals;
		protected ObservableCollection<IUserEventTypeModel> _eventTypesOC;
		protected ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<MeasurementUnitItem> _measurementUnitsOC;
		protected IUserEventTypeModel _selectedEventType;
		private RelayCommand _goToAddNewTypePageCommand;


		#endregion
		//Properties
		#region Properties
		public int FontSize => _fontSize;
		public abstract string SubmitButtonText { get; set; }

		public string EventTypePickerText { get => "Select event Type"; }

		public decimal EntryText
		{
			get { return _entryText; }
			set
			{
				if (_entryText != value)
				{
					_entryText = value;
					_isValueTypeTextOK = true;
					QuantityAmount = new Quantity(EntryText, _selectedMeasurementUnit.TypeOfMeasurementUnit);
					OnPropertyChanged();
					_submitEventCommand.NotifyCanExecuteChanged();
				}
			}
		}

		public MainEventTypes SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				FilterAllEventTypesOCByMainEventType(value);
				OnPropertyChanged();
			}
		}

		private void FilterAllEventTypesOCByMainEventType(MainEventTypes value)
		{
			var tempFilteredEventTypes = FilterUserTypesForVisuals(value);
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllEventTypesOC));
		}

		private List<IUserEventTypeModel> FilterUserTypesForVisuals(MainEventTypes value)
		{
			return _allUserTypesForVisuals.FindAll(x => x.MainEventType == value);
		}
		public ObservableCollection<EventVisualDetails> MainEventTypesOC 
		{ 
			get => _mainEventTypesCCHelper.MainEventTypesOC;
			set => _mainEventTypesCCHelper.MainEventTypesOC = value; 
		}
		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC
		{
			get => _measurementUnitsOC;
			set
			{
				_measurementUnitsOC = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}


		// Basic Event Information
		#region Basic Event Information
		public IUserEventTypeModel SelectedEventType
		{
			get => _selectedEventType;
			set
			{
				_selectedEventType = value;
				MainEventTypeButtonsColor = _selectedEventType.EventTypeColor;
				_mainEventTypesCCHelper.SelectedMainEventType = _selectedEventType.MainEventType; // (This is not using SelectedMainEventType(property) so there would be no filtering applied to UserEventTypes)
				SetVisualsForSelectedUserType();
				SetPropperVisualForMainTypeSelected(value.MainEventType);
				if (_selectedEventType.MainEventType == MainEventTypes.Value)
				{
					if (MeasurementUnitsOC == null || MeasurementUnitsOC.Count == 0)
					{
						MeasurementUnitsOC = Factory.PopulateMeasurementCollection();
						SelectedMeasurementUnit = MeasurementUnitsOC[0];
					}
				}
				_submitEventCommand.NotifyCanExecuteChanged();

				OnPropertyChanged();
			}
		}

		public Color MainEventTypeButtonsColor
		{
			get
			{
				if (_mainEventTypeButtonColor == null)
				{
					return _mainEventTypesCCHelper.MainEventTypeButtonsColor;
				}
				else return _mainEventTypeButtonColor;
			}
			set
			{
				_mainEventTypeButtonColor = value;
				OnPropertyChanged();
			}
		}

		public bool IsCompleted
		{
			get => _isCompleted;
			set
			{
				_isCompleted = value;
				OnPropertyChanged();
			}
		}
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				_isValueTypeSelected = value;
				OnPropertyChanged();
			}
		}
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
				_submitEventCommand.NotifyCanExecuteChanged();
			}
		}
		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				OnPropertyChanged();
			}
		}
		// Start Date/Time
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				_startDateTime = value;
				OnPropertyChanged();
				if (_startDateTime > _endDateTime)
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
			}
		}
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				_startExactTime = value;
				OnPropertyChanged();
				if (_startDateTime.Date == _endDateTime.Date && _startExactTime > _endExactTime)
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
			}
		}
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				if (_startDateTime > value)
				{
					_startDateTime = value;
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(StartDateTime));
				}
				else
				{
					_endDateTime = value;
				}
				OnPropertyChanged();
			}
		}
		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				if (_startDateTime.Date == _endDateTime.Date && value < _startExactTime)
				{
					_startExactTime = value;
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(StartExactTime));

				}
				else
				{
					_endExactTime = value;
				}
				OnPropertyChanged();
			}
		}
		public Quantity QuantityAmount
		{
			get => _quantityAmount;
			set
			{
				_quantityAmount = value;
				OnPropertyChanged();
			}
		}
		public MeasurementUnitItem SelectedMeasurementUnit
		{
			get => _selectedMeasurementUnit;
			set
			{
				_selectedMeasurementUnit = value;
				QuantityAmount = new Quantity(EntryText, _selectedMeasurementUnit.TypeOfMeasurementUnit);
				OnPropertyChanged();
			}
		}

		#endregion

		// Command
		public AsyncRelayCommand SubmitEventCommand => _submitEventCommand;
		public RelayCommand<IUserEventTypeModel> SelectUserEventTypeCommand { get; set; }
		public RelayCommand<EventVisualDetails> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAddNewTypePageCommand => _goToAddNewTypePageCommand ?? (_goToAddNewTypePageCommand = new RelayCommand(GoToAddNewTypePage));
		#endregion


		// Helper Methods
		#region Helper Methods
		private bool IsNumeric(string value)
		{
			return Decimal.TryParse(value, out _);
		}

		protected void ClearFields()
		{
			Title = "";
			Description = "";
			if (AllEventTypesOC != null && AllEventTypesOC.Count > 0)
			{
				SelectedEventType = AllEventTypesOC[0];
			}
			StartDateTime = DateTime.Today;
			EndDateTime = DateTime.Today;
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.TimeOfDay;
			IsCompleted = false;
			if(SelectedEventType.MainEventType == MainEventTypes.Value)
			{
				EntryText = 0;
			}
		}
		protected void OnUserEventTypeSelected(IUserEventTypeModel selectedEvent)
		{
			SelectedEventType = selectedEvent;
		}
		protected void SetVisualsForSelectedUserType()
		{
			foreach (var eventType in AllEventTypesOC)		// it sets colors in a different AllEventTypesOC then SelectedEventType is...
			{
				eventType.BackgroundColor = Color.FromRgba(255, 255, 255, 1);
			}
			var SelectedEventType = AllEventTypesOC.FirstOrDefault(x => x.EventTypeName == _selectedEventType.EventTypeName);
			SelectedEventType.BackgroundColor = SelectedEventType.EventTypeColor;
		}
		protected void OnMainEventTypeSelected(EventVisualDetails eventType)
		{
			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(eventType);
			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
			if (SelectedMainEventType == MainEventTypes.Value)
			{
				SelectedMeasurementUnit = MeasurementUnitsOC[0];
			}
			OnUserEventTypeSelected(AllEventTypesOC[0]);


		}
		private void SetPropperVisualForMainTypeSelected(MainEventTypes _maineventType)
		{
			if(_maineventType == MainEventTypes.Value)
			{
				IsValueTypeSelected = true;
			}
			else
			{
				IsValueTypeSelected = false;
			}
		}
		public void DisableVisualsForAllMainEventTypes()
		{
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
		}
		private void GoToAddNewTypePage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewTypePage());
		}

		#endregion
	}
}
