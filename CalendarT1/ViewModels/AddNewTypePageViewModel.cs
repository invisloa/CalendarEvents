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

namespace CalendarT1.ViewModels
{
	public class AddNewTypePageViewModel : BaseViewModel
	{
		public string PageTitle => IsEdit ? "EDIT TYPE" : "ADD NEW TYPE";								// idk
		public string PlaceholderText => IsEdit ? $"TYPE NEW NAME FOR: {TypeName}" : "NEW TYPE NAME";   // top of the page TextBox
		public string SubmitButtonText => IsEdit ? "SUBMIT CHANGES" : "ADD NEW TYPE";		
		private Color _selectedColor = Color.FromRgb(255, 0, 0);                                        // initialize with red
		private string _typeName;
		public bool IsEdit => _currentType != null;
		private readonly Dictionary<MainEventTypes, EventDetails> _eventVisualDetails = new Dictionary<MainEventTypes, EventDetails>();
		private MainEventTypes _selectedEventType = MainEventTypes.Event;
		IEventRepository _eventRepository;
		private IUserEventTypeModel _currentType;
		public bool IsNotEdit => !IsEdit;
		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;
		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }
		public ObservableCollection<EventDetails> MainEventTypesOC { get; set; }
		public RelayCommand GoToAllTypesPageCommand { get; private set; }

		public IUserEventTypeModel CurrentType
		{
			get => _currentType;
			set
			{
				if (value == _currentType)
				{
					return;
				}
				_currentType = value;
				OnPropertyChanged();
			}
		}

		public Color SelectedColor
		{
			get => _selectedColor;
			set
			{
				if (value == _selectedColor)
				{
					return;
				}
				_selectedColor = value;
				OnPropertyChanged();
			}
		}

		public string TypeName
		{
			get => _typeName;
			set
			{
				if (value == _typeName)
				{
					return;
				}
				_typeName = value;
				SubmitTypeCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
			}
		}

		public RelayCommand<EventDetails> EventTypeSelectedCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
		public AsyncRelayCommand SubmitTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteSelectedEventTypeCommand { get; private set; }

		private bool CanExecuteSubmitCommand() => !string.IsNullOrEmpty(TypeName);

		private async Task DeleteSelectedEventType()
		{
			await _eventRepository.DeleteFromUserEventTypesListAsync(_currentType);
			await Shell.Current.GoToAsync("..");
		}

		private async Task SubmitType()
		{
			if (IsEdit)
			{
				// cannot change main event type => may lead to some future errors???
				_currentType.EventTypeName = TypeName;
				_currentType.EventTypeColor = SelectedColor;
				await _eventRepository.UpdateEventTypeAsync(_currentType);
				await Shell.Current.GoToAsync("..");
			}
			else
			{
				var newUserType = Factory.CreateNewEventType(_selectedEventType, TypeName, _selectedColor);
				await _eventRepository.AddUserEventTypeAsync(newUserType);
				await Shell.Current.GoToAsync("..");
			}
		}
		public AddNewTypePageViewModel(IEventRepository eventRepository, IUserEventTypeModel currentType = null)
		{
			_eventRepository = eventRepository;
			InitializeColorButtons();
			EventTypeSelectedCommand = new RelayCommand<EventDetails>(ConvertEventDetailsAndSelectType);
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
		private void InitializeMainEventTypes()
		{
			MainEventTypesOC = new ObservableCollection<EventDetails>();

			// dynamically create Main Event Types according to enum
			foreach (MainEventTypes eventType in Enum.GetValues(typeof(MainEventTypes)))
			{
				var eventDetails = new EventDetails
				{
					Text = eventType.ToString(),
					Opacity = FadedOpacity,
					Border = BorderSize
				};

				_eventVisualDetails[eventType] = eventDetails;
				MainEventTypesOC.Add(eventDetails);
			}

			SetSelectedEventType(_selectedEventType);	// if create mode it is event by default, if edit it is the type of the event
		}
		private void ConvertEventDetailsAndSelectType(EventDetails selectedEventTypeDetails)
		{
			if (!Enum.TryParse(selectedEventTypeDetails.Text, out MainEventTypes parsedTypeOfEvent))
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {selectedEventTypeDetails.Text}");
			}

			SetSelectedEventType(parsedTypeOfEvent);
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

	}
	#region Helperclass
	public class EventDetails : BaseViewModel
	{
		private string _text;
		private float _opacity;
		private int _border;

		public string Text
		{
			get => _text;
			set { _text = value; OnPropertyChanged(); }
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