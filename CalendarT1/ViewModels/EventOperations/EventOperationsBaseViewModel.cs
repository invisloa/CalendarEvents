using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views.CustomControls;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
    public abstract class EventOperationsBaseViewModel : BaseViewModel
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
		public RelayCommand<EventVisualDetails> MainEventTypeSelectedCommand { get; set; }

		#region Properties
		private IMainEventTypesCC _mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass();
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _currentEvent;
		protected bool _isCompleted;
		protected string _title;
		protected string _description;
		protected Quantity _quantityAmount;
		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		protected string _submitButtonText;
		protected AsyncRelayCommand _submitEventCommand;
		protected Color _mainEventTypeButtonColor;
		protected List<IUserEventTypeModel> _allUserTypesForVisuals;

		MeasurementUnit _measurementUnit;
		public RelayCommand<IUserEventTypeModel> SelectUserEventTypeCommand { get; set; }

		public MainEventTypes SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				var tempFilteredEventTypes = _allUserTypesForVisuals.FindAll(x => x.MainEventType == value);
				AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(tempFilteredEventTypes);
				OnPropertyChanged(nameof(AllEventTypesOC));
				OnPropertyChanged();
			}
		}


		public string EventTypePickerText { get => "Select event Type"; }

		// Basic Event Information
		#region
		public ObservableCollection<EventVisualDetails> MainEventTypesOC { get => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesOC; set => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesOC = value; }

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
		public Quantity QuantityAmount
		{
			get => _quantityAmount;
			set
			{
				_quantityAmount = value;
				OnPropertyChanged();
			}
		}
		public MeasurementUnit MeasurementUnit
		{
			get => _measurementUnit;
			set
			{
				_measurementUnit = value;
				OnPropertyChanged(nameof(MeasurementUnit));
			}
		}
		public abstract string SubmitButtonText { get; set; }

		#endregion

		// Command
		public AsyncRelayCommand SubmitEventCommand => _submitEventCommand;
		#endregion

		protected ObservableCollection<IUserEventTypeModel> _eventTypesOC;
		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}
		protected ObservableCollection<MeasurementUnit> _measurementUnits;
		public ObservableCollection<MeasurementUnit> MeasurementUnits
		{
			get => _measurementUnits;
			set
			{
				_measurementUnits = value;
				OnPropertyChanged();
			}
		}

		protected ObservableCollection<IGeneralEventModel> _allEventsListOC;
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}

		protected IUserEventTypeModel _selectedEventType;
		public IUserEventTypeModel SelectedEventType
		{
			get => _selectedEventType;
			set
			{
				_selectedEventType = value;
				MainEventTypeButtonsColor = _selectedEventType.EventTypeColor;
				_mainEventTypesCCHelper.SelectedMainEventType = _selectedEventType.MainEventType;
				if (_selectedEventType.MainEventType == MainEventTypes.Value)
				{
					MeasurementUnits = new ObservableCollection<MeasurementUnit>(Enum.GetValues(typeof(MeasurementUnit)).Cast<MeasurementUnit>());
				}
				OnPropertyChanged();
			}
		}

		// Helper Methods
		#region Date adjusting methods
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
		}
		protected void OnUserEventTypeSelected(IUserEventTypeModel selectedEvent)
		{
			SelectedEventType = selectedEvent;
			SetVisualsForSelectedUserType();
		}
		protected void SetVisualsForSelectedUserType()
		{
			foreach (var eventType in AllEventTypesOC)
			{
				eventType.BackgroundColor = Color.FromRgba(255, 255, 255, 1);
			}
			SelectedEventType.BackgroundColor = SelectedEventType.EventTypeColor;
		}
		public void UpdateAllEventsList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}
		public void UpdateAllEventTypesList()
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}
		protected void OnMainEventTypeSelected(EventVisualDetails eventType)
		{
			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(eventType);
			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
			OnUserEventTypeSelected(AllEventTypesOC[0]);
		}
		#endregion

	}
}
