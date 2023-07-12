using CalendarT1.Models;
using CalendarT1.Models.Enums;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Services
{
	public static class Factory
	{
		// event priorities
		#region EventPriorities
		public static EventPriority CreatePriority(EnumPriorityLevels level)
		{
			return new EventPriority(level);
		}
		public static IEnumerable<EventPriority> CreateAllPrioritiesLevelsEnumerable()
		{
			return Enum.GetValues(typeof(EnumPriorityLevels))
				.Cast<EnumPriorityLevels>()
				.Select(level => CreatePriority(level));
		}
		#endregion

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
	}
}
