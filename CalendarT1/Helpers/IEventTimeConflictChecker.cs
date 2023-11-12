using CalendarT1.Models.EventModels;

namespace CalendarT1.Helpers
{
	public interface IEventTimeConflictChecker
	{
		public List<IGeneralEventModel> allEvents { get; set; }

		bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck);
	}
}