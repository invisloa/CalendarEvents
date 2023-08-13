using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Services
{
    public static class Factory
	{

		// Event Repository

		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, Quantity quantityAmount = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
		{
			if (eventTypeModel.MainType == MainEventTypes.Event)
			{
				return new EventModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else if (eventTypeModel.MainType == MainEventTypes.Task)
			{
				return new TaskModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else	// Value type event
			{
				return new ValueModel(title, description, startTime, endTime, eventTypeModel, quantityAmount, isCompleted, postponeTime, wasShown);
			}
		}

		public static IUserEventTypeModel CreateNewEventType(MainEventTypes mainEventType, string eventTypeName, Color eventTypeColor)
		{
			return new UserEventTypeModel(mainEventType, eventTypeName, eventTypeColor);
		} 

	}
}
