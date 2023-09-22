using CalendarT1.Models.EventModels;
using CalendarT1.ViewModels;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CalendarT1.Models.EventTypesModels
{
	public class UserEventTypeModel : BaseViewModel, IUserEventTypeModel
	{
		public MainEventTypes MainEventType { get; set; }
		public string EventTypeName { get; set; }
		private Color _eventTypeColor;

		// Store color as string due to serialization issues
		public string EventTypeColorString
		{
			get
			{
				return _eventTypeColor.ToArgbHex();
			}

			set
			{
				_eventTypeColor = Color.FromArgb(value);
			}
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
				if (_eventTypeColor != value)
				{
					_eventTypeColor = value;
					OnPropertyChanged();
				}
			}
		}

		private Color _backgroundColor;

		// BackgroundColor uses its own private backing field
		[JsonIgnore]
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				if (_backgroundColor != value)
				{
					_backgroundColor = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isSelectedToFilter;
		public bool IsSelectedToFilter
		{
			get => _isSelectedToFilter;
			set
			{
				if (_isSelectedToFilter != value)
				{
					_isSelectedToFilter = value;
					OnPropertyChanged();
				}
			}
		}
		Quantity _quantityAmount;
		public Quantity QuantityAmount
		{
			get => _quantityAmount;
			set
			{
				if (_quantityAmount != value)
				{
					_quantityAmount = value;
					OnPropertyChanged();
				}
			}
		}

		public UserEventTypeModel(MainEventTypes mainEventType, string eventTypeName, Color eventTypeColor, Quantity quantity = null, bool isSelectedToFilter = true)
		{
			MainEventType = mainEventType;
			IsSelectedToFilter = isSelectedToFilter;
			EventTypeName = eventTypeName;
			EventTypeColor = eventTypeColor;
			BackgroundColor = eventTypeColor; // Initialize BackgroundColor as EventTypeColor upon object creation
			QuantityAmount = quantity;
		}

		public override bool Equals(object obj)
		{
			if (obj is IUserEventTypeModel other)
			{
				return MainEventType == other.MainEventType &&
					   EventTypeColorString == other.EventTypeColorString &&
					   EventTypeName == other.EventTypeName;
			}
			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + MainEventType.GetHashCode();
				hash = hash * 23 + (EventTypeName?.GetHashCode() ?? 0);
				hash = hash * 23 + (EventTypeColorString?.GetHashCode() ?? 0);
				return hash;
			}
		}

		public override string ToString()
		{
			return EventTypeName;
		}
		//This method will be called after deserialization and will ensure that BackgroundColor is initialized to the value of EventTypeColor
		[OnDeserialized]
		private void OnDeserializedMethod(StreamingContext context)
		{
			BackgroundColor = EventTypeColor;
		}

	}
}
