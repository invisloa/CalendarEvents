using CalendarT1.Models.EventTypesModels;
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

		private readonly Dictionary<MainEventTypes, EventVisualDetails> _eventVisualDetails = new Dictionary<MainEventTypes, EventVisualDetails>();
		private MainEventTypes _selectedEventType = MainEventTypes.Event;
		private IUserEventTypeModel _currentType;
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red
		private string _typeName;

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
			MainEventTypesOC = new ObservableCollection<EventVisualDetails>(_eventVisualDetails.Values);
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

			SetSelectedEventType(_selectedEventType);   // if create mode it is event by default, if edit it is the type of the event
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
