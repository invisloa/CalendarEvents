using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarT1.Services
{
	public class EventReminderService 
	{
		private readonly IEventRepository _eventRepository;
		private readonly IPopupService _popupService;

		public EventReminderService(IEventRepository eventRepository, IPopupService popupService)
		{
			_eventRepository = eventRepository;
			_popupService = popupService;
		}

		public async Task CheckEvents()
		{
			var events = await _eventRepository.GetEventsListAsync();
			var currentDate = DateTime.Now;

			foreach (var eventModel in events.Where(e => !e.WasShown && !e.IsCompleted && e.StartDateTime >= currentDate))
			{
				eventModel.WasShown = true;
				await _eventRepository.UpdateEventAsync(eventModel);
				await _popupService.ShowReminderPopup(eventModel);
			}
		}

		public async Task MarkAsComplete(Guid eventId)
		{
			var eventModel = await _eventRepository.GetEventByIdAsync(eventId);

			if (eventModel == null)
			{
				throw new ArgumentException("Event with provided ID doesn't exist.");
			}

			eventModel.IsCompleted = true;
			await _eventRepository.UpdateEventAsync(eventModel);
		}
		public async Task GoToEvent(Guid eventId)
		{
			var eventModel = await _eventRepository.GetEventByIdAsync(eventId);

			if (eventModel == null)
			{
				throw new ArgumentException("Event with provided ID doesn't exist.");
			}

			 
		}
		public async Task PostponeEvent(Guid eventId, int hours = 24)
		{
			var eventModel = await _eventRepository.GetEventByIdAsync(eventId);

			if (eventModel == null)
			{
				throw new ArgumentException("Event with provided ID doesn't exist.");
			}

			eventModel.PostponeHistory.Add(eventModel.StartDateTime);
			eventModel.StartDateTime = eventModel.StartDateTime.AddHours(hours);
			eventModel.EndDateTime = eventModel.EndDateTime.AddHours(hours);
			eventModel.WasShown = false;
			await _eventRepository.UpdateEventAsync(eventModel);
		}
	}
}
