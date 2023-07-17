using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;

public class LocalMachineEventRepository : IEventRepository
{
	private List<EventModel> _allEventsList;

	public List<EventModel> AllEventsList
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
													Preferences.Default.Get("ProgramName", "ProgramNameWasNotFound"), "EventsCalendarT1.json");

	public Task AddEventAsync(EventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		return SaveEventsListAsync();
	}

	public Task DeleteFromEventsListAsync(EventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		return SaveEventsListAsync();
	}

	public Task ClearEventsListAsync()
	{
		AllEventsList.Clear();
		return SaveEventsListAsync();
	}

	public async Task<List<EventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			_allEventsList = JsonConvert.DeserializeObject<List<EventModel>>(jsonString);
		}
		else
		{
			_allEventsList = new List<EventModel>();
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
		var jsonString = JsonConvert.SerializeObject(AllEventsList);
		await File.WriteAllTextAsync(EventsFilePath, jsonString);
	}

	public Task UpdateEventAsync(EventModel eventToUpdate)
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

	public Task<EventModel> GetEventByIdAsync(Guid eventId)
	{
		var selectedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventId);
		return Task.FromResult(selectedEvent);
	}
}
