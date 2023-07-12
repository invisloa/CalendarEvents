using CalendarT1.Models;

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
