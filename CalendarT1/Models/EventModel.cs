namespace CalendarT1.Models
{
	public class EventModel
	{
		public Guid Id { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EventPriority PriorityLevel { get; set; }
		public bool IsCompleted { get; set; }
		public bool WasShown { get; set; }
		public List<DateTime> PostponeHistory { get; set; }
		public EventModel(string title, string description, EventPriority eventPriority, DateTime startTime, DateTime endTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			Id = Guid.NewGuid();
			Title = title;
			Description = description;
			PriorityLevel = eventPriority;
			StartDateTime = startTime;
			EndDateTime = endTime;
			IsCompleted = isCompleted;
			WasShown = wasShown;
			PostponeHistory = new List<DateTime>(); // default new list 

		}
	}
}
