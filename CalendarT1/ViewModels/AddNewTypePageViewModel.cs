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
using CalendarT1.Views.CustomControls;

namespace CalendarT1.ViewModels
{
	public class AddNewTypePageViewModel : BaseViewModel, IMainEventTypesCC
	{
		#region Fields
		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;
		public MainEventTypes SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
			}
		}
		private MainEventTypes _selectedEventType = MainEventTypes.Event; // initialize for create mode
		private IUserEventTypeModel _currentType;   // if null => add new type, else => edit type
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;
		private IEventRepository _eventRepository;
		#endregion

		#region Properties
		public string PageTitle => IsEdit ? "EDIT TYPE" : "ADD NEW TYPE";
		public string PlaceholderText => IsEdit ? $"TYPE NEW NAME FOR: {TypeName}" : "NEW TYPE NAME";
		public string SubmitButtonText => IsEdit ? "SUBMIT CHANGES" : "ADD NEW TYPE";
		public bool IsEdit => _currentType != null;
		public bool IsNotEdit => !IsEdit;

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
		private IMainEventTypesCC _mainEventTypesCCHelper { get; set; } = Factory.CreateNewIMainEventTypeHelperClass();
		public ObservableCollection<EventVisualDetails> MainEventTypesOC { get => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesOC; set => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypesOC = value; }

		public RelayCommand<EventVisualDetails> MainEventTypeSelectedCommand => ((IMainEventTypesCC)_mainEventTypesCCHelper).MainEventTypeSelectedCommand;
		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }

		#endregion

		#region Commands
		public RelayCommand GoToAllTypesPageCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }

		#endregion

		#region Constructor
		// constructor for create mode
		public AddNewTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			InitializeColorButtons();
			SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
			GoToAllTypesPageCommand = new RelayCommand(GoToAllTypesPage);
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitCommand);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
		}
		// constructor for edit mode
		public AddNewTypePageViewModel(IEventRepository eventRepository, IUserEventTypeModel currentType)
			: this(eventRepository)
		{
			CurrentType = currentType;
			_selectedEventType = currentType.MainEventType;
			MainEventTypeButtonsColor = currentType.EventTypeColor;
			TypeName = currentType.EventTypeName;
			// set proper visuals for an edited event type
		}
		#endregion
		#region Methods
		private bool CanExecuteSubmitCommand() => !string.IsNullOrEmpty(TypeName);
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
						await _eventRepository.DeleteFromUserEventTypesListAsync(_currentType);
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
			await _eventRepository.DeleteFromUserEventTypesListAsync(_currentType);
			await Shell.Current.GoToAsync("..");
		}

		private async Task SubmitType()
		{
			if (IsEdit)
			{
				// cannot change main event type => may lead to some future errors???
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = MainEventTypeButtonsColor;
				await _eventRepository.UpdateEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("..");								// TODO CHANGE NOT WORKING!!!
				
			}
			else
			{
				var newUserType = Factory.CreateNewEventType(_selectedEventType, TypeName, _selectedColor);
				await _eventRepository.AddUserEventTypeAsync(newUserType);			
				await Shell.Current.GoToAsync("..");                                // TODO CHANGE NOT WORKING!!!
			}
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
			Application.Current.MainPage.Navigation.PushAsync(new AllTypesPage(_eventRepository));
		}
		private void InitializeColorButtons()		// also to extract as a separate custom control
		{
			ButtonsColors = new ObservableCollection<ButtonProperties>
			{

				new ButtonProperties { ButtonColor = Color.FromRgb(144, 238, 144), ButtonBorder = BorderSize}, // LightGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(60, 179, 113), ButtonBorder = BorderSize}, // MediumSeaGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(34, 139, 34), ButtonBorder = BorderSize}, // ForestGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), ButtonBorder = BorderSize}, // Lime
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 100, 0), ButtonBorder = BorderSize}, // DarkGreen
    
				// Blue shades
				new ButtonProperties { ButtonColor = Color.FromRgb(135, 206, 235), ButtonBorder = BorderSize}, // SkyBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(70, 130, 180), ButtonBorder = BorderSize}, // SteelBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 255), ButtonBorder = BorderSize}, // Blue
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 205), ButtonBorder = BorderSize}, // MediumBlue
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 128), ButtonBorder = BorderSize}, // Navy
    
				// Red shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 99, 71), ButtonBorder = BorderSize}, // Tomato
				new ButtonProperties { ButtonColor = Color.FromRgb(220, 20, 60), ButtonBorder = BorderSize}, // Crimson
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), ButtonBorder = BorderSize}, // Red
				new ButtonProperties { ButtonColor = Color.FromRgb(139, 0, 0), ButtonBorder = BorderSize}, // DarkRed
				new ButtonProperties { ButtonColor = Color.FromRgb(128, 0, 0), ButtonBorder = BorderSize}, // Maroon
    
				// Violet shades
				new ButtonProperties { ButtonColor = Color.FromRgb(238, 130, 238), ButtonBorder = BorderSize}, // Violet
				new ButtonProperties { ButtonColor = Color.FromRgb(221, 160, 221), ButtonBorder = BorderSize}, // Plum
				new ButtonProperties { ButtonColor = Color.FromRgb(138, 43, 226), ButtonBorder = BorderSize}, // BlueViolet
				new ButtonProperties { ButtonColor = Color.FromRgb(153, 50, 204), ButtonBorder = BorderSize}, // DarkOrchid
				new ButtonProperties { ButtonColor = Color.FromRgb(148, 0, 211), ButtonBorder = BorderSize}, // DarkViolet
    
				// Orange shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 160, 122), ButtonBorder = BorderSize},  // LightSalmon
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 165, 0), ButtonBorder = BorderSize}, // Orange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 140, 0), ButtonBorder = BorderSize}, // DarkOrange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 69, 0), ButtonBorder = BorderSize}, // OrangeRed
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 30, 0), ButtonBorder = BorderSize}, // OrangeRed
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