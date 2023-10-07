using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
    public interface IGeneralEventModel
    {
        string Description { get; set; }
        IUserEventTypeModel EventType { get; set; }
        Color EventVisibleColor { get; }
        Guid Id { get; set; }
        bool IsCompleted { get; set; }
        List<DateTime> PostponeHistory { get; set; }
        TimeSpan ReminderTime { get; set; }
        DateTime StartDateTime { get; set; }
		DateTime EndDateTime { get; set; }
        string Title { get; set; }
        bool WasShown { get; set; }
        public QuantityModel QuantityAmount { get; set; }
        public List<MicroTaskModel> MicroTasksList { get; set; }
    }
}