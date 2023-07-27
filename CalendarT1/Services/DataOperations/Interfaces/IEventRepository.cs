using CalendarT1.Models.EventModels;

namespace CalendarT1.Services.DataOperations.Interfaces
{
    public interface IEventRepository
	{
		Task<List<IGeneralEventModel>> GetEventsListAsync();
		Task SaveEventsListAsync();
		Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete);
		Task AddEventAsync(IGeneralEventModel eventToAdd);
		Task ClearEventsListAsync();
		Task UpdateEventAsync(IGeneralEventModel eventToUpdate);
		Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId);
	}
}