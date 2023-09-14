using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

public class LocalMachineEventRepository : IEventRepository
{
	public event Action OnEventListChanged;
	public event Action OnUserTypeListChanged;

	#region File Paths generation code
	private static string _eventsFilePath = null;
	private static string _userTypesFilePath = null;

	private static string EventsFilePath
	{
		get
		{
			if (_eventsFilePath == null)
			{
				_eventsFilePath = CalculateEventsFilePath();
			}
			return _eventsFilePath;
		}
	}

	private static string UserTypesFilePath
	{
		get
		{
			if (_userTypesFilePath == null)
			{
				_userTypesFilePath = CalculateUserTypesFilePath();
			}
			return _userTypesFilePath;
		}
	}
	private static string CalculateEventsFilePath()
	{
		return Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
	}

	private static string CalculateUserTypesFilePath()
	{
		return Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonUserTypesFileName", "CalendarTypesOfEventsD"));
	}
	#endregion

	public LocalMachineEventRepository() { }

	#region Events Repository
	private List<IGeneralEventModel> _allEventsList = new List<IGeneralEventModel>();
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
	public async Task ClearAllUserTypesAsync()
	{
		ClearEventsListAsync();
		AllUserEventTypesList.Clear();
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
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
		try
		{
			var directoryPath = Path.GetDirectoryName(EventsFilePath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			if (AllEventsList.Count > 0)
			{
				AllEventsList = AllEventsList.OrderBy(e => e.StartDateTime).ToList();
			}
			var jsonString = JsonConvert.SerializeObject(AllEventsList, settings);
			await File.WriteAllTextAsync(EventsFilePath, jsonString);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "while SaveEventsListAsync");
		}
	}


	public async Task UpdateEventsAsync(IGeneralEventModel eventToUpdate)	// cos nie tak	???
	{
		var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		if (eventToUpdateInList != null)
		{
			// TO CHECK
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
	private List<IUserEventTypeModel> _allUserEventTypesList = new List<IUserEventTypeModel>();
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
		_allEventsList = await GetEventsListAsync();                          // TO CHECK -  ConfigureAwait
		_allUserEventTypesList = await GetUserEventTypesListAsync();          // TO CHECK -  ConfigureAwait
	}
	public async Task<List<IUserEventTypeModel>> GetUserEventTypesListAsync()
	{
		if (File.Exists(UserTypesFilePath))
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
		var selectedEventType = AllUserEventTypesList.FirstOrDefault(e => e.EventTypeName == eventTypeToSelect.EventTypeName);		// TO CHANGE
		return Task.FromResult(selectedEventType);
	}
	public List<IGeneralEventModel> DeepCopyAllEventsList()
	{
		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
		var serialized = JsonConvert.SerializeObject(_allEventsList, settings);
		return JsonConvert.DeserializeObject<List<IGeneralEventModel>>(serialized, settings);
	}
	public List<IUserEventTypeModel> DeepCopyUserEventTypesList()
	{
		var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
		var serialized = JsonConvert.SerializeObject(_allUserEventTypesList, settings);
		return JsonConvert.DeserializeObject<List<IUserEventTypeModel>>(serialized, settings);
	}
	#endregion
}
