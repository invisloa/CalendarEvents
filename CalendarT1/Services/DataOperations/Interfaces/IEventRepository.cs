using CalendarT1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public interface IEventRepository
	{
		Task<List<EventModel>> LoadEventsListAsync();
		Task SaveEventsListAsync();
		Task DeleteFromEventsListAsync(EventModel eventToDelete);
		Task AddEventAsync(EventModel eventToAdd);
		Task ClearEventsListAsync();
		Task UpdateEventAsync(EventModel eventToUpdate);
	}
}