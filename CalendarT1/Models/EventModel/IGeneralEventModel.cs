namespace CalendarT1.Models
{
	public interface IGeneralEventModel
	{
		string Description { get; set; }
		DateTime EndDateTime { get; set; }
		IEventTypeModel EventType { get; set; }
		Color EventVisibleColor { get; }
		Guid Id { get; set; }
		bool IsCompleted { get; set; }
		List<DateTime> PostponeHistory { get; set; }
		TimeSpan ReminderTime { get; set; }
		DateTime StartDateTime { get; set; }
		string Title { get; set; }
		bool WasShown { get; set; }
	}
}