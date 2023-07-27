namespace CalendarT1.Models
{
	public interface IEventTypeModel
	{
		Color EventTypeColor { get; set; }
		string EventTypeColorString { get; set; }
		string EventTypeName { get; set; }
		bool IsSelectedToFilter { get; set; }

		string ToString();
	}
}