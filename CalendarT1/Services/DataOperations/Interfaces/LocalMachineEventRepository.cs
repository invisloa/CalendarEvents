using CalendarT1.Models;
using Newtonsoft.Json;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public class LocalMachineEventRepository : IEventRepository
	{
		private List<EventModel> _allEventsList;
		public List<EventModel> AllEventsList
		{
			get => _allEventsList;
			set
			{ 
				if (_allEventsList == value) { return;}
				_allEventsList = value;
				SaveEventsListAsync();

			}
		}



		// define file path for your events
		private static readonly string EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory,
														Preferences.Default.Get("ProgramName", "ProgramNameWasNotFound"), "EventsCalendarT1.json");
		public LocalMachineEventRepository()
		{
			LoadEventsListAsync();
		}

		public async Task AddEventAsync(EventModel eventToAdd)
		{
			AllEventsList.Add(eventToAdd);
			SaveEventsListAsync();
		}

		public async Task DeleteFromEventsListAsync(EventModel eventToDelete)
		{
			AllEventsList.Remove(eventToDelete);
			SaveEventsListAsync();
		}


		public async Task ClearEventsListAsync()
		{
			AllEventsList.Clear();
			SaveEventsListAsync();
		}

		public async Task SaveEventsListAsync()
		{
			var directoryPath = Path.GetDirectoryName(EventsFilePath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			var jsonString = JsonConvert.SerializeObject(AllEventsList);
			try
			{
				File.WriteAllText(EventsFilePath, jsonString);
			}
			catch
			{
				throw;
			}
		}

		public async Task UpdateEventAsync(EventModel eventToUpdate)
		{
			// find event in list
			var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
			if (eventToUpdateInList != null)
			{
				// replace the event
				AllEventsList.Remove(eventToUpdateInList);
				AllEventsList.Add(eventToUpdate);
				SaveEventsListAsync();
			}
		}

		public async Task<List<EventModel>> LoadEventsListAsync()
		{
			// Check if the file exists, if it does then load the data
			if (File.Exists(EventsFilePath))
			{
				using var sr = new StreamReader(EventsFilePath);
				var jsonString = await sr.ReadToEndAsync();
				AllEventsList = JsonConvert.DeserializeObject<List<EventModel>>(jsonString);
			}
			return AllEventsList;
		}
	}
}