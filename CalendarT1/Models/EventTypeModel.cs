using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarT1.Models
{
	public class EventTypeModel : INotifyPropertyChanged
	{
		#region InotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		private string _eventTypeName;
		public string EventTypeName
		{ 
			get => _eventTypeName;
			set
			{
				_eventTypeName = value;
				OnPropertyChanged();
			}
		}
		// Store color as string
		[NonSerialized] // This won't be included in the serialized data
		private Color _eventTypeColor;

		// This will be included in the serialized data instead
		public string EventTypeColorString
		{
			get => _eventTypeColor.ToHex();
			set => _eventTypeColor = Color.FromHex(value);
		}
		public Color EventTypeColor
		{
			get
			{
				return _eventTypeColor;
			}
			set
			{
				_eventTypeColor = value;
				OnPropertyChanged();
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

		public EventTypeModel(string eventTypeName, Color eventTypeColor, bool isTask, bool isSelectedToFilter = true)
		{
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
