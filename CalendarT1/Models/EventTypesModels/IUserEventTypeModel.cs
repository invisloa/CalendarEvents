namespace CalendarT1.Models.EventTypesModels
{
    public interface IUserEventTypeModel
    {
        Color EventTypeColor { get; set; }
        string EventTypeColorString { get; set; }
        string EventTypeName { get; set; }
        bool IsSelectedToFilter { get; set; }

        string ToString();
    }
}