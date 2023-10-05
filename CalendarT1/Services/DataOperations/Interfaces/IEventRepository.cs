using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Services.DataOperations.Interfaces
{
    public interface IEventRepository
	{
		public event Action OnEventListChanged;
		public event Action OnUserEventTypeListChanged;

		Task AddEventAsync(IGeneralEventModel eventToAdd);
		Task AddSubEventTypeAsync(IUserEventTypeModel eventTypeToAdd);
		Task AddMainEventTypeAsync(IMainEventType eventTypeToAdd);
		Task SaveEventsListAsync();
		Task SaveSubEventTypesListAsync();
		Task SaveMainEventTypesListAsync();
		Task UpdateEventAsync(IGeneralEventModel eventToUpdate);
		Task UpdateSubEventTypeAsync(IUserEventTypeModel eventTypeToUpdate);
		Task UpdateMainEventTypeAsync(IMainEventType eventTypeToUpdate);
		Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete);
		Task DeleteFromSubEventTypesListAsync(IUserEventTypeModel eventTypeToDelete);
		Task DeleteFromMainEventTypesListAsync(IMainEventType eventTypeToDelete);

		Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId);

		Task<IUserEventTypeModel> GetSubEventTypeAsync(IUserEventTypeModel eventTypeToSelect);
		Task<IMainEventType> GetMainEventTypeAsync(IMainEventType eventTypeToSelect);

		Task ClearAllEventsListAsync();
		Task ClearAllSubEventTypesAsync();
		Task ClearAllMainEventTypesAsync();

		List<IGeneralEventModel> AllEventsList { get; }
		List<IUserEventTypeModel> AllUserEventTypesList { get; }
		List<IMainEventType> AllMainEventTypesList { get; }
		List<IGeneralEventModel> DeepCopyAllEventsList();
		List<IUserEventTypeModel> DeepCopySubEventTypesList();
		List<IMainEventType> DeepCopyMainEventTypesList();
		Task<List<IGeneralEventModel>> GetEventsListAsync();
		Task<List<IUserEventTypeModel>> GetSubEventTypesListAsync();
		Task<List<IMainEventType>> GetMainEventTypesListAsync();
		Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSave = null);
		Task LoadEventsAndTypesFromFile();
		Task InitializeAsync();

	}
}




