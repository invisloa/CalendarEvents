using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace CalendarT1.ViewModels
{
	public class AddNewTypePageViewModel : BaseViewModel
	{
		private enum TypeOfEvent
		{
			Event,
			Task,
			Spending
		}

		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 15;
		private Color _selectedColor;
		private string _typeName;
		private readonly Dictionary<TypeOfEvent, EventDetails> _eventDetails = new Dictionary<TypeOfEvent, EventDetails>();

		private TypeOfEvent _selectedEventType = TypeOfEvent.Event;

		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }
		public ObservableCollection<EventDetails> EventTypesOC { get; set; }

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
				OnPropertyChanged();
			}

		}
		public RelayCommand<string> ColorSelectionCommand { get; private set; }
		public RelayCommand<EventDetails> EventTypeSelectedCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }

		public AddNewTypePageViewModel()
		{
			ColorSelectionCommand = new RelayCommand<string>(OnColorSelection);
			EventTypeSelectedCommand = new RelayCommand<EventDetails>(SetEventTypeSelected);
			SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
			SelectedColor = Color.FromRgb(255, 0, 0); // Red
			InitializeEventTypes();

			ButtonsColors = new ObservableCollection<ButtonProperties>
				{
					new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), IsSelected = false , ButtonBorder = BorderSize}, // Red
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), IsSelected = false , ButtonBorder = BorderSize}, // Green
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 255), IsSelected = false , ButtonBorder = BorderSize}, // Blue
				};
		}

		private void InitializeEventTypes()
		{
			EventTypesOC = new ObservableCollection<EventDetails>();

			foreach (TypeOfEvent eventType in Enum.GetValues(typeof(TypeOfEvent)))
			{
				var eventDetails = new EventDetails
				{
					Text = eventType.ToString(),
					Opacity = FadedOpacity,
					Border = BorderSize
				};

				_eventDetails[eventType] = eventDetails;
				EventTypesOC.Add(eventDetails);
			}

			SetSelectedEventType(_selectedEventType);
		}
		private void SetEventTypeSelected(EventDetails selectedEventTypeDetails)
		{
			if (!Enum.TryParse(selectedEventTypeDetails.Text, out TypeOfEvent parsedTypeOfEvent))
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {selectedEventTypeDetails.Text}");
			}

			SetSelectedEventType(parsedTypeOfEvent);
		}
		private void SetSelectedEventType(TypeOfEvent eventType)
		{
			_selectedEventType = eventType;

			foreach (var eventDetail in _eventDetails.Values)
			{
				eventDetail.Opacity = FadedOpacity;
				eventDetail.Border = BorderSize;
			}

			_eventDetails[eventType].Opacity = FullOpacity;
			_eventDetails[eventType].Border = NoBorderSize;

			// Force update of the ObservableCollection
			EventTypesOC = new ObservableCollection<EventDetails>(_eventDetails.Values);
		}
		private void OnColorSelection(string buttonIdentifier)
		{
			foreach (var button in ButtonsColors)
			{
				button.ButtonBorder = button.ButtonColor.ToString() == buttonIdentifier ? NoBorderSize : BorderSize;
			}
		}

		private void SelectColor(ButtonProperties selectedColor)
		{
			SelectedColor = selectedColor.ButtonColor;

			foreach (var button in ButtonsColors)
			{
				button.ButtonBorder = button == selectedColor ? NoBorderSize : BorderSize;
			}
		}
	}

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
	public class ButtonProperties
	{
		public Color ButtonColor { get; set; }
		public int ButtonBorder { get; set; }
		public bool IsSelected { get; set; }
	}
}
