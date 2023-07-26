using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace CalendarT1.Models
{
	public abstract class AbstractEventModel
	{
		public Guid Id { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public bool WasShown { get; set; }
		public virtual bool IsCompleted { get; set; }

		public List<DateTime> PostponeHistory { get; set; }
		public TimeSpan ReminderTime { get; set; }
		[JsonIgnore]
		public Color EventVisibleColor
		{
			set
			{
				_eventTypeColor = value;
			}
			get
			{
				Color color = _eventTypeColor;
				if (IsCompleted)
				{
					color = IsCompleteColorAdapt(color);
				}
				return color;
			}
		}
		public AbstractEventModel(string title, string description, DateTime startTime, DateTime endTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			Id = Guid.NewGuid();
			Title = title;
			Description = description;
			StartDateTime = startTime;
			EndDateTime = endTime;
			IsCompleted = isCompleted;
			WasShown = wasShown;
			PostponeHistory = new List<DateTime>(); // default new list 
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
				}
			}
		}

		private Color _eventTypeColor;

		// This will be included in the serialized data instead
		public string EventTypeColorString
		{
			get => _eventTypeColor.ToHex();
			set => _eventTypeColor = Color.FromHex(value);
		}

		private Color IsCompleteColorAdapt(Color color)
		{
			// Here you can make the color gray or decrease the alpha. Here's an example of decreasing the alpha:
			return Color.FromRgba(color.Red, color.Green, color.Blue, color.Alpha / 20);
		}

	}
}
