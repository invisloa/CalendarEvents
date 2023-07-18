using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventsSharing
{
	public class ShareEventsJson : IShareEvents
	{

		public ShareEventsJson(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}
		private readonly IEventRepository _eventRepository;
		public async Task AddEventAsync(EventModel eventModel)
		{
			await _eventRepository.AddEventAsync(eventModel);
		}
		public string SerializeEventToJson(EventModel eventModel)
		{
			var jsonString = JsonConvert.SerializeObject(eventModel);
			return jsonString;
		}

		public async Task ShareEventAsync(EventModel eventModel)
		{
			var jsonString = SerializeEventToJson(eventModel);

			await Share.RequestAsync(new ShareTextRequest
			{
				Text = jsonString,
				Title = $"Share {eventModel.Title}"
			});
		}

		public async Task ImportEventAsync(string jsonString)
		{
			var eventModel = JsonConvert.DeserializeObject<EventModel>(jsonString);
			await AddEventAsync(eventModel);
		}

	}
}