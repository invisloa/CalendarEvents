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

		public void AddToEventsList(EventModel eventToAdd)
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
					scheduleList.Add(new EventModel
					{
						StartDateTime = DateTime.Now.AddDays(i),
						EndDateTime = DateTime.Now.AddDays(i).AddHours(1),
						Title = $"Test {i + 1}",
						Description = $"Test {i + 1} Description",
						PriorityLevel = new EventPriority(EnumPriorityLevels.Lowest + i)
					});
				}
				AllEventsList = scheduleList;
			}
				return AllEventsList;
		}

		public void SaveEventsList(List<EventModel> eventListToSave)
		{
			AllEventsList = eventListToSave;
		}
	}
}
