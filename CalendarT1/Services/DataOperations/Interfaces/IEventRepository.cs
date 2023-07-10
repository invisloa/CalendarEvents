using CalendarT1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.DataOperations.Interfaces
{
	public interface IEventRepository
	{
		public List<EventModel> LoadEventsList();
		public void SaveEventsList();
		public void DeleteFromEventsList(EventModel eventToDelete);
		public void AddEvent(EventModel eventToAdd);
		public void ClearEventsList();
		public void UpdateEvent(EventModel eventToUpdate);


	}
}
