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


		public static IUserEventTypeModel CreateNewEventType(MainEventTypes mainEventType, string eventTypeName, Color eventTypeColor)
		{
			return new UserEventTypeModel(mainEventType, eventTypeName, eventTypeColor);
		} 

	}
}
