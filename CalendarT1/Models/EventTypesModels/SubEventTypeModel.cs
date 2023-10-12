using CalendarT1.Models.EventModels;
using CalendarT1.ViewModels;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CalendarT1.Models.EventTypesModels
{
	public class SubEventTypeModel : BaseViewModel, ISubEventTypeModel
	{
		public IMainEventType MainEventType { get; set; }
		public string EventTypeName { get; set; }
		private Color _eventTypeColor;
		private TimeSpan _defaultEventTime;
		private bool _isSelectedToFilter;

		private bool _isValueType;
		private bool _isMultiTaskType;
		private List<MicroTaskModel> _multiTasksList;
		private QuantityModel _quantityAmount;
		public List<MicroTaskModel> MicroTasksList
		{
			get => _multiTasksList;
			set
			{
				if (_multiTasksList != value)
				{
					_multiTasksList = value;
					OnPropertyChanged();
				}
			}
		}
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
		public TimeSpan DefaultEventTimeSpan
		{
			get
			{
				return _defaultEventTime;
			}
			set
			{
				if (_defaultEventTime != value)
				{
					_defaultEventTime = value;
				}
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

		// BackgroundColor is added to the model to store the color that is currently shown (isCompleted color adjustment) - consider changing it to a converter (low priority)
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
		public bool IsValueType
		{
			get => _isValueType;
			set
			{
				if (_isValueType != value)
				{
					_isValueType = value;
					OnPropertyChanged();
				}
			}
		}
		public bool IsMicroTaskType
		{
			get => _isMultiTaskType;
			set
			{
				if (_isMultiTaskType != value)
				{
					_isMultiTaskType = value;
					OnPropertyChanged();
				}
			}
		}
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
		public QuantityModel DefaultQuantityAmount
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

		public SubEventTypeModel(IMainEventType mainEventType, string eventTypeName, Color eventTypeColor, TimeSpan defaultEventTime, QuantityModel quantity = null, List<MicroTaskModel> multiTasksList = null, bool isSelectedToFilter = true)
		{
			MainEventType = mainEventType;
			IsSelectedToFilter = isSelectedToFilter;
			DefaultEventTimeSpan = defaultEventTime;
			EventTypeName = eventTypeName;
			EventTypeColor = eventTypeColor;
			BackgroundColor = eventTypeColor; // Initialize BackgroundColor as EventTypeColor upon object creation
			DefaultQuantityAmount = quantity;
			MicroTasksList = multiTasksList;
		}

		public bool Equals(ISubEventTypeModel obj)
		{
			if (obj is ISubEventTypeModel other)
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
