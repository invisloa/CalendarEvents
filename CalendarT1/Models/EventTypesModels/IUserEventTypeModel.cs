using CalendarT1.Models.EventModels;

namespace CalendarT1.Models.EventTypesModels
{
	public interface IUserEventTypeModel : IEquatable<object>
	{
		MainEventTypes MainEventType { get; set; }
		Color EventTypeColor { get; set; }	// original event color
		Color BackgroundColor { get; set; }	// color that is currently shown (isCompleted color adjustment)
		string EventTypeColorString { get; set; }	// needed for json serialization
		string EventTypeName { get; set; }	
		bool IsSelectedToFilter { get; set; }
		Quantity QuantityAmount { get; set; }	
		public TimeSpan DefaultEventTimeSpan { get; set; }	// default event time for the event type
		string ToString();
		bool Equals(object other);	// to check if the event type is already in the list
		int GetHashCode();	// to check if the event type is already in the list
	}
}