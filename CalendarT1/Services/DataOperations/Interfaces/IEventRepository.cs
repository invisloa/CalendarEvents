using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Services.DataOperations.Interfaces
{
    public interface IEventRepository
	{
		Task<List<IGeneralEventModel>> GetEventsListAsync();
		Task SaveEventsListAsync();
		Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete);
		Task AddEventAsync(IGeneralEventModel eventToAdd);
		Task ClearEventsListAsync();
		Task UpdateEventsAsync(IGeneralEventModel eventToUpdate);
		Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId);
		Task<List<IUserEventTypeModel>> GetUserEventTypesListAsync();
		Task SaveUserEventTypesListAsync();
		Task DeleteFromUserEventTypesListAsync(IUserEventTypeModel eventTypeToDelete);
		Task AddUserEventTypeAsync(IUserEventTypeModel eventTypeToAdd);
		Task UpdateEventTypeAsync(IUserEventTypeModel eventTypeToUpdate);
		Task InitializeAsync();

	}
}




