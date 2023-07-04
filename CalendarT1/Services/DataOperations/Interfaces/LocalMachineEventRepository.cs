using CalendarT1.Models;
using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public class LocalMachineEventRepository : IEventRepository
	{
		public List<EventModel> AllEventsList { get; set; } = new List<EventModel>();

		public void AddEvent(EventModel eventToAdd)
		{
			AllEventsList.Add(eventToAdd);
		}

		public void DeleteFromEventsList(EventModel eventToDelete)
		{
			AllEventsList.Remove(eventToDelete);
		}

		// preload events list with dummy data
		public List<EventModel> LoadEventsList()
		{
			if (AllEventsList.Count == 0)
			{
				var scheduleList = new List<EventModel>();
				for (int i = 0; i < 5; i++)
				{
					scheduleList.Add(new EventModel($"Test {i + 1}", $"Test {i + 1} Description",
									 new EventPriority(EnumPriorityLevels.Lowest + i),
								     DateTime.Now.AddDays(i), DateTime.Now.AddDays(i).AddHours(1)));
				}
				AllEventsList = scheduleList;
			}
				return AllEventsList;
		}

		public void RemoveEvent(EventModel eventToRemove)
		{
			AllEventsList.Remove(eventToRemove);
		}

		public void SaveEventsList(List<EventModel> eventListToSave)
		{
			AllEventsList = eventListToSave;
		}
		public void UpdateEvent(EventModel eventToUpdate)
		{
			// find event in list
			var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		}
	}
}
