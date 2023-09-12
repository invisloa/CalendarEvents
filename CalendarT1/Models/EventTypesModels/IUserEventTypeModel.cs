using CalendarT1.Models.EventModels;

namespace CalendarT1.Models.EventTypesModels
{
	public interface IUserEventTypeModel : IEquatable<IUserEventTypeModel>
	{
		MainEventTypes MainEventType { get; set; }
		Color EventTypeColor { get; set; }
		Color BackgroundColor { get; set; }
		string EventTypeColorString { get; set; }
		string EventTypeName { get; set; }
		bool IsSelectedToFilter { get; set; }
		Quantity QuantityAmount { get; set; }
		string ToString();

		// Default method implementations
		bool IEquatable<IUserEventTypeModel>.Equals(IUserEventTypeModel other)
		{
			if (other == null)
				return false;

			return
				MainEventType == other.MainEventType &&
				EventTypeColorString == other.EventTypeColorString &&
				EventTypeName == other.EventTypeName;
		}

		int GetHashCode()
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
	}
}