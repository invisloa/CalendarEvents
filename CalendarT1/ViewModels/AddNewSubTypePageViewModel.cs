﻿using System;
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
using CalendarT1.Views.CustomControls.CCViewModels;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CalendarT1.Helpers;
using CalendarT1.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;

namespace CalendarT1.ViewModels
{
    public class AddNewSubTypePageViewModel : BaseViewModel
	{
		// TODO ! CHANGE THE BELOW CLASS TO VIEW MODEL 
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }
		public DefaultEventTimespanCCViewModel DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();
		public MeasurementSelectorCCViewModel DefaultMeasurementSelectorCCHelper { get; set; } = Factory.CreateNewMeasurementSelectorCCHelperClass();
		public MicroTasksListCCViewModel MicroTasksListCCHelper { get; set; }

		public IUserTypeExtraOptionsViewModel UserTypeExtraOptionsHelper { get; set; }

		public event Action<IMainEventType> MainEventTypeChanged;		// TODO implement this event ?? if needed??
		#region Fields
		private const int NoBorderSize = 0;
		private const int BorderSize = 7;
		private IMainEventTypesCC _mainEventTypesCCHelper;
		private TimeSpan _defaultEventTime;
		private ISubEventTypeModel _currentType;   // if null => add new type, else => edit type
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;
		private IEventRepository _eventRepository;
		List<MicroTaskModel> microTasksList = new List<MicroTaskModel>();
		#endregion

		#region Properties
		public string QuantityValueText => IsEdit ? "DEFAULT VALUE:" : "Value:";
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
		public ISubEventTypeModel CurrentType
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


		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }
		#endregion
		#region Commands
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; set; }
		public RelayCommand GoToAllSubTypesPageCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }
		#region Commands CanExecute
		private bool CanExecuteSubmitTypeCommand() => !string.IsNullOrEmpty(TypeName) && SelectedMainEventType != null;
		#endregion
		#endregion

		#region Constructors
		// constructor for create mode
		public AddNewSubTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			UserTypeExtraOptionsHelper = Factory.CreateNewUserTypeExtraOptionsHelperClass(false);
			DefaultEventTimespanCCHelper.SelectedUnitIndex = 2; // minutes
			DefaultEventTimespanCCHelper.DurationValue = 30;
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass(_eventRepository.AllMainEventTypesList);
			MicroTasksListCCHelper = Factory.CreateNewMicroTasksListCCHelperClass(microTasksList);
			InitializeCommon();
		}

		// constructor for edit mode
		public AddNewSubTypePageViewModel(IEventRepository eventRepository, ISubEventTypeModel currentType)
		{
			_eventRepository = eventRepository;
			MicroTasksListCCHelper = Factory.CreateNewMicroTasksListCCHelperClass(currentType.MicroTasksList);
			SelectedMainEventType = currentType.MainEventType;
			CurrentType = currentType;
			SelectedSubTypeColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			DefaultEventTimespanCCHelper.SetControlsValues(currentType.DefaultEventTimeSpan);
			setIsVisibleForExtraControlsInEditMode();
			InitializeCommon();

			// set proper visuals for an edited event type ??
		}

		private void InitializeCommon()
		{
			InitializeColorButtons();
			bool isEditMode = CurrentType != null;
			UserTypeExtraOptionsHelper = Factory.CreateNewUserTypeExtraOptionsHelperClass(isEditMode);
			SelectColorCommand = new RelayCommand<ButtonProperties>(OnSelectColorCommand);
			GoToAllSubTypesPageCommand = new RelayCommand(GoToAllSubTypesPage);
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitTypeCommand);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(OnMainEventTypeSelected);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
		}

		private void setIsVisibleForExtraControlsInEditMode()
		{
			UserTypeExtraOptionsHelper.IsValueTypeSelected = CurrentType.IsValueType;
			UserTypeExtraOptionsHelper.IsMicroTasksTypeSelected = CurrentType.IsMicroTaskType;
			UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected = CurrentType.DefaultEventTimeSpan != TimeSpan.Zero;
		}
		#endregion


		#region Methods
		private async Task DeleteSelectedEventType()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x == _currentType); // TODO !!! check if this works (equals)
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

			//await Shell.Current.GoToAsync($"{nameof(AllSubTypesPage)}");
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
				SetExtraUserControlsValues(_currentType);
				await _eventRepository.UpdateSubEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("..");								// TODO CHANGE NOT WORKING!!!
			}
			else
			{
				// TODO NOW !!!!!
				var timespan = UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected ? DefaultEventTimespanCCHelper.GetDefaultDuration() : TimeSpan.Zero;
				var quantityAmount = UserTypeExtraOptionsHelper.IsValueTypeSelected ? DefaultMeasurementSelectorCCHelper.QuantityAmount : null;
				var multiTasks = UserTypeExtraOptionsHelper.IsMicroTasksTypeSelected ? new List<MicroTaskModel>(MicroTasksListCCHelper.MicroTasksOC) : null;
				var newUserType = Factory.CreateNewEventType(SelectedMainEventType, TypeName, _selectedColor, timespan, quantityAmount, multiTasks);
				await _eventRepository.AddSubEventTypeAsync(newUserType);
				await Shell.Current.GoToAsync("..");    // TODO !!!!! CHANGE NOT WORKING!!!
			}
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
		private void GoToAllSubTypesPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllSubTypesPage());
		}
		private void InitializeColorButtons() //TODO ! also to extract as a separate custom control
		{
			ButtonsColorsInitializerHelperClass buttonsColorsInitializerHelperClass = new ButtonsColorsInitializerHelperClass(BorderSize);
			ButtonsColors = buttonsColorsInitializerHelperClass.ButtonsColors;
		}
		public void SetExtraUserControlsValues(ISubEventTypeModel _currentType)
		{
			if (_currentType == null)
			{
				return;
			}
			if (UserTypeExtraOptionsHelper.IsDefaultEventTimespanSelected)
			{
				_currentType.DefaultEventTimeSpan = DefaultEventTimespanCCHelper.GetDefaultDuration();
			}
			else
			{
				_currentType.DefaultEventTimeSpan = TimeSpan.Zero;
			}
			if (UserTypeExtraOptionsHelper.IsMicroTasksTypeSelected)
			{
				_currentType.IsMicroTaskType = true;
				_currentType.MicroTasksList = new List<MicroTaskModel>(MicroTasksListCCHelper.MicroTasksOC);
			}
			else
			{
				_currentType.IsMicroTaskType = false;
				_currentType.MicroTasksList = null;
			}
		}
	}
	#endregion

}














// in ctor
// not sure if helper class doing this	
// 		MeasurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);

// helper class doing this

//IsValueTypeSelectedCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
//IsSubTaskListTypeSelectedCommand = new RelayCommand(() => IsMicroTaskListSelected = !IsMicroTaskListSelected);
//IsDefaultTimespanSelectedCommand = new RelayCommand(() => IsDefaultEventTimespanSelected = !IsDefaultEventTimespanSelected);
//AddSubTaskEventCommand = new RelayCommand(OnAddSubTaskEventCommand, CanExecuteAddSubTaskEventCommand);
//SelectSubTaskCommand = new RelayCommand<MultiTask>(OnSubTaskSelected);