namespace CalendarT1.Models
{
	public class EventModel
	{
		public Guid Id { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EventTypeModel EventType { get; set; }
		public bool IsCompleted { get; set; }
		public bool WasShown { get; set; }
		public Color EventVisibleColor
		{
			get
			{
				Color color = EventType.EventTypeColor;

				// Apply the completed color adjustment if necessary
				if (IsCompleted)
				{
					color = IsCompleteColorAdapt(color);
				}
				return color;
			}
		}
		public List<DateTime> PostponeHistory { get; set; }
		public EventModel(string title, string description, EventTypeModel eventPriority, DateTime startTime, DateTime endTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			Id = Guid.NewGuid();
			Title = title;
			Description = description;
			EventType = eventPriority;
			StartDateTime = startTime;
			EndDateTime = endTime;
			IsCompleted = isCompleted;
			WasShown = wasShown;
			PostponeHistory = new List<DateTime>(); // default new list 

		}
		private Color IsCompleteColorAdapt(Color color)
		{
			// Here you can make the color gray or decrease the alpha. Here's an example of decreasing the alpha:
			return Color.FromRgba(color.Red, color.Green, color.Blue, color.Alpha / 20);
		}

	}
}
