using CalendarT1.Models.EventModels;

namespace CalendarT1.Models.EventTypesModels
{
    public interface IUserEventTypeModel
    {
		MainEventTypes MainEventType { get; set; }
		Color EventTypeColor { get; set; }
		Color BackgroundColor { get; set; }
		string EventTypeColorString { get; set; }
        string EventTypeName { get; set; }
        bool IsSelectedToFilter { get; set; }
        Quantity QuantityAmount { get; set; }
        string ToString();
    }
}