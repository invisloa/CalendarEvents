using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using CalendarT1.Views.CustomControls.CCHelperClass;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	/// <summary>
	/// Contains only must know data for events
	/// </summary>
	public abstract class EventOperationsBaseViewModel : BaseViewModel, IMainEventTypesCC, IMeasurementSelectorCC
	{
		//MeasurementCC implementation
		#region MeasurementCC implementation
		IMeasurementSelectorCC _measurementSelectorHelperClass { get; set; } = Factory.CreateMeasurementSelectorCCHelperClass();
		public int ValueFontSize { get; set; } = 20;
		public bool IsValueTypeSelectionEnabled { get; set; } = true;

		private bool _isValueTypeSelected;
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				if (_isValueTypeSelected != value)
				{
					_isValueTypeSelected = value;
					OnPropertyChanged();
				}
			}
		}
		public void SelectPropperMeasurementData(IUserEventTypeModel userEventTypeModel)
		{
			_measurementSelectorHelperClass.SelectPropperMeasurementData(userEventTypeModel);
		}
		public string QuantityValueText { get => _measurementSelectorHelperClass.QuantityValueText; set => _measurementSelectorHelperClass.QuantityValueText = value; }
		public decimal QuantityValue
		{
			get => _measurementSelectorHelperClass.QuantityValue;
			set
			{
				_measurementSelectorHelperClass.QuantityValue = value;
				OnPropertyChanged();
			}
		}
		public MeasurementUnitItem SelectedMeasurementUnit
		{
			get => _measurementSelectorHelperClass.SelectedMeasurementUnit;
			set
			{
				_measurementSelectorHelperClass.SelectedMeasurementUnit = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC => _measurementSelectorHelperClass.MeasurementUnitsOC;
		public Quantity QuantityAmount { get => _measurementSelectorHelperClass.QuantityAmount; set => _measurementSelectorHelperClass.QuantityAmount = value; }
		private void OnMeasurementUnitSelected(MeasurementUnitItem measurementUnitItem)
		{
			_measurementSelectorHelperClass.SelectedMeasurementUnit = measurementUnitItem;
		}
		// set this relay command in a constructor
		public virtual RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand { get; set; }
		#endregion

		public EventOperationsBaseViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_allUserTypesForVisuals = new List<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList());
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList());
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventVisualDetails>(OnMainEventTypeSelected);
			SelectUserEventTypeCommand = new RelayCommand<IUserEventTypeModel>(OnUserEventTypeSelected);
			MeasurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);
		}

		//Fields
		#region Fields
		// Language
		private int _fontSize = 20;
		protected string _submitButtonText;


		// normal fields
		private bool _firstTimeEventsShown = true;

		protected IMainEventTypesCC _mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass();
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _selectedCurrentEvent;
		protected bool _isCompleted;
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
		protected List<IUserEventTypeModel> _allUserTypesForVisuals;
		protected ObservableCollection<IUserEventTypeModel> _eventTypesOC;
		protected ObservableCollection<IGeneralEventModel> _allEventsListOC;
		protected IUserEventTypeModel _selectedEventType;
		private RelayCommand _goToAddNewTypePageCommand;
		private RelayCommand _goToAddEventPageCommand;


		#endregion
		//Properties
		#region Properties
		protected abstract bool IsEditMode { get; }
		public int FontSize => _fontSize;
		public abstract string SubmitButtonText { get; set; }

		public string EventTypePickerText { get => "Select event Type"; }
		public RelayCommand GoToAddEventPageCommand
		{
			get
			{
				return _goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));
			}
		}



		public MainEventTypes SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
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
		public ObservableCollection<MainEventVisualDetails> MainEventTypesOC 
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
		public IUserEventTypeModel SelectedEventType		// TO CHANGE !!!
		{
			get => _selectedEventType;
			set
			{
				_selectedEventType = value;
				MainEventTypeButtonsColor = _selectedEventType.EventTypeColor;
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


		#endregion

		// Command
		public AsyncRelayCommand SubmitEventCommand => _submitEventCommand;
		public RelayCommand<IUserEventTypeModel> SelectUserEventTypeCommand { get; set; }
		public RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAddNewTypePageCommand => _goToAddNewTypePageCommand ?? (_goToAddNewTypePageCommand = new RelayCommand(GoToAddNewTypePage));
		#endregion


		// Helper Methods
		#region Helper Methods
		private bool IsNumeric(string value)
		{
			return Decimal.TryParse(value, out _);
		}

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(_eventRepository, DateTime.Today));
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
				QuantityValue = 0;
			}
		}
		protected void OnUserEventTypeSelected(IUserEventTypeModel selectedEvent)
		{
			var lastSelectedTypedefaultValue = SelectedEventType?.QuantityAmount.Value ?? 0m;
			SelectedEventType = selectedEvent;
			SelectedMainEventType = SelectedEventType.MainEventType;
			if (SelectedMainEventType == MainEventTypes.Value)
			{
				IsValueTypeSelected = true;
				_measurementSelectorHelperClass.SelectPropperMeasurementData(SelectedEventType);
				if (!IsEditMode && (QuantityValue == 0 || QuantityValue == lastSelectedTypedefaultValue))
				{	//Set default values when createMode
					QuantityValue = SelectedEventType.QuantityAmount.Value;
				}
				SelectedMeasurementUnit = _measurementSelectorHelperClass.SelectedMeasurementUnit;
			}
			else
			{
				IsValueTypeSelected = false;
			}

			//((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypeSelectedCommand.Execute( ); // (This is not using SelectedMainEventType(property) so there would be no filtering applied to UserEventTypes)
			SetVisualsForSelectedUserType();
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
		protected virtual void OnMainEventTypeSelected(MainEventVisualDetails selectedMainEventType)
		{
			if (SelectedMainEventType.ToString() != selectedMainEventType.ToString())
			{
				((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypeSelectedCommand.Execute(selectedMainEventType);
				SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
				if (SelectedMainEventType == MainEventTypes.Value)
				{
					IsValueTypeSelected = true;
				}
				else
				{
					IsValueTypeSelected = false;
				}

				FilterAllEventTypesOCByMainEventType(SelectedMainEventType);
			}
			if(AllEventTypesOC.Count > 0)
			{
				OnUserEventTypeSelected(AllEventTypesOC[0]);
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
