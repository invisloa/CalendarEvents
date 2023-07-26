using CalendarT1.Models;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public interface IEventRepository
	{
		Task<List<AbstractEventModel>> GetEventsListAsync();
		Task SaveEventsListAsync();
		Task DeleteFromEventsListAsync(AbstractEventModel eventToDelete);
		Task AddEventAsync(AbstractEventModel eventToAdd);
		Task ClearEventsListAsync();
		Task UpdateEventAsync(AbstractEventModel eventToUpdate);
		Task<AbstractEventModel> GetEventByIdAsync(Guid eventId);
	}
}