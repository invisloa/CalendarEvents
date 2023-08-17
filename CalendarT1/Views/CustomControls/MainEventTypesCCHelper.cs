﻿using CalendarT1.Models.EventTypesModels;
using CalendarT1.ViewModels;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls
{
	public class MainEventTypesCCHelper : IMainEventTypesCC
	{
		// Fields
		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;
		public Color MainEventTypeButtonsColor { get; set; } = Color.FromRgb(0, 0, 0); // Defeault color is black


		private readonly Dictionary<MainEventTypes, EventVisualDetails> _eventVisualDetails = new Dictionary<MainEventTypes, EventVisualDetails>();
		private MainEventTypes _selectedMainEventType = MainEventTypes.Event;
		private IUserEventTypeModel _currentType;
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;
		public MainEventTypes SelectedMainEventType
		{
			get => _selectedMainEventType;
			set
			{
				_selectedMainEventType = value;
				// set visuals for selected event type
				SetSelectedMainEventType(_selectedMainEventType);
			}
		}

		// Properties
		public ObservableCollection<EventVisualDetails> MainEventTypesOC { get; set; }

		public RelayCommand<EventVisualDetails> MainEventTypeSelectedCommand { get; private set; }

		// Constructor
		public MainEventTypesCCHelper()
		{
			MainEventTypeSelectedCommand = new RelayCommand<EventVisualDetails>(ConvertEventDetailsAndSelectType);
			InitializeMainEventTypes();
		}

		// Methods
		private void ConvertEventDetailsAndSelectType(EventVisualDetails selectedEventTypeDetails)
		{
			if (!Enum.TryParse(selectedEventTypeDetails.MainEventNameText, out MainEventTypes parsedTypeOfEvent))
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {selectedEventTypeDetails.MainEventNameText}");
			}

			SetSelectedMainEventType(parsedTypeOfEvent);
		}

		private void SetSelectedMainEventType(MainEventTypes mainEventType)
		{
			_selectedMainEventType = mainEventType;
			DisableVisualsForAllMainEventTypes();
			//for selected event type set different visuals
			_eventVisualDetails[mainEventType].Opacity = FullOpacity;
			_eventVisualDetails[mainEventType].Border = NoBorderSize;

			// Force update of the ObservableCollection
			MainEventTypesOC = new ObservableCollection<EventVisualDetails>(_eventVisualDetails.Values);
		}
		public void DisableVisualsForAllMainEventTypes()
		{
			foreach (var eventType in _eventVisualDetails.Values)
			{
				eventType.Opacity = FadedOpacity;
				eventType.Border = BorderSize;
			}
		}
		private void InitializeMainEventTypes()
		{
			MainEventTypesOC = new ObservableCollection<EventVisualDetails>();

			// dynamically create Main Event Types according to enum
			foreach (MainEventTypes eventType in Enum.GetValues(typeof(MainEventTypes)))
			{
				var eventDetails = new EventVisualDetails
				{
					MainEventNameText = eventType.ToString(),
					Opacity = FadedOpacity,
					Border = BorderSize
				};

				_eventVisualDetails[eventType] = eventDetails;
				MainEventTypesOC.Add(eventDetails);
			}

			SetSelectedMainEventType(_selectedMainEventType);   // if create mode it is event by default, if edit it is the type of the event
		}
	}

	public class EventVisualDetails : BaseViewModel
	{
		// Fields
		private string _mainEventNameText;
		private float _opacity;
		private int _border;

		// Properties
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
}