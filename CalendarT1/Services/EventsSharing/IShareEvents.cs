using CalendarT1.Models.EventModels;

namespace CalendarT1.Services.EventsSharing
{
    public interface IShareEvents
	{
		public Task ShareEventAsync(AbstractEventModel eventModel);

		public Task ImportEventAsync(string jsonString);
	}
}
