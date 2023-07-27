using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

namespace CalendarT1.Services.EventsSharing
{
    public class ShareEventsJson : IShareEvents
	{

		public ShareEventsJson(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}
		private readonly IEventRepository _eventRepository;
		public async Task AddEventAsync(AbstractEventModel eventModel)
		{
			await _eventRepository.AddEventAsync(eventModel);
		}
		public string SerializeEventToJson(AbstractEventModel eventModel)
		{
			var jsonString = JsonConvert.SerializeObject(eventModel);
			return jsonString;
		}

		public async Task ShareEventAsync(AbstractEventModel eventModel)
		{
			// You might want to make sure your eventModel is saved before you share the link.
			var link = $"myapp://event?id={eventModel.Id}";

			await Share.RequestAsync(new ShareTextRequest
			{
				Text = link,
				Title = $"Share {eventModel.Title}"
			});
		}
		public async Task ImportEventAsync(string jsonString)
		{
			var eventModel = JsonConvert.DeserializeObject<AbstractEventModel>(jsonString);
			var eventExists = await _eventRepository.GetEventByIdAsync(eventModel.Id) != null;

			if (!eventExists)
			{
				await AddEventAsync(eventModel);
			}
			else
			{
				// TODO: Add a message or handle the case when the event already exists
			}
		}


	}
}