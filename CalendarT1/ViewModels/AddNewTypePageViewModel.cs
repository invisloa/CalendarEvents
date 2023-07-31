﻿using System;
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
		private const int BorderSize = 10;
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
		public RelayCommand<EventDetails> EventTypeSelectedCommand { get; private set; }
		public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }

		public AddNewTypePageViewModel()
		{
			EventTypeSelectedCommand = new RelayCommand<EventDetails>(SetEventTypeSelected);
			SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
			SelectedColor = Color.FromRgb(255, 0, 0); // Red
			InitializeEventTypes();

			ButtonsColors = new ObservableCollection<ButtonProperties>
			{

				new ButtonProperties { ButtonColor = Color.FromRgb(144, 238, 144), ButtonBorder = BorderSize}, // LightGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(60, 179, 113), ButtonBorder = BorderSize}, // MediumSeaGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(34, 139, 34), ButtonBorder = BorderSize}, // ForestGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 100, 0), ButtonBorder = BorderSize}, // DarkGreen
				new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), ButtonBorder = BorderSize}, // Lime
    
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
				new ButtonProperties { ButtonColor = Color.FromRgb(128, 0, 0), ButtonBorder = BorderSize}, // Maroon
				new ButtonProperties { ButtonColor = Color.FromRgb(139, 0, 0), ButtonBorder = BorderSize}, // DarkRed
    
				// Violet shades
				new ButtonProperties { ButtonColor = Color.FromRgb(238, 130, 238), ButtonBorder = BorderSize}, // Violet
				new ButtonProperties { ButtonColor = Color.FromRgb(221, 160, 221), ButtonBorder = BorderSize}, // Plum
				new ButtonProperties { ButtonColor = Color.FromRgb(148, 0, 211), ButtonBorder = BorderSize}, // DarkViolet
				new ButtonProperties { ButtonColor = Color.FromRgb(138, 43, 226), ButtonBorder = BorderSize}, // BlueViolet
				new ButtonProperties { ButtonColor = Color.FromRgb(153, 50, 204), ButtonBorder = BorderSize}, // DarkOrchid
    
				// Orange shades
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 160, 122), ButtonBorder = BorderSize},  // LightSalmon
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 165, 0), ButtonBorder = BorderSize}, // Orange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 140, 0), ButtonBorder = BorderSize}, // DarkOrange
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 69, 0), ButtonBorder = BorderSize}, // OrangeRed
				new ButtonProperties { ButtonColor = Color.FromRgb(255, 30, 0), ButtonBorder = BorderSize}, // OrangeRed

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
	public class ButtonProperties : BaseViewModel
	{
		private int _borderSize;
		public Color ButtonColor { get; set; }
		public int ButtonBorder { get => _borderSize;
			set
			{
				_borderSize = value;
				OnPropertyChanged();
			}
		}
	}
}
