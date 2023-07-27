using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

public class LocalMachineEventRepository : IEventRepository
{
	private List<AbstractEventModel> _allEventsList;

	public List<AbstractEventModel> AllEventsList
	{
		get
		{
			if (_allEventsList == null)
				GetEventsListAsync().Wait();
			return _allEventsList;
		}
		set
		{
			if (_allEventsList == value) { return; }
			_allEventsList = value;
			SaveEventsListAsync().Wait();
		}
	}

	private static readonly string EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory,
													Preferences.Default.Get("ProgramName", "ProgramNameWasNotFound"), $"{Preferences.Get("JsonFileName", "JsonFileNameDefault")}.json");

	public Task AddEventAsync(AbstractEventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		return SaveEventsListAsync();
	}

	public Task DeleteFromEventsListAsync(AbstractEventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		return SaveEventsListAsync();
	}

	public Task ClearEventsListAsync()
	{
		AllEventsList.Clear();
		return SaveEventsListAsync();
	}

	public async Task<List<AbstractEventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented };
			_allEventsList = JsonConvert.DeserializeObject<List<AbstractEventModel>>(jsonString, settings);
		}
		else
		{
			_allEventsList = new List<AbstractEventModel>();
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

	public Task UpdateEventAsync(AbstractEventModel eventToUpdate)
	{
		var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		if (eventToUpdateInList != null)
		{
			AllEventsList.Remove(eventToUpdateInList);
			AllEventsList.Add(eventToUpdate);
			return SaveEventsListAsync();
		}
		else
		{
			return Task.CompletedTask;
		}
	}

	public Task<AbstractEventModel> GetEventByIdAsync(Guid eventId)
	{
		var selectedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventId);
		return Task.FromResult(selectedEvent);
	}
}
