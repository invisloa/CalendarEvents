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
using System.Runtime.CompilerServices;

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

		private bool _isMultiTaskTypeSelected;
		public bool IsMultiTaskTypeSelected
		{
			get => _isMultiTaskTypeSelected;
			set
			{
				if (_isMultiTaskTypeSelected != value)
				{
					_isMultiTaskTypeSelected = value;
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
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass(eventRepository.AllMainEventTypesList);
			_eventRepository = eventRepository;
			_allUserTypesForVisuals = new List<IUserEventTypeModel>(eventRepository.DeepCopySubEventTypesList());
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopySubEventTypesList());
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
		protected IMainEventTypesCC _mainEventTypesCCHelper;
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _selectedCurrentEvent;
		protected bool _isCompleted;
		protected string _title;
		protected string _description;
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
		public event Action<IMainEventType> MainEventTypeChanged;


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



		public IMainEventType SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				OnPropertyChanged();
			}
		}

		private void FilterAllEventTypesOCByMainEventType(IMainEventType value)
		{
			var tempFilteredEventTypes = FilterUserTypesForVisuals(value);

            AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllEventTypesOC));
		}

		private List<IUserEventTypeModel> FilterUserTypesForVisuals(IMainEventType value)
		{
			string myvalue = $"my value is: {value}";
			string values = "";
			foreach (var item in _allUserTypesForVisuals)
			{
				values += $"my values are: {item.MainEventType}";
			}
			return _allUserTypesForVisuals.FindAll(x => x.MainEventType.Equals(value));
		}
		public ObservableCollection<MainEventVisualDetails> MainEventTypesVisualsOC 
		{ 
			get => _mainEventTypesCCHelper.MainEventTypesVisualsOC;
			set => _mainEventTypesCCHelper.MainEventTypesVisualsOC = value; 
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
				if(_selectedEventType == value) return;
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
		bool _isChangingEndTimes = false;
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				if(_startDateTime == value) return;
				_startDateTime = value;
				OnPropertyChanged();
				if (SelectedEventType != null && !_isChangingEndTimes)
				{
					SetEndExactTimeAccordingToEventType();
				}
                else if (_startDateTime > _endDateTime)
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
			}
		}
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				try
				{
					if(_endDateTime == value) return;
					_isChangingEndTimes = true;
					if (_startDateTime > value)
					{
						_endDateTime = _startDateTime = value;
						OnPropertyChanged(nameof(StartDateTime));
					}
					else
					{
						_endDateTime = value;
					}
					OnPropertyChanged();
					_isChangingEndTimes = false;
				}
				catch
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
				finally
				{
					_isChangingEndTimes = false;
				}
			}
		}
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				if (_startExactTime == value) return; // Avoid unnecessary setting and triggering.
				_startExactTime = value;
				OnPropertyChanged();
				if(SelectedEventType != null && !_isChangingEndTimes)
				{
					SetEndExactTimeAccordingToEventType();
				}
				else if (_startDateTime.Date == _endDateTime.Date && _startExactTime > _endExactTime)
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
			}
		}

		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				try
				{
					if (_endExactTime == value) return; // Avoid unnecessary setting and triggering.
					_isChangingEndTimes = true;
					_endExactTime = value;
					if (_startDateTime.Date == _endDateTime.Date && value < _startExactTime)
					{
						_startExactTime = value;
						OnPropertyChanged(nameof(StartExactTime));
					}
					OnPropertyChanged();
				}
				catch
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
				finally
				{
					_isChangingEndTimes = false;
				}
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
			IsCompleted = false;
			if(SelectedEventType.IsValueType)
			{
				QuantityValue = 0; 
			}
			// TODO Show POPUP ???
		}
		protected void OnUserEventTypeSelected(IUserEventTypeModel selectedEvent)
		{
			var lastSelectedTypedefaultValue = SelectedEventType?.QuantityAmount?.Value ?? 0;
			SelectedEventType = selectedEvent;
			IsMultiTaskTypeSelected = selectedEvent.IsMultiTaskType ? true : false;
			// TODO Show Task Options ???

			IsValueTypeSelected = selectedEvent.IsValueType ? true : false;
			if (IsValueTypeSelected)
			{
				_measurementSelectorHelperClass.SelectPropperMeasurementData(SelectedEventType);
				if (!IsEditMode && (QuantityValue == 0 || QuantityValue == lastSelectedTypedefaultValue))
				{   //Set default values when createMode
					QuantityValue = SelectedEventType.QuantityAmount.Value;
				}
				SelectedMeasurementUnit = _measurementSelectorHelperClass.SelectedMeasurementUnit;
			}



			SetEndExactTimeAccordingToEventType();
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
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(AllEventTypesOC); // ??????
			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(SelectedEventType.MainEventType);
			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
			SetSelectedEventTypeControlsVisibility();
		}
		protected virtual void OnMainEventTypeSelected(MainEventVisualDetails selectedMainEventType)
		{
			if (SelectedMainEventType == null || SelectedMainEventType.ToString() != selectedMainEventType.ToString())
			{
				_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(selectedMainEventType);
				SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
				FilterAllEventTypesOCByMainEventType(SelectedMainEventType);
			}
			if(AllEventTypesOC.Count > 0)
			{
				OnUserEventTypeSelected(AllEventTypesOC[0]);
			}
		}
		private void SetSelectedEventTypeControlsVisibility()
		{
			IsValueTypeSelected = SelectedEventType.IsValueType;
			IsMultiTaskTypeSelected = SelectedEventType.IsMultiTaskType;
		}
		private void SetEndExactTimeAccordingToEventType()
		{
			try
			{
				var timeSpanAdded = StartExactTime.Add(SelectedEventType.DefaultEventTimeSpan);

				// Calculate the number of whole days within the TimeSpan
				int days = (int)timeSpanAdded.TotalDays;

				// Calculate the remaining hours, minutes, and seconds after removing whole days
				TimeSpan remainingTime = TimeSpan.FromHours(timeSpanAdded.Hours).Add(TimeSpan.FromMinutes(timeSpanAdded.Minutes)).Add(TimeSpan.FromSeconds(timeSpanAdded.Seconds));

				// Set EndDateTime by adding whole days
				EndDateTime = StartDateTime.AddDays(days);

				// Set EndExactTime to the remaining hours, minutes, and seconds
				EndExactTime = remainingTime;
			}
			catch
			{
				EndExactTime = StartExactTime;
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
