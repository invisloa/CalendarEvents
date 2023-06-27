using CalendarT1.Models;
using CalendarT1.Models.Enums;
using CalendarT1.Services.DataOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static IEnumerable<EventPriority> CreateAllPrioritiesLevels()
		{
			return Enum.GetValues(typeof(EnumPriorityLevels))
				.Cast<EnumPriorityLevels>()
				.Select(level => CreatePriority(level));
		}
		#endregion

		// Event Repository
		#region EventRepository

		public static IEventRepository CreateEventRepository() => new LocalMachineEventRepository();

		#endregion
	}
}
