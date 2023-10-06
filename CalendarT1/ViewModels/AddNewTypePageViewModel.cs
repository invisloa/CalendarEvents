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
using CommunityToolkit.Mvvm.ComponentModel;
using CalendarT1.Helpers;

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
					AddSubTaskEventCommand.RaiseCanExecuteChanged();
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
					OnPropertyChanged(nameof(IsValueTypeSelected));
					OnPropertyChanged(nameof(IsValueTypeColor));
				}
			}
		}
		public TimeSpan DefaultTimespan
		{
			get
			{
				return IsDefaultEventTimespanSelected ? DefaultEventTimespanCCHelper.GetDefaultDuration() : TimeSpan.Zero;
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
		public RelayCommand<MultiTask> SelectSubTaskCommand { get; set; }
		
















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


		public event Action<IMainEventType> MainEventTypeChanged;		// TODO implement this event ?? if needed??
		#region Fields
		private const int NoBorderSize = 0;
		private const int BorderSize = 7;
		private IMainEventTypesCC _mainEventTypesCCHelper;
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

		public IMainEventType SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
			    SubmitTypeCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
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
			get; set;
		}

		public Color SelectedSubTypeColor
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
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }

		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }

		#endregion

		#region Commands
		public DefaultEventTimespanCCHelperClass DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAllTypesPageCommand { get; private set; }
		public AsyncRelayCommand TempRemoveAllUserTypesCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }
		#region Commands CanExecute

		private bool CanExecuteAddSubTaskEventCommand() => !string.IsNullOrEmpty(SubTaskToAddTitle);

		private bool CanExecuteSubmitTypeCommand() => !string.IsNullOrEmpty(TypeName) && SelectedMainEventType != null;
		#endregion
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
			SelectColorCommand = new RelayCommand<ButtonProperties>(OnSelectColorCommand);
			GoToAllTypesPageCommand = new RelayCommand(GoToAllTypesPage);
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitTypeCommand);
			MeasurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelected);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
			IsValueTypeSelectedCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
			IsSubTaskListTypeSelectedCommand = new RelayCommand(() => IsSubTaskListSelected = !IsSubTaskListSelected);
			IsDefaultTimespanSelectedCommand = new RelayCommand(() => IsDefaultEventTimespanSelected = !IsDefaultEventTimespanSelected);
			AddSubTaskEventCommand = new RelayCommand(OnAddSubTaskEventCommand, CanExecuteAddSubTaskEventCommand);
			SelectSubTaskCommand = new RelayCommand<MultiTask>(OnSubTaskSelected);
		}
		// constructor for edit mode
		public AddNewTypePageViewModel(IEventRepository eventRepository, IUserEventTypeModel currentType)
			: this(eventRepository)
		{
			SelectedMainEventType = currentType.MainEventType;
			CurrentType = currentType;
			SelectedSubTypeColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			DefaultEventTimespanCCHelper.SetControlsValues(currentType.DefaultEventTimeSpan);
			setIsVisibleForExtraControlsForEditMode(currentType);
			// set proper visuals for an edited event type
		}

		#endregion


		#region Methods
		private void OnAddSubTaskEventCommand()
		{
			SubTasksListOC.Add(new MultiTask(SubTaskToAddTitle));
			SubTaskToAddTitle = String.Empty;
		}
		private void OnSubTaskSelected(MultiTask multiTask)
		{
			multiTask.IsSubTaskCompleted = !multiTask.IsSubTaskCompleted;
			OnPropertyChanged(nameof(SubTasksListOC));
		}

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
				// cannot change main event, Quantity type => may lead to some future errors???
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = _selectedColor;
				SetExtraUserControlsAccordingToSelections();
				await _eventRepository.UpdateSubEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("..");								// TODO CHANGE NOT WORKING!!!
				
			}
			else
			{
				Quantity quantityAmount = IsValueTypeSelected ? new Quantity(SelectedMeasurementUnit.TypeOfMeasurementUnit, QuantityValue) : null;
				List<MultiTask> multiTasks = IsSubTaskListSelected ? new List<MultiTask>(SubTasksListOC) : null;

				var newUserType = Factory.CreateNewEventType(SelectedMainEventType, TypeName, _selectedColor, DefaultTimespan, quantityAmount, multiTasks);
				await _eventRepository.AddSubEventTypeAsync(newUserType);
				await Shell.Current.GoToAsync("..");    // TODO CHANGE NOT WORKING!!!
			}
		}
		// doing
		private void setIsVisibleForExtraControlsForEditMode(IUserEventTypeModel userEventTypeModel)
		{
			IsValueTypeSelected = userEventTypeModel.IsValueType;
			IsSubTaskListSelected = userEventTypeModel.IsMultiTaskType;
			IsDefaultEventTimespanSelected = userEventTypeModel.DefaultEventTimeSpan != TimeSpan.Zero;
		}

		private void OnMainEventTypeSelected(MainEventTypeViewModel selectedMainEventType)
		{
			((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypeSelectedCommand.Execute(selectedMainEventType);

			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
		}
		private void OnSelectColorCommand(ButtonProperties selectedColor)
		{
			SelectedSubTypeColor = selectedColor.ButtonColor;

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
			ButtonsColorsInitializerHelperClass buttonsColorsInitializerHelperClass = new ButtonsColorsInitializerHelperClass(BorderSize);
			ButtonsColors = buttonsColorsInitializerHelperClass.ButtonsColors;
		}
		private void SetExtraUserControlsAccordingToSelections()
		{
			if (IsDefaultEventTimespanSelected)
			{
				_currentType.DefaultEventTimeSpan = DefaultTimespan;
			}
			else
			{
				_currentType.DefaultEventTimeSpan = TimeSpan.Zero;
			}
			if (IsSubTaskListSelected)
			{
				_currentType.IsMultiTaskType = true;
				_currentType.MultiTasksList = new List<MultiTask>(SubTasksListOC);
			}
			else
			{
				_currentType.IsMultiTaskType = false;
				_currentType.MultiTasksList = null;
			}
		}

	}
	#endregion

}