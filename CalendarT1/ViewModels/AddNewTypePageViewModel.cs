using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using CalendarT1.Models.EventTypesModels;
using Newtonsoft.Json;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using CalendarT1.Views;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CalendarT1.Models.EventModels;
using CalendarT1.Views.CustomControls.CCHelperClass;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
    public class AddNewTypePageViewModel : BaseViewModel, IMainEventTypesCC, IMeasurementSelectorCC
	{
		private Color _deselectedColor = (Color)Application.Current.Resources["DeselectedBackgroundColor"];
		private bool _isValueTypeSelected;
		private string _subtaskToAddTitle;
		public string SubTaskToAddTitle
		{
			get => _subtaskToAddTitle;
			set
			{
				if (_subtaskToAddTitle != value)
				{
					_subtaskToAddTitle = value;
					OnPropertyChanged();
				}
			}
		}
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				if (_isValueTypeSelected != value)
				{
					_isValueTypeSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsValueTypeColor));
				}
			}
		}
		private bool _isSubTaskListSelected;
		public bool IsSubTaskListSelected
		{
			get => _isSubTaskListSelected;
			set
			{
				if (_isSubTaskListSelected != value)
				{
					_isSubTaskListSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsSubTaskListTypeColor));
				}
			}
		}
		private bool _isDefaultEventTimespanSelected;
		public bool IsDefaultEventTimespanSelected
		{
			get => _isDefaultEventTimespanSelected;
			set
			{
				if (_isDefaultEventTimespanSelected != value)
				{
					_isDefaultEventTimespanSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsDefaultTimespanColor));
				}
			}
		}


		public Color IsValueTypeColor
		{
			get
			{
				if (IsValueTypeSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public Color IsSubTaskListTypeColor
		{
			get
			{
				if (IsSubTaskListSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public Color IsDefaultTimespanColor
		{
			get
			{
				if (IsDefaultEventTimespanSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public ObservableCollection<MultiTask> SubTasksListOC { get; set; } = new ObservableCollection<MultiTask>();

		public RelayCommand AddSubTaskEventCommand { get; set; } 
		
















		#region MeasurementCC implementation
		public int ValueFontSize { get; set; } = 20;

		IMeasurementSelectorCC _measurementSelectorHelperClass { get; set; } = Factory.CreateMeasurementSelectorCCHelperClass();

		public void SelectPropperMeasurementData(IUserEventTypeModel userEventTypeModel)
		{
			_measurementSelectorHelperClass.SelectPropperMeasurementData(userEventTypeModel);
		}

		public decimal QuantityValue { get => _measurementSelectorHelperClass.QuantityValue; set => _measurementSelectorHelperClass.QuantityValue = value; }

		public MeasurementUnitItem SelectedMeasurementUnit
		{
			get => _measurementSelectorHelperClass.SelectedMeasurementUnit;
			set
			{
				_measurementSelectorHelperClass.SelectedMeasurementUnit = value;
			}
		}
		public ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC => _measurementSelectorHelperClass.MeasurementUnitsOC;
		public Quantity QuantityAmount { get => _measurementSelectorHelperClass.QuantityAmount; set => _measurementSelectorHelperClass.QuantityAmount = value; }
		private void OnMeasurementUnitSelected(MeasurementUnitItem measurementUnitItem)
		{
			_measurementSelectorHelperClass.SelectedMeasurementUnit = measurementUnitItem;
			_measurementSelectorHelperClass.QuantityAmount = new Quantity(measurementUnitItem.TypeOfMeasurementUnit, _measurementSelectorHelperClass.QuantityValue) ;
		}

		// Command set in the constructor
		public RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand { get; set; } 
		public RelayCommand IsValueTypeSelectedCommand { get; set; }
		public RelayCommand IsSubTaskListTypeSelectedCommand { get; set; }
		public RelayCommand IsDefaultTimespanSelectedCommand { get; set; }
		

		#endregion

		public DefaultEventTimespanCCHelperClass DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();

		public event Action<IMainEventType> MainEventTypeChanged;		// TODO implement this event ?? if needed??


		#region Fields
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;

		private TimeSpan _defaultEventTime;
		private IUserEventTypeModel _currentType;   // if null => add new type, else => edit type
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;
		private IEventRepository _eventRepository;
		#endregion

		#region Properties
		public string QuantityValueText { get; set; } = "DEFAULT VALUE:";
		public string PageTitle => IsEdit ? "EDIT TYPE" : "ADD NEW TYPE";
		public string PlaceholderText => IsEdit ? $"TYPE NEW NAME FOR: {TypeName}" : "...NEW TYPE NAME...";
		public string SubmitButtonText => IsEdit ? "SUBMIT CHANGES" : "ADD NEW TYPE";
		public bool IsEdit => _currentType != null;
		public bool IsNotEdit => !IsEdit;
		public TimeSpan DefaultEventTimeSpan
		{
			get => _defaultEventTime;
			set
			{
				if (_defaultEventTime == value) return;
				_defaultEventTime = value;
			}
		}
		public IMainEventType SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
			}
		}
		public IUserEventTypeModel CurrentType
		{
			get => _currentType;
			set
			{
				if (value == _currentType) return;
				_currentType = value;
				OnPropertyChanged();
			}
		}

		public Color MainEventTypeButtonsColor
		{
			get => _selectedColor;
			set
			{
				if (value == _selectedColor) return;
				_selectedColor = value;
				OnPropertyChanged();
			}
		}
		public string TypeName
		{
			get => _typeName;
			set
			{
				if (value == _typeName) return;
				_typeName = value;
				SubmitTypeCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
			}
		}

		// helper class that makes dirty work for main event types
		private IMainEventTypesCC _mainEventTypesCCHelper { get; set; }

		public ObservableCollection<MainEventVisualDetails> MainEventTypesVisualsOC { get => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }

		public RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; set; }
		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }

		#endregion

		#region Commands
		public RelayCommand GoToAllTypesPageCommand { get; private set; }
		public AsyncRelayCommand TempRemoveAllUserTypesCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }
		#endregion

		#region Constructors
		// constructor for create mode
		public AddNewTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass(_eventRepository.AllMainEventTypesList);
			InitializeColorButtons();
			DefaultEventTimespanCCHelper.DurationValue = 30;
			DefaultEventTimespanCCHelper.SelectedUnitIndex = 2;
			SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
			GoToAllTypesPageCommand = new RelayCommand(GoToAllTypesPage);
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitTypeCommand);
			MeasurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventVisualDetails>(OnMainEventTypeSelected);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
			TempRemoveAllUserTypesCommand = new AsyncRelayCommand(TempRemoveAllUserTypes);
			IsValueTypeSelectedCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
			IsSubTaskListTypeSelectedCommand = new RelayCommand(() => IsSubTaskListSelected = !IsSubTaskListSelected);
			IsDefaultTimespanSelectedCommand = new RelayCommand(() => IsDefaultEventTimespanSelected = !IsDefaultEventTimespanSelected);
			AddSubTaskEventCommand = new RelayCommand(() => { SubTasksListOC.Add(new MultiTask(SubTaskToAddTitle)); });
		}
		// constructor for edit mode
		public AddNewTypePageViewModel(IEventRepository eventRepository, IUserEventTypeModel currentType)
			: this(eventRepository)
		{
			CurrentType = currentType;
			MainEventTypeButtonsColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			DefaultEventTimeSpan = currentType.DefaultEventTimeSpan;
			// set proper visuals for an edited event type
		}
		#endregion


		#region Methods
		private bool CanExecuteSubmitTypeCommand() => !string.IsNullOrEmpty(TypeName);
		private async Task DeleteSelectedEventType()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x.EventType.EventTypeName == _currentType.EventTypeName);
			if (eventTypesInDb.Any())
			{
				var action = await App.Current.MainPage.DisplayActionSheet("This type is used in some events.", "Cancel", null, "Delete all associated events", "Go to All Events Page");
				switch (action)
				{
					case "Delete all associated events":
						// Perform the operation to delete all events of the event type.
						_eventRepository.AllEventsList.RemoveAll(x => x.EventType.EventTypeName == _currentType.EventTypeName);
						await _eventRepository.SaveEventsListAsync();
						await _eventRepository.DeleteFromSubEventTypesListAsync(_currentType);
						// TODO make a confirmation message
						break;
					case "Go to All Events Page":
						// Redirect to the All Events Page.
						await Shell.Current.GoToAsync("ViewAllEventsPage");
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;
			}
			await _eventRepository.DeleteFromSubEventTypesListAsync(_currentType);
			await Shell.Current.Navigation.PopAsync();

			//await Shell.Current.GoToAsync($"{nameof(AllTypesPage)}");
		}
		public void DisableVisualsForAllMainEventTypes()
		{
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
		}
		private async Task SubmitType()
		{
			if (IsEdit)
			{
				// cannot change main event type => may lead to some future errors???
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = MainEventTypeButtonsColor;
				if(IsDefaultEventTimespanSelected)
				{
					_currentType.DefaultEventTimeSpan = DefaultEventTimespanCCHelper.GetDefaultDuration();
				}
				else
				{
					_currentType.DefaultEventTimeSpan = TimeSpan.Zero;
				}
				await _eventRepository.UpdateSubEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("..");								// TODO CHANGE NOT WORKING!!!
				
			}
			else
			{
				Quantity quantityAmount = null;
				if (IsValueTypeSelected)
				{
					quantityAmount = new Quantity(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue);
				}
				List<MultiTask> multiTasks = null;
				if (IsSubTaskListSelected)
				{
					multiTasks = new List<MultiTask>(SubTasksListOC); // TODO ADD multiTasks to 
				}
				var newUserType = Factory.CreateNewEventType(SelectedMainEventType, TypeName, _selectedColor, DefaultEventTimespanCCHelper.GetDefaultDuration(), quantityAmount, multiTasks);
				await _eventRepository.AddSubEventTypeAsync(newUserType);
				await Shell.Current.GoToAsync("..");    // TODO CHANGE NOT WORKING!!!
			}
		}


		private void OnMainEventTypeSelected(MainEventVisualDetails selectedMainEventType)
		{
			((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypeSelectedCommand.Execute(selectedMainEventType);

			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
		}

		private async Task TempRemoveAllUserTypes()
		{
			await _eventRepository.ClearAllSubEventTypesAsync();
		}
		private void SelectColor(ButtonProperties selectedColor)
		{
			MainEventTypeButtonsColor = selectedColor.ButtonColor;

			foreach (var button in ButtonsColors)
			{
				button.ButtonBorder = button == selectedColor ? NoBorderSize : BorderSize;
			}
		}
		private void GoToAllTypesPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllTypesPage());
		}
		private void InitializeColorButtons() // also to extract as a separate custom control
		{
			ButtonsColors = new ObservableCollection<ButtonProperties>
			{
				// Green shades
				new ButtonProperties { ButtonColor = Color.FromRgb(102, 205, 170), ButtonBorder = BorderSize}, // MediumAquamarine
				new ButtonProperties { ButtonColor = Color.FromRgb(127, 255, 212), ButtonBorder = BorderSize}, // Aquamarine
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 250, 154), ButtonBorder = BorderSize}, // MediumSpringGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(152, 251, 152), ButtonBorder = BorderSize}, // PaleGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(130, 255, 154), ButtonBorder = BorderSize}, // LightGreen

				// Lively Green shades
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 127), ButtonBorder = BorderSize}, // SpringGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(60, 179, 113), ButtonBorder = BorderSize}, // MediumSeaGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(50, 205, 50), ButtonBorder = BorderSize}, // LimeGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(124, 252, 0), ButtonBorder = BorderSize}, // LawnGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(107, 142, 35), ButtonBorder = BorderSize}, // OliveDrab

				// Blue shades
				new ButtonProperties { ButtonColor = Color.FromRgb(173, 216, 230), ButtonBorder = BorderSize}, // LightBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(176, 224, 230), ButtonBorder = BorderSize}, // PowderBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(135, 206, 250), ButtonBorder = BorderSize}, // LightSkyBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(100, 149, 237), ButtonBorder = BorderSize}, // CornflowerBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(70, 130, 180), ButtonBorder = BorderSize}, // SteelBlue

				// Vibrant Blue shades
				new ButtonProperties { ButtonColor = Color.FromRgb(30, 144, 255), ButtonBorder = BorderSize}, // DodgerBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 191, 255), ButtonBorder = BorderSize}, // DeepSkyBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(65, 105, 225), ButtonBorder = BorderSize}, // RoyalBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(25, 25, 112), ButtonBorder = BorderSize}, // MidnightBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 205), ButtonBorder = BorderSize}, // MediumBlue

				// Pink shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 182, 193), ButtonBorder = BorderSize}, // LightPink
				new ButtonProperties { ButtonColor = Color.FromRgb(219, 112, 147), ButtonBorder = BorderSize}, // PaleVioletRed
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 105, 180), ButtonBorder = BorderSize}, // HotPink
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 20, 147), ButtonBorder = BorderSize}, // DeepPink
				new ButtonProperties { ButtonColor = Color.FromRgb(199, 21, 133), ButtonBorder = BorderSize}, // MediumVioletRed

				// Orange shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 160, 122), ButtonBorder = BorderSize}, // LightSalmon
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 165, 0), ButtonBorder = BorderSize}, // Orange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 140, 0), ButtonBorder = BorderSize}, // DarkOrange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 99, 71), ButtonBorder = BorderSize}, // Tomato
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 69, 0), ButtonBorder = BorderSize}, // OrangeRed

				// Red shades
				new ButtonProperties { ButtonColor = Color.FromRgb(240, 128, 128), ButtonBorder = BorderSize}, // LightCoral
				new ButtonProperties { ButtonColor = Color.FromRgb(205, 92, 92), ButtonBorder = BorderSize}, // IndianRed
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), ButtonBorder = BorderSize}, // Red
				new ButtonProperties { ButtonColor = Color.FromRgb(220, 20, 60), ButtonBorder = BorderSize}, // Crimson
				new ButtonProperties { ButtonColor = Color.FromRgb(178, 34, 34), ButtonBorder = BorderSize}, // Firebrick

				// Additional Red shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 85, 85), ButtonBorder = BorderSize}, // Valentine Red
				new ButtonProperties { ButtonColor = Color.FromRgb(250, 52, 57), ButtonBorder = BorderSize}, // Ruby Red
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 40, 0), ButtonBorder = BorderSize}, // Strong Red
				new ButtonProperties { ButtonColor = Color.FromRgb(205, 0, 0), ButtonBorder = BorderSize}, // Bright Red
				new ButtonProperties { ButtonColor = Color.FromRgb(153, 0, 0), ButtonBorder = BorderSize}, // Rich Red

			};
		}

	}
	#endregion


	#region Helper Classes

	public class ButtonProperties : BaseViewModel
	{
		private int _borderSize;
		[JsonIgnore]
		public Color ButtonColor { get; set; }
		public int ButtonBorder
		{
			get => _borderSize;
			set
			{
				_borderSize = value;
				OnPropertyChanged();
			}
		}
		public string ButtonColorRgb        // when deserialized it will not fire OnPropertyChanged on ButtonColor to check later if its ok TOCHECK
		{
			get { return $"{ButtonColor.Red}, {ButtonColor.Green}, {ButtonColor.Blue}"; }
			set
			{
				var rgbValues = value.Split(',').Select(int.Parse).ToArray();
				ButtonColor = Color.FromRgb(rgbValues[0], rgbValues[1], rgbValues[2]);
			}
		}
	}
	#endregion
}