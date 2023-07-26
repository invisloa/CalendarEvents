using CalendarT1.Models;

namespace CalendarT1.Services.EventsSharing
{
	public interface IShareEvents
	{
		public Task ShareEventAsync(EventModel eventModel);

		public Task ImportEventAsync(string jsonString);
	}
}
