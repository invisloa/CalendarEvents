using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

public class LocalMachineEventRepository : IEventRepository
{
	public event Action OnEventListChanged;
	public event Action OnUserTypeListChanged;

	private static readonly string EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
	public LocalMachineEventRepository() { }

	#region Events Repository
	private List<IGeneralEventModel> _allEventsList;
	public List<IGeneralEventModel> AllEventsList			
	{
		get
		{
			return _allEventsList;
		}
		private set
		{
			if (_allEventsList == value) { return; }
			_allEventsList = value;
			OnEventListChanged?.Invoke();
		}
	}
	public async Task AddEventAsync(IGeneralEventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();

	}

	public async Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();

	}

	public async Task ClearEventsListAsync()
	{
		AllEventsList.Clear();
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();

	}

	public async Task<List<IGeneralEventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			AllEventsList = JsonConvert.DeserializeObject<List<IGeneralEventModel>>(jsonString, settings);
		}
		else
		{
			AllEventsList = new List<IGeneralEventModel>();
		}
		return AllEventsList;
	}

	public async Task SaveEventsListAsync()
	{
		var directoryPath = Path.GetDirectoryName(EventsFilePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented};
		var jsonString = JsonConvert.SerializeObject(AllEventsList, settings);
		await File.WriteAllTextAsync(EventsFilePath, jsonString);
	}

	public async Task UpdateEventsAsync(IGeneralEventModel eventToUpdate)	// cos nie tak	???
	{
		var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		if (eventToUpdateInList != null)
		{
			// TO CHECK
			//AllEventsList.Remove(eventToUpdateInList);
			//AllEventsList.Add(eventToUpdate);
			await SaveEventsListAsync();
		}
		else
		{
			await Task.CompletedTask;
		}
	}

	public Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId)
	{
		var selectedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventId);
		return Task.FromResult(selectedEvent);
	}
	#endregion


	// UserTypes Repository
	#region UserTypes Repository
	private List<IUserEventTypeModel> _allUserEventTypesList;
	private static readonly string UserTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonUserTypesFileName", "CalendarTypesOfEventsD"));
	public List<IUserEventTypeModel> AllUserEventTypesList
	{
		get
		{
			return _allUserEventTypesList;
		}
		private set
		{
			if (_allUserEventTypesList == value) { return; }
			_allUserEventTypesList = value;
			OnUserTypeListChanged?.Invoke();
		}
	}
	public async Task InitializeAsync()
	{
		_allEventsList = await GetEventsListAsync().ConfigureAwait(false);                          // TO CHECK - cosideer ConfigureAwait left to default??????
		_allUserEventTypesList = await GetUserEventTypesListAsync().ConfigureAwait(false);          // TO CHECK - cosideer ConfigureAwait left to default??????
	}
	public async Task<List<IUserEventTypeModel>> GetUserEventTypesListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(UserTypesFilePath);
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			AllUserEventTypesList = JsonConvert.DeserializeObject<List<IUserEventTypeModel>>(jsonString, settings);
		}
		else
		{
			AllUserEventTypesList = new List<IUserEventTypeModel>();
		}
		return AllUserEventTypesList;
	}

	public async Task SaveUserEventTypesListAsync()
	{
		var directoryPath = Path.GetDirectoryName(UserTypesFilePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
		var jsonString = JsonConvert.SerializeObject(AllUserEventTypesList, settings);
		await File.WriteAllTextAsync(UserTypesFilePath, jsonString);
		OnUserTypeListChanged?.Invoke();
	}

	public async Task DeleteFromUserEventTypesListAsync(IUserEventTypeModel eventTypeToDelete)
	{
		AllUserEventTypesList.Remove(eventTypeToDelete);
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
	}

	public async Task AddUserEventTypeAsync(IUserEventTypeModel eventTypeToAdd)
	{
		AllUserEventTypesList.Add(eventTypeToAdd);
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
	}
	public async Task UpdateEventTypeAsync(IUserEventTypeModel eventTypeToUpdate)
	{
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
	}

	public Task GetUserEventTypeAsync(IUserEventTypeModel eventTypeToSelect)
	{
		var selectedEventType = AllUserEventTypesList.FirstOrDefault(e => e == eventTypeToSelect);		// TO CHANGE
		return Task.FromResult(selectedEventType);
	}
	#endregion
}
