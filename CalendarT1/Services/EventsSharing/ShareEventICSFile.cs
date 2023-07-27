using CalendarT1.Models.EventModels;

namespace CalendarT1.Services.EventsSharing
{
    internal class ShareEventICSFile : IShareEvents
	{
		public Task ImportEventAsync(string jsonString)
		{
			throw new NotImplementedException();
		}

		public Task ShareEventAsync(IGeneralEventModel eventModel)
		{
			throw new NotImplementedException();
		}
	}
}