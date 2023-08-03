using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Services
{
    public static class Factory
	{

		// Event Repository
		#region EventRepository
		private static IEventRepository CreateEventRepository() => new LocalMachineEventRepository();

		private static IEventRepository _eventRepository;
		public static IEventRepository EventRepository
		{
			get
			{
				if (_eventRepository == null)
				{
					_eventRepository = CreateEventRepository();
				}
				return _eventRepository;
			}
		}

		#endregion
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, decimal spendingAmount=0, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			if (eventTypeModel.MainType == MainEventTypes.Event)
			{
				return new EventModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else if (eventTypeModel.MainType == MainEventTypes.Task)
			{
				return new TaskModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else	// spending event
			{
				return new SpendingModel(title, description, startTime, endTime, eventTypeModel, spendingAmount, isCompleted, postponeTime, wasShown);
			}
		}

		public static IUserEventTypeModel CreateNewEventType(MainEventTypes mainEventType, string eventTypeName, Color eventTypeColor)
		{
			return new UserEventTypeModel(mainEventType, eventTypeName, eventTypeColor);
		} 

	}
}
