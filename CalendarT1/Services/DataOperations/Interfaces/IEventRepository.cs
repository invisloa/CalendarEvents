using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Services.DataOperations.Interfaces
{
    public interface IEventRepository
	{
		public event Action OnEventListChanged;
		public event Action OnUserTypeListChanged;

		public List<IGeneralEventModel> AllEventsList { get;}
		public List<IUserEventTypeModel> AllUserEventTypesList { get;}
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
		Task ClearAllUserTypesAsync();
		Task AddUserEventTypeAsync(IUserEventTypeModel eventTypeToAdd);
		Task UpdateEventTypeAsync(IUserEventTypeModel eventTypeToUpdate);
		Task GetUserEventTypeAsync (IUserEventTypeModel eventTypeToSelect);
		Task InitializeAsync();
		List<IGeneralEventModel> DeepCopyAllEventsList();
		List<IUserEventTypeModel> DeepCopyUserEventTypesList();

		Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSave = null);

		Task LoadEventsAndTypesFromFile();

	}
}




