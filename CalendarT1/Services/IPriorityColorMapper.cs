using CalendarT1.Models.Enums;

namespace CalendarT1.Services
{
	public interface IPriorityColorMapper
	{
		Color GetColor(EnumPriorityLevels priority);
	}
}