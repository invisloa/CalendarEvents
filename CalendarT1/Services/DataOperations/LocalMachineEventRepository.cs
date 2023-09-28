using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using Newtonsoft.Json;
using Microsoft.Maui;
using CommunityToolkit.Maui.Storage;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using CalendarT1;
using CalendarT1.Services;
using System.Security.Cryptography;

public class LocalMachineEventRepository : IEventRepository
{
	AdvancedEncryptionStandardService _aesService;



	#region File Paths generation code
	private static string _eventsFilePath = null;
	private static string _userTypesFilePath = null;
	public event Action OnEventListChanged;
	public event Action OnUserTypeListChanged;
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

	public LocalMachineEventRepository()
	{
		string keyBase64 = Convert.ToBase64String(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F });
		string ivBase64 = "MojeSuperHasloXD";


		_aesService = new AdvancedEncryptionStandardService(keyBase64, ivBase64);
	}

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
		await ClearEventsListAsync();
		AllUserEventTypesList.Clear();
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
	}

	public async Task<List<IGeneralEventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			var settings = JsonSerializerSettings_Auto;
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
			var settings = JsonSerializerSettings_Auto;
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


	public async Task UpdateEventsAsync(IGeneralEventModel eventToUpdate)   // cos nie tak	???
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
			var settings = JsonSerializerSettings_Auto;
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
		var settings = JsonSerializerSettings_Auto;
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
		OnUserTypeListChanged?.Invoke();
		await SaveUserEventTypesListAsync();
	}
	public async Task UpdateEventTypeAsync(IUserEventTypeModel eventTypeToUpdate)
	{
		await SaveUserEventTypesListAsync();
		OnUserTypeListChanged?.Invoke();
	}
	public Task GetUserEventTypeAsync(IUserEventTypeModel eventTypeToSelect)
	{
		var selectedEventType = AllUserEventTypesList.FirstOrDefault(e => e.EventTypeName == eventTypeToSelect.EventTypeName);      // TO CHANGE
		return Task.FromResult(selectedEventType);
	}
	public List<IGeneralEventModel> DeepCopyAllEventsList()
	{
		var settings = JsonSerializerSettings_Auto;
		var serialized = JsonConvert.SerializeObject(_allEventsList, settings);
		return JsonConvert.DeserializeObject<List<IGeneralEventModel>>(serialized, settings);
	}
	public List<IUserEventTypeModel> DeepCopyUserEventTypesList()
	{
		var settings = JsonSerializerSettings_Auto;
		var serialized = JsonConvert.SerializeObject(_allUserEventTypesList, settings);
		return JsonConvert.DeserializeObject<List<IUserEventTypeModel>>(serialized, settings);
	}
	#endregion

	//FILE SAVE AND LOAD EVENTS AND TYPES
	#region FILE SAVE AND LOAD
	async Task SaveEventsAndTypesToFile(CancellationToken cancellationToken, List<IGeneralEventModel> eventsToSaveList = null)
	{
		var settings = JsonSerializerSettings_All;
		EventsAndTypesForJson eventsAndTypesToSave;
		// if eventsToSaveList is null, save all events and types
		if (eventsToSaveList == null)
		{
			eventsAndTypesToSave = new EventsAndTypesForJson()
			{
				Events = AllEventsList,
				UserEventTypes = AllUserEventTypesList
			};
		}
		else
		{
			var typesToSaveFromSpecifiedEvents = new List<IUserEventTypeModel>();
			foreach (var eventItem in eventsToSaveList)
			{
				if (!typesToSaveFromSpecifiedEvents.Contains(eventItem.EventType))
				{
					typesToSaveFromSpecifiedEvents.Add(eventItem.EventType);
				}
			}
			eventsAndTypesToSave = new EventsAndTypesForJson()
			{
				Events = eventsToSaveList,
				UserEventTypes = typesToSaveFromSpecifiedEvents
			};
		}

		try
		{
			var jsonString = JsonConvert.SerializeObject(eventsAndTypesToSave, settings);
			var encryptedString = _aesService.EncryptString(jsonString); // Encrypt the jsonString
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(encryptedString)); // Use UTF8 or another Encoding as needed.

			var fileSaverResult = await FileSaver.Default.SaveAsync("EventsList.cics", stream, cancellationToken);
			if (fileSaverResult.IsSuccessful)
			{
				await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
			}
			else
			{
				await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
			}
		}
		catch (Exception ex)
		{
			await Toast.Make($"The file was not saved successfully with error: {ex.Message}").Show(cancellationToken);
		}
	}


	async Task LoadEventsAndTypesFromFile(CancellationToken cancellationToken)
	{
		var settings = JsonSerializerSettings_All;
		var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
	{
		{ DevicePlatform.WinUI, new[] { ".cics" } },
		{ DevicePlatform.Android, new[] { ".cics" } },
		{ DevicePlatform.iOS, new[] { ".cics" } }
	});
		var pickOptions = new PickOptions
		{
			FileTypes = customFileType
		};
		try
		{
			// Prompt the user to select the file
			var filePickerResult = await FilePicker.PickAsync(pickOptions);

			if (filePickerResult != null)
			{
				using var stream = await filePickerResult.OpenReadAsync();
				using var reader = new StreamReader(stream, Encoding.Default); // Use consistent encoding
				var encryptedString = await reader.ReadToEndAsync();
				var jsonString = _aesService.DecryptString(encryptedString); // Decrypt the string

				// Deserialize the content of the file
				var loadedData = JsonConvert.DeserializeObject<EventsAndTypesForJson>(jsonString, settings);


				foreach (var eventItem in loadedData.Events)
				{
					var isEventAlreadyAdded = AllEventsList.Any(e => e.Id == eventItem.Id);
					if (!isEventAlreadyAdded)
					{
						AllEventsList.Add(eventItem);
					}
					else
					{
						// ask the user if he wants to overwrite the event
						var action = await App.Current.MainPage.DisplayActionSheet($"Event {eventItem.Title} already exists", "Cancel", null, "Overwrite", "Duplicate", "Skip");
						switch (action)
						{
							case "Overwrite":
								var eventToUpdate = AllEventsList.FirstOrDefault(e => e.Id == eventItem.Id);
								if (eventToUpdate != null)
								{
									AllEventsList.Remove(eventToUpdate);
									AllEventsList.Add(eventItem);
								}
								break;
							case "Duplicate":
								eventItem.Id = Guid.NewGuid();
								eventItem.Title += " (.)";
								AllEventsList.Add(eventItem);
								break;
							case "Skip":
								// Do nothing, just skip.
								break;
							default:
								// Cancel was selected or back button was pressed.
								break;
						}
					}
				}

				foreach (var eventType in loadedData.UserEventTypes)
				{
					if (!AllUserEventTypesList.Contains(eventType))
					{
						AllUserEventTypesList.Add(eventType);
					}
				}

				await Toast.Make($"Data loaded successfully from: {filePickerResult.FileName}").Show(cancellationToken);
				await SaveEventsListAsync();
				await SaveUserEventTypesListAsync();
			}
			else
			{
				await Toast.Make($"Failed to pick a file: User canceled file picking").Show(cancellationToken);
			}
		}
		catch (Exception ex)
		{
			await Toast.Make($"An error occurred while loading the file: {ex.Message}").Show(cancellationToken);
		}
	}

	private static readonly JsonSerializerSettings JsonSerializerSettings_Auto = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto
	};
	private static readonly JsonSerializerSettings JsonSerializerSettings_All = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.All
	};

	public async Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSaveList = null)
	{

		await SaveEventsAndTypesToFile(CancellationToken.None, eventsToSaveList);
	}
	public async Task LoadEventsAndTypesFromFile()
	{
		await LoadEventsAndTypesFromFile(CancellationToken.None);
	}
	private class EventsAndTypesForJson
	{
		public List<IGeneralEventModel> Events { get; set; }
		public List<IUserEventTypeModel> UserEventTypes { get; set; }
	}


	#endregion
}
