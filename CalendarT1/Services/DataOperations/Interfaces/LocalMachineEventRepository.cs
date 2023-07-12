using CalendarT1.Models;

/* Unmerged change from project 'CalendarT1 (net7.0-android)'
Before:
using System.IO;
using System;
using Microsoft.Maui;
using CalendarT1.Models;
After:
using Microsoft.Maui;
using Newtonsoft.Json;
using System;
using System.IO;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-windows10.0.22621.0)'
Before:
using System.IO;
using System;
using Microsoft.Maui;
using CalendarT1.Models;
After:
using Microsoft.Maui;
using Newtonsoft.Json;
using System;
using System.IO;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-maccatalyst)'
Before:
using System.IO;
using System;
using Microsoft.Maui;
using CalendarT1.Models;
After:
using Microsoft.Maui;
using Newtonsoft.Json;
using System;
using System.IO;
*/
using Newtonsoft.Json;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public class LocalMachineEventRepository : IEventRepository
	{
		public List<EventModel> AllEventsList { get; set; } = new List<EventModel>();


		// define file path for your events
		private static readonly string EventsFilePath = Path.Combine(FileSystem.Current.AppDataDirectory,
														Preferences.Default.Get("ProgramName", "ProgramNameWasNotFound"), "EventsCalendarT1.json");
		public LocalMachineEventRepository()
		{
			LoadEventsList();
		}

		public void AddEvent(EventModel eventToAdd)
		{
			AllEventsList.Add(eventToAdd);
			SaveEventsList();
		}

		public void DeleteFromEventsList(EventModel eventToDelete)
		{
			AllEventsList.Remove(eventToDelete);
			SaveEventsList();
		}

		public List<EventModel> LoadEventsList()
		{
			// Check if the file exists, if it does then load the data
			if (File.Exists(EventsFilePath))
			{
				var jsonString = File.ReadAllText(EventsFilePath);
				AllEventsList = JsonConvert.DeserializeObject<List<EventModel>>(jsonString);
			}
			return AllEventsList;
		}

		public void ClearEventsList()
		{
			AllEventsList.Clear();
			SaveEventsList();
		}

		public void SaveEventsList()
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
			catch (Exception ex)
			{
				string message = ex.Message;
				int i = 0;
			}

		}

		public void UpdateEvent(EventModel eventToUpdate)
		{
			// find event in list
			var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
			if (eventToUpdateInList != null)
			{
				// replace the event
				AllEventsList.Remove(eventToUpdateInList);
				AllEventsList.Add(eventToUpdate);
				SaveEventsList();
			}
		}
	}
}












//using CalendarT1.Models;
//using CalendarT1.Models.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CalendarT1.Services.DataOperations.Interfaces
//{
//	public class LocalMachineEventRepository : IEventRepository
//	{
//		public List<EventModel> AllEventsList { get; set; } = new List<EventModel>();

//		public void AddEvent(EventModel eventToAdd)
//		{
//			AllEventsList.Add(eventToAdd);
//		}

//		public void DeleteFromEventsList(EventModel eventToDelete)
//		{
//			AllEventsList.Remove(eventToDelete);
//		}

//		// preload events list with dummy data
//		public List<EventModel> LoadEventsList()
//		{
//			if (AllEventsList.Count == 0)
//			{
//				var scheduleList = new List<EventModel>();
//				for (int i = 0; i < 5; i++)
//				{
//					scheduleList.Add(new EventModel($"Test {i + 1}", $"Test {i + 1} Description",
//									 new EventPriority(EnumPriorityLevels.Lowest + i),
//								     DateTime.Now.AddDays(i), DateTime.Now.AddDays(i).AddHours(1)));
//				}
//				AllEventsList = scheduleList;
//			}
//				return AllEventsList;
//		}

//		public void RemoveEvent(EventModel eventToRemove)
//		{
//			AllEventsList.Remove(eventToRemove);
//		}

//		public void SaveEventsList(List<EventModel> eventListToSave)
//		{
//			AllEventsList = eventListToSave;
//		}
//		public void UpdateEvent(EventModel eventToUpdate)
//		{
//			// find event in list
//			var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
//		}
//	}
//}
