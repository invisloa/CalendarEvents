using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.ViewModels;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
    public class MainEventTypesCCHelper : IMainEventTypesCC
    {
        // Fields
        private const int FullOpacity = 1;
        private const float FadedOpacity = 0.3f;
        private const int NoBorderSize = 0;
        private const int BorderSize = 10;
        List<IMainEventType> _mainEventTypesList;
        public Color MainEventTypeButtonsColor { get; set; } = Color.FromRgb(0, 0, 0); // Defeault color is black
        private readonly Dictionary<IMainEventType, MainEventVisualDetails> _eventVisualDetails = new Dictionary<IMainEventType, MainEventVisualDetails>();
        private IMainEventType _selectedMainEventType = null;
        private Color _selectedColor = Color.FromRgb(255, 0, 0); // initialize with red

		public event Action<IMainEventType> MainEventTypeChanged;

		public IMainEventType SelectedMainEventType
        {
            get => _selectedMainEventType;
            set
            {
                if (_selectedMainEventType == value)
                {
					return;
				}
                _selectedMainEventType = value;
                // set visuals for selected event type
                SetSelectedMainEventType(_selectedMainEventType);       // TO CHECK IF COMMENTING THIS LINE IS OK
				MainEventTypeChanged?.Invoke(_selectedMainEventType); // Fire the event

			}
		}
        // Properties
        public ObservableCollection<MainEventVisualDetails> MainEventTypesVisualsOC { get; set; }

        public RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; private set; }

        // Constructor
        public MainEventTypesCCHelper(List<IMainEventType> mainEventTypesList)
        {
			_mainEventTypesList = mainEventTypesList;
			MainEventTypeSelectedCommand = new RelayCommand<MainEventVisualDetails>(ConvertEventDetailsAndSelectType);
            InitializeMainEventTypes();
        }

        // Methods
        private void ConvertEventDetailsAndSelectType(MainEventVisualDetails selectedEventTypeDetails)
        {
			var selectedEventType = _mainEventTypesList.FirstOrDefault(sc => sc.Title == selectedEventTypeDetails.MainEventTitle);

			if (selectedEventType == null)
            { 
                throw new ArgumentException($"Invalid TypeOfEvent value: {selectedEventTypeDetails.MainEventTitle}");
            }

            SetSelectedMainEventType(selectedEventType);
        }

        private void SetSelectedMainEventType(IMainEventType mainEventType)
        {
            _selectedMainEventType = mainEventType;
            DisableVisualsForAllMainEventTypes();
            //for selected event type set different visuals
            _eventVisualDetails[mainEventType].Opacity = FullOpacity;
            _eventVisualDetails[mainEventType].Border = NoBorderSize;

            //OC updates automatically
           //MainEventTypesOC = new ObservableCollection<MainEventVisualDetails>(_eventVisualDetails.Values);
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
            MainEventTypesVisualsOC = new ObservableCollection<MainEventVisualDetails>();

            // dynamically create Main Event Types according to enum
            foreach (MainEventType eventType in _mainEventTypesList)
            {
                var eventDetails = new MainEventVisualDetails
                {
                    MainEventTitle = eventType.ToString(),
                    Opacity = FadedOpacity,
                    Border = BorderSize
                };

                _eventVisualDetails[eventType] = eventDetails;
                MainEventTypesVisualsOC.Add(eventDetails);
            }
            if (_selectedMainEventType != null)
            {
                SetSelectedMainEventType(_selectedMainEventType);   // TODO TOCHECK consider (if create mode it is event by default TEMPORARY IT IS NOT) , if edit it is the type of the event
            }
        }
    }

    public class MainEventVisualDetails : BaseViewModel
    {
        // Fields
        private string _mainEventTitle;
        private float _opacity;
        private int _border;

        // Properties
        public string MainEventTitle
        {
            get => _mainEventTitle;
            set { _mainEventTitle = value; OnPropertyChanged(); }
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
