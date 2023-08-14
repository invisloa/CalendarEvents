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

namespace CalendarT1.ViewModels
{
	public class AddNewTypePageViewModel : BaseViewModel
	{
		#region Fields
		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;

		// each event type has its EventDetails object assigned which contains visuals info(so its easy to change visuals on selection)
		private readonly Dictionary<MainEventTypes, EventDetails> _eventVisualDetails = new Dictionary<MainEventTypes, EventDetails>();
		private MainEventTypes _selectedEventType = MainEventTypes.Event;
		private IUserEventTypeModel _currentType;
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

		public Color SelectedColor
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
		public ObservableCollection<EventDetails> MainEventTypesOC { get; set; }
		#endregion

		#region Commands
		public RelayCommand GoToAllTypesPageCommand { get; private set; }
		public RelayCommand<EventDetails> MainEventTypeSelectedCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }
		#endregion

		#region Constructor
		public AddNewTypePageViewModel(IEventRepository eventRepository, IUserEventTypeModel currentType = null)
		{
			_eventRepository = eventRepository;
			InitializeColorButtons();
			MainEventTypeSelectedCommand = new RelayCommand<EventDetails>(ConvertEventDetailsAndSelectType);
			SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
			GoToAllTypesPageCommand = new RelayCommand(GoToAllTypesPage);
			InitializeMainEventTypes();
			SubmitTypeCommand = new AsyncRelayCommand(SubmitType, CanExecuteSubmitCommand);
			DeleteSelectedEventTypeCommand = new AsyncRelayCommand(DeleteSelectedEventType);
			if (currentType != null)
			{
				CurrentType = currentType;
				_selectedEventType = currentType.MainType;
				SelectedColor = currentType.EventTypeColor;
				TypeName = currentType.EventTypeName;
				// set propper visuals for a edited event type
			}
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
		private void ConvertEventDetailsAndSelectType(EventDetails selectedEventTypeDetails)
		{
			if (!Enum.TryParse(selectedEventTypeDetails.MainEventNameText, out MainEventTypes parsedTypeOfEvent))
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {selectedEventTypeDetails.MainEventNameText}");
			}

			SetSelectedEventType(parsedTypeOfEvent);
		}
		private async Task SubmitType()
		{
			if (IsEdit)
			{
				// cannot change main event type => may lead to some future errors???
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = SelectedColor;
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
		private void SetSelectedEventType(MainEventTypes eventType)
		{
			_selectedEventType = eventType;

			foreach (var eventDetail in _eventVisualDetails.Values)
			{
				eventDetail.Opacity = FadedOpacity;
				eventDetail.Border = BorderSize;
			}

			//for selected event type set different visuals
			_eventVisualDetails[eventType].Opacity = FullOpacity;
			_eventVisualDetails[eventType].Border = NoBorderSize;

			// Force update of the ObservableCollection
			MainEventTypesOC = new ObservableCollection<EventDetails>(_eventVisualDetails.Values);
		}
		private void SelectColor(ButtonProperties selectedColor)
		{
			SelectedColor = selectedColor.ButtonColor;

			foreach (var button in ButtonsColors)
			{
				button.ButtonBorder = button == selectedColor ? NoBorderSize : BorderSize;
			}
		}
		private void GoToAllTypesPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllTypesPage(_eventRepository));
		}
		private void InitializeColorButtons()
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
		// ... other methods

		private void InitializeMainEventTypes()
		{
			MainEventTypesOC = new ObservableCollection<EventDetails>();

			// dynamically create Main Event Types according to enum
			foreach (MainEventTypes eventType in Enum.GetValues(typeof(MainEventTypes)))
			{
				var eventDetails = new EventDetails
				{
					MainEventNameText = eventType.ToString(),
					Opacity = FadedOpacity,
					Border = BorderSize
				};

				_eventVisualDetails[eventType] = eventDetails;
				MainEventTypesOC.Add(eventDetails);
			}

			SetSelectedEventType(_selectedEventType);   // if create mode it is event by default, if edit it is the type of the event
		}
	}
	#endregion


	#region Helper Classes
	public class EventDetails : BaseViewModel
	{
		private string _mainEventNameText;
		private float _opacity;
		private int _border;

		public string MainEventNameText
		{
			get => _mainEventNameText;
			set { _mainEventNameText = value; OnPropertyChanged(); }
		}
		public float Opacity
		{
			get => _opacity;
			set { _opacity = value; OnPropertyChanged(); }
		}
		public int Border
		{
			get => _border;
			set { _border = value; OnPropertyChanged(); }
		}
	}

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