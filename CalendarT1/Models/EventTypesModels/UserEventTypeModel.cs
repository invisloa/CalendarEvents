using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarT1.Models.EventTypesModels
{
    public class UserEventTypeModel :  IUserEventTypeModel
    {
		public MainEventType MainType { get; set; }
        public string EventTypeName { get; set; }
		// Store color as string due to serialization issues
		[JsonIgnore] // This won't be included in the serialized data
		private Color _eventTypeColor;
        // This will be included in the serialized data instead
        public string EventTypeColorString
        {
            get => _eventTypeColor.ToHex();
            set => _eventTypeColor = Color.FromHex(value);
        }
		[JsonIgnore]
		public Color EventTypeColor
        {
            get
            {
                return _eventTypeColor;
            }
            set
            {
                _eventTypeColor = value;
            }
        }

        private bool _isSelectedToFilter;
        public bool IsSelectedToFilter { get; set; }
		public UserEventTypeModel(MainEventType mainEventType, string eventTypeName, Color eventTypeColor, bool isSelectedToFilter = true)
        {
			MainType = mainEventType;
			IsSelectedToFilter = isSelectedToFilter;
            EventTypeName = eventTypeName;
            EventTypeColor = eventTypeColor;
        }
        public override string ToString()
        {
            return EventTypeName;
        }
    }
}
