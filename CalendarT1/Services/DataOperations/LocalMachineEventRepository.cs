using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

public class LocalMachineEventRepository : IEventRepository
{
	public LocalMachineEventRepository()
	{
		GetEventsListAsync().ConfigureAwait(false).GetAwaiter().GetResult();		// this is a workaround for the async method to be called in the constructor
		GetUserEventTypesListAsync().ConfigureAwait(false).GetAwaiter().GetResult();// this is a workaround for the async method to be called in the constructor
	}
	#region Events Repository
	private List<IGeneralEventModel> _allEventsList;
	private static readonly string EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
	public List<IGeneralEventModel> AllEventsList			// TODO ASYNC CHANGE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	{
		get
		{
			return _allEventsList;
		}
		set
		{
			if (_allEventsList == value) { return; }
			_allEventsList = value;
			SaveEventsListAsync();
		}
	}
	public async Task AddEventAsync(IGeneralEventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		await SaveEventsListAsync();
	}

	public async Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		await SaveEventsListAsync();
	}

	public async Task ClearEventsListAsync()
	{
		AllEventsList.Clear();
		await SaveEventsListAsync();
	}

	public async Task<List<IGeneralEventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			_allEventsList = JsonConvert.DeserializeObject<List<IGeneralEventModel>>(jsonString, settings);
		}
		else
		{
			_allEventsList = new List<IGeneralEventModel>();
		}
		return _allEventsList;
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

	public async Task UpdateEventAsync(IGeneralEventModel eventToUpdate)
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

	#region UserTypes Repository
	private List<IUserEventTypeModel> _allEventTypesList;
	private static readonly string UserTypesFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonUserTypesFileName", "CalendarTypesOfEventsD"));
	public List<IUserEventTypeModel> AllEventTypesList
	{
		get
		{
			if (_allEventTypesList == null)
				GetUserEventTypesListAsync().Wait();
			return _allEventTypesList;
		}
		set
		{
			if (_allEventTypesList == value) { return; }
			_allEventTypesList = value;
			SaveUserEventTypesListAsync().Wait();
		}
	}

	public async Task<List<IUserEventTypeModel>> GetUserEventTypesListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(UserTypesFilePath);
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			_allEventTypesList = JsonConvert.DeserializeObject<List<IUserEventTypeModel>>(jsonString, settings);
		}
		else
		{
			_allEventTypesList = new List<IUserEventTypeModel>();
		}
		return _allEventTypesList;
	}

	public async Task SaveUserEventTypesListAsync()
	{
		var directoryPath = Path.GetDirectoryName(UserTypesFilePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
		var jsonString = JsonConvert.SerializeObject(AllEventTypesList, settings);
		await File.WriteAllTextAsync(UserTypesFilePath, jsonString);
	}

	public async Task DeleteFromUserEventTypesListAsync(IUserEventTypeModel eventTypeToDelete)
	{
		AllEventTypesList.Remove(eventTypeToDelete);
		await SaveUserEventTypesListAsync();
	}

	public async Task AddUserEventTypeAsync(IUserEventTypeModel eventTypeToAdd)
	{
		AllEventTypesList.Add(eventTypeToAdd);
		await SaveUserEventTypesListAsync();
	}
	public async Task UpdateEventTypeAsync(IUserEventTypeModel eventTypeToUpdate)
	{
		await SaveUserEventTypesListAsync();
	}
	#endregion
}
